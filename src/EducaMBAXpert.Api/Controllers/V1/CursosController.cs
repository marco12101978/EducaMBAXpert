using EducaMBAXpert.Api.Authentication;
using EducaMBAXpert.Api.ViewModels.User;
using EducaMBAXpert.CatalagoCursos.Application.Interfaces;
using EducaMBAXpert.CatalagoCursos.Application.ViewModels;
using EducaMBAXpert.Core.Messages.CommonMessages.Notifications;
using EducaMBAXpert.Usuarios.Application.Interfaces;
using EducaMBAXpert.Usuarios.Application.ViewModels;
using EducaMBAXpert.Usuarios.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace EducaMBAXpert.Api.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/catalogo_curso")]
    [Authorize(Roles = "Admin")]
    public class CursosController : MainController
    {
        private readonly ICursoConsultaAppService _cursoConsultaAppService;
        private readonly ICursoComandoAppService _cursoComandoAppService;

        public CursosController(ICursoConsultaAppService cursoConsultaAppService,
                                ICursoComandoAppService cursoComandoAppService,
                                IMediator mediator,
                                NotificationContext notificationContext,
                                IAppIdentityUser user) : base(mediator, notificationContext, user)
        {
            _cursoConsultaAppService = cursoConsultaAppService;
            _cursoComandoAppService = cursoComandoAppService;
        }

        [HttpPost("novo")]
        [SwaggerOperation(Summary = "Registra um novo curso", Description = "Cria um novo curso com os dados fornecidos.")]
        [ProducesResponseType(typeof(CursoViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> NovoCursoCompleto([FromBody] CursoInputModel curso)
        {
            if (!ModelState.IsValid)
                return CustomResponse(HttpStatusCode.BadRequest);
            
            await _cursoComandoAppService.Adicionar(curso);
            return CustomResponse(HttpStatusCode.OK,curso);
        }


        [HttpGet("obter_todos")]
        [SwaggerOperation(Summary = "Obtém todos os cursos", Description = "Retorna uma lista com todos os cursos.")]
        [ProducesResponseType(typeof(IEnumerable<CursoViewModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ObterTodos()
        {
            var cursos = await _cursoConsultaAppService.ObterTodos();
            return CustomResponse(HttpStatusCode.OK, cursos);
        }

        [HttpGet("obter/{id:guid}")]
        [SwaggerOperation(Summary = "Obtém um curso por ID", Description = "Retorna os dados de um curso específico.")]
        [ProducesResponseType(typeof(CursoViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ObterPorId(Guid id)
        {
            var curso = await _cursoConsultaAppService.ObterPorId(id);
            if (curso == null)
                return NotFoundResponse("Curso não encontrado.");

            return CustomResponse(HttpStatusCode.OK, curso);
        }


        private IActionResult NotFoundResponse(string message)
        {
            NotificarErro(message);
            return CustomResponse(HttpStatusCode.NotFound);
        }
    }
}
