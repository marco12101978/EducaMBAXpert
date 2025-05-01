using EducaMBAXpert.Api.Authentication;
using EducaMBAXpert.CatalagoCursos.Application.Services;
using EducaMBAXpert.Contracts.Cursos;
using EducaMBAXpert.Core.Messages.CommonMessages.Notifications;
using EducaMBAXpert.Usuarios.Application.Services;
using EducaMBAXpert.Usuarios.Application.ViewModels;
using EducaMBAXpert.Usuarios.Domain.Interfaces;
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
        private readonly IUsuarioAppService _usuarioAppService;
        private readonly ICursoAppService _cursoAppService;
        private readonly IMatriculaAppService _matriculaAppService;
        private readonly ICursoConsultaService _cursoConsultaService;

        public MatriculasController(IMediator mediator,
                                        IUsuarioAppService usuarioAppService,
                                        IMatriculaAppService matriculaAppService,
                                        ICursoAppService cursoAppService,
                                        ICursoConsultaService cursoConsultaService,
                                        NotificationContext notificationContext,
                                        IAppIdentityUser user) : base(mediator, notificationContext, user)
        {
            _usuarioAppService = usuarioAppService;
            _cursoAppService = cursoAppService;
            _matriculaAppService = matriculaAppService;
            _cursoConsultaService = cursoConsultaService;
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

            var usuario = await _usuarioAppService.ObterPorId(idUsuario);

            if (usuario == null)
            {
                NotificarErro("Usuario não encontrado.");
                return CustomResponse(HttpStatusCode.NotFound);
            }

            var curso = _cursoAppService.ObterPorId(matricula.CursoId);

            if (curso == null)
            {
                NotificarErro("Curso não encontrado.");
                return CustomResponse(HttpStatusCode.NotFound);
            }

            matricula.UsuarioId = idUsuario;

            await _usuarioAppService.AdicionarMatriculaCurso(matricula);

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
            var matriculas = await _usuarioAppService.ObterTodasMatriculasPorUsuarioId(idUsuario,true);

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
            var matriculas = await _usuarioAppService.ObterTodasMatriculasPorUsuarioId(idUsuario, false);

            return CustomResponse(HttpStatusCode.OK, matriculas);
        }


        [HttpPost("matricula/{matriculaId}/aula/{aulaId}/concluir")]
        public async Task<IActionResult> ConcluirAula(Guid matriculaId, Guid aulaId)
        {
            var _matriculas = await _matriculaAppService.ObterMatricula(matriculaId);

            var _exite = await _cursoConsultaService.ExiteAulaNoCurso(_matriculas.CursoId, aulaId);

            await _matriculaAppService.ConcluirAula(matriculaId, aulaId);

            return CustomResponse(HttpStatusCode.OK);
        }


        [HttpGet("matricula/{matriculaId}/certificado")]
        public async Task<IActionResult> VerificarCertificado(Guid matriculaId)
        {
            var podeEmitir = await _matriculaAppService.PodeEmitirCertificado(matriculaId);
            return CustomResponse(HttpStatusCode.OK, new { podeEmitir });
        }

        [HttpGet("{matriculaId}/certificado/download")]
        public async Task<IActionResult> BaixarCertificado(Guid matriculaId)
        {
            var pdf = await _matriculaAppService.GerarCertificadoPDF(matriculaId);

            if (pdf == null)
            {
                return CustomResponse(HttpStatusCode.BadRequest);
            }

            return File(pdf, "application/pdf", "Certificado.pdf");
        }


    }
}
