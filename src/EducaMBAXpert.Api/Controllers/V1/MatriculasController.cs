using EducaMBAXpert.Api.Authentication;
using EducaMBAXpert.CatalagoCursos.Application.Interfaces;
using EducaMBAXpert.Contracts.Cursos;
using EducaMBAXpert.Core.Messages.CommonMessages.Notifications;
using EducaMBAXpert.Usuarios.Application.Interfaces;
using EducaMBAXpert.Usuarios.Application.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace EducaMBAXpert.Api.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/matriculas")]
    [Authorize]
    public class MatriculasController : MainController
    {
        private readonly IUsuarioComandoAppService _usuarioComandoAppService;
        private readonly IUsuarioConsultaAppService _usuarioConsultaAppService;
        private readonly IMatriculaConsultaAppService _matriculaConsultaAppService;
        private readonly IMatriculaComandoAppService _matriculaComandoAppService;
        private readonly ICursoConsultaService _cursoConsultaService;
        private readonly ICursoConsultaAppService _cursoConsultaAppService;

        public MatriculasController(IMediator mediator,
                                    IUsuarioConsultaAppService usuarioConsultaAppService,
                                    IUsuarioComandoAppService usuarioComandoAppService,
                                    IMatriculaConsultaAppService matriculaConsultaAppService,
                                    IMatriculaComandoAppService matriculaComandoAppService,
                                    ICursoConsultaService cursoConsultaService,
                                    ICursoConsultaAppService cursoConsultaAppService,
                                    NotificationContext notificationContext,
                                    IAppIdentityUser user) : base(mediator, notificationContext, user)
        {
            _usuarioConsultaAppService = usuarioConsultaAppService;
            _usuarioComandoAppService = usuarioComandoAppService;
            _matriculaConsultaAppService = matriculaConsultaAppService;
            _matriculaComandoAppService = matriculaComandoAppService;
            _cursoConsultaService = cursoConsultaService;
            _cursoConsultaAppService = cursoConsultaAppService;
        }

        [HttpPost("matricular/{idUsuario:guid}")]
        [SwaggerOperation(Summary = "Matricular usuário", Description = "Executa a matrícula no curso.")]
        [ProducesResponseType(typeof(MatriculaInputModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Matricular(Guid idUsuario,[FromBody] MatriculaInputModel matricula)
        {
            if (!ModelState.IsValid || matricula.UsuarioId != idUsuario)
                return CustomResponse(HttpStatusCode.BadRequest);

            var usuario = await _usuarioConsultaAppService.ObterPorId(idUsuario);
            if (usuario == null)
                return NotFoundResponse("Usuário não encontrado.");

            var curso = await _cursoConsultaAppService.ObterPorId(matricula.CursoId);
            if (curso == null)
                return NotFoundResponse("Curso não encontrado.");

            await _usuarioComandoAppService.AdicionarMatriculaCurso(matricula);
            return CustomResponse(HttpStatusCode.OK);
        }


        [Authorize(Roles = "Admin")]
        [HttpGet("usuario/{idUsuario:guid}/matriculas-ativas")]
        [SwaggerOperation(Summary = "Listar matrículas ativas", Description = "Retorna as matrículas ativas por usuário.")]
        [ProducesResponseType(typeof(IEnumerable<MatriculaViewModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ObterAtivasPorIdUsuario(Guid idUsuario)
        {
            var matriculas = await _usuarioConsultaAppService.ObterTodasMatriculasPorUsuarioId(idUsuario,true);
            return CustomResponse(HttpStatusCode.OK, matriculas);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("usuario/{idUsuario:guid}/matriculas-inativas")]
        [SwaggerOperation(Summary = "Listar matrículas inativas", Description = "Retorna as matrículas inativas por usuário.")]
        [ProducesResponseType(typeof(IEnumerable<MatriculaViewModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ObterNaoAtivasPorIdUsuario(Guid idUsuario)
        {
            var matriculas = await _usuarioConsultaAppService.ObterTodasMatriculasPorUsuarioId(idUsuario, false);
            return CustomResponse(HttpStatusCode.OK, matriculas);
        }

        [HttpPost("matricula/{matriculaId}/aula/{aulaId}/concluir")]
        [SwaggerOperation(Summary = "Concluir aula", Description = "Marca uma aula como concluída para a matrícula.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ConcluirAula(Guid matriculaId, Guid aulaId)
        {
            var _matriculas = await _matriculaConsultaAppService.ObterMatricula(matriculaId);
            if (_matriculas == null)
                return NotFoundResponse("Matrícula não encontrada.");

            if (_matriculas.Ativo == false)
                return NotFoundResponse("Matrícula não ativa.");

            var aulaExiste = await _cursoConsultaService.ExisteAulaNoCurso(_matriculas.CursoId, aulaId);
            if (!aulaExiste.Data)
                return NotFoundResponse("Aula não encontrada no curso.");

            await _matriculaComandoAppService.ConcluirAula(matriculaId, aulaId);

            return CustomResponse(HttpStatusCode.OK);
        }


        [HttpGet("matricula/{matriculaId}/certificado")]
        [SwaggerOperation(Summary = "Verificar certificado", Description = "Verifica se o certificado pode ser emitido.")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        public async Task<IActionResult> VerificarCertificado(Guid matriculaId)
        {
            var podeEmitir = await _matriculaConsultaAppService.PodeEmitirCertificado(matriculaId);
            return CustomResponse(HttpStatusCode.OK, new { podeEmitir });
        }

        [HttpGet("matricula/{matriculaId}/certificado/download")]
        [SwaggerOperation(Summary = "Baixar certificado", Description = "Gera e baixa o certificado em PDF.")]
        [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> BaixarCertificado(Guid matriculaId)
        {
            var pdf = await _matriculaConsultaAppService.GerarCertificadoPDF(matriculaId);
            if (pdf == null)
                return CustomResponse(HttpStatusCode.BadRequest, "Não foi possível gerar o certificado.");

            return File(pdf, "application/pdf", "Certificado.pdf");
        }


        private IActionResult NotFoundResponse(string message)
        {
            NotificarErro(message);
            return CustomResponse(HttpStatusCode.NotFound);
        }
    }
}
