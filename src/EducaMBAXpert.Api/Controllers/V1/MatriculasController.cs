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
        [SwaggerOperation(Summary = "Matricula", Description = "Executa o matricula no curso.")]
        [ProducesResponseType(typeof(ObjectResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Matricula(Guid idUsuario,[FromBody] MatriculaInputModel matricula)
        {
            if (!ModelState.IsValid)
                return CustomResponse(HttpStatusCode.BadRequest);

            if (matricula.UsuarioId != idUsuario)
                return CustomResponse(HttpStatusCode.BadRequest);

            var usuario = await _usuarioConsultaAppService.ObterPorId(idUsuario);

            if (usuario == null)
            {
                NotificarErro("Usuario não encontrado.");
                return CustomResponse(HttpStatusCode.NotFound);
            }

            var curso = _cursoConsultaAppService.ObterPorId(matricula.CursoId);

            if (curso == null)
                return CustomResponse(HttpStatusCode.NotFound);

            await _usuarioComandoAppService.AdicionarMatriculaCurso(matricula);

            return CustomResponse(HttpStatusCode.OK);
        }


        [Authorize(Roles = "Admin")]
        [HttpGet("obter-matriculas-ativas-usuario/{idUsuario:guid}")]
        [SwaggerOperation(Summary = "Obtém Matriculas por ID cliente", Description = "Retorna as Matriculas por Usuario específico.")]
        [ProducesResponseType(typeof(UsuarioViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> ObterAtivasPorIdUsuario(Guid idUsuario)
        {
            var matriculas = await _usuarioConsultaAppService.ObterTodasMatriculasPorUsuarioId(idUsuario,true);

            return CustomResponse(HttpStatusCode.OK, matriculas);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("obter-matriculas-nao-ativas-usuario/{idUsuario:guid}")]
        [SwaggerOperation(Summary = "Obtém Matriculas por ID cliente", Description = "Retorna as Matriculas por Usuario específico.")]
        [ProducesResponseType(typeof(UsuarioViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> ObterNaoAtivasPorIdUsuario(Guid idUsuario)
        {
            var matriculas = await _usuarioConsultaAppService.ObterTodasMatriculasPorUsuarioId(idUsuario, false);

            return CustomResponse(HttpStatusCode.OK, matriculas);
        }


        [HttpPost("matricula/{matriculaId}/aula/{aulaId}/concluir")]
        public async Task<IActionResult> ConcluirAula(Guid matriculaId, Guid aulaId)
        {
            var _matriculas = await _matriculaConsultaAppService.ObterMatricula(matriculaId);

            if (_matriculas == null)
            {
                NotificarErro("Matricula não encontrada .");
                return CustomResponse(HttpStatusCode.NotFound);
            }


            var result = await _cursoConsultaService.ExisteAulaNoCurso(_matriculas.CursoId, aulaId);
            if (!result.Data)
            {
                NotificarErro("Aula não encontrada no Curso.");
                return CustomResponse(HttpStatusCode.NotFound);
            }

            await _matriculaComandoAppService.ConcluirAula(matriculaId, aulaId);

            return CustomResponse(HttpStatusCode.OK);
        }


        [HttpGet("matricula/{matriculaId}/certificado")]
        public async Task<IActionResult> VerificarCertificado(Guid matriculaId)
        {
            var podeEmitir = await _matriculaConsultaAppService.PodeEmitirCertificado(matriculaId);
            return CustomResponse(HttpStatusCode.OK, new { podeEmitir });
        }

        [HttpGet("{matriculaId}/certificado/download")]
        public async Task<IActionResult> BaixarCertificado(Guid matriculaId)
        {
            var pdf = await _matriculaConsultaAppService.GerarCertificadoPDF(matriculaId);

            if (pdf == null)
            {
                return CustomResponse(HttpStatusCode.BadRequest);
            }

            return File(pdf, "application/pdf", "Certificado.pdf");
        }


    }
}
