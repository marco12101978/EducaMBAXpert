using EducaMBAXpert.Api.Authentication;
using EducaMBAXpert.CatalagoCursos.Application.Services;
using EducaMBAXpert.Core.Messages.CommonMessages.Notifications;
using EducaMBAXpert.Usuarios.Application.Services;
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
    [Route("api/v{version:apiVersion}/pagamentos")]
    [Authorize]
    public class MatriculaCursoController : MainController
    {
        private readonly IUsuarioAppService _usuarioAppService;
        private readonly ICursoAppService _cursoAppService;
        public MatriculaCursoController(IMediator mediator,
                                        IUsuarioAppService usuarioAppService,
                                        ICursoAppService cursoAppService,
                                        NotificationContext notificationContext,
                                        IAppIdentityUser user) : base(mediator, notificationContext, user)
        {
            _usuarioAppService = usuarioAppService;
            _cursoAppService = cursoAppService;
        }

        [HttpPost("matricular-curso")]
        [SwaggerOperation(Summary = "Matricula", Description = "Executa o matricula no curso.")]
        [ProducesResponseType(typeof(ObjectResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Matricula([FromBody] MatriculaInputModel maticula)
        {
            if (!ModelState.IsValid)
                return CustomResponse(HttpStatusCode.BadRequest);

            var usuario = await _usuarioAppService.ObterPorId(maticula.UsuarioId);

            if (usuario == null)
            {
                NotificarErro("Usuario não encontrado.");
                return CustomResponse(HttpStatusCode.NotFound);
            }

            var curso = _cursoAppService.ObterPorId(maticula.CursoId);

            if (curso == null)
            {
                NotificarErro("Curso não encontrado.");
                return CustomResponse(HttpStatusCode.NotFound);
            }


            await _usuarioAppService.AdicionarMatriculaCurso(maticula);

            return CustomResponse(HttpStatusCode.OK);
        }
    }
}
