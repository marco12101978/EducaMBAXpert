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
        private readonly IUsuarioAppService _usuarioAppService;

        public CursosController(ICursoConsultaAppService cursoConsultaAppService,
                                ICursoComandoAppService cursoComandoAppService,
                                IUsuarioAppService usuarioAppService,
                                IMediator mediator,
                                NotificationContext notificationContext,
                                IAppIdentityUser user) : base(mediator, notificationContext, user)
        {
            _cursoConsultaAppService = cursoConsultaAppService;
            _cursoComandoAppService = cursoComandoAppService;
            _usuarioAppService = usuarioAppService;
        }

        [HttpPost("novo")]
        [SwaggerOperation(Summary = "Registra um novo Curso", Description = "Cria um novo Curso com os dados fornecidos.")]
        [ProducesResponseType(typeof(CursoViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> NovoCursoCompleto([FromBody] CursoInputModel curso)
        {
            if (!ModelState.IsValid)
                return CustomResponse(HttpStatusCode.BadRequest);
            
            var sucess = await _cursoComandoAppService.Adicionar(curso);


            return CustomResponse(HttpStatusCode.OK,curso);
        }


        [HttpGet("obter_todos")]
        [SwaggerOperation(Summary = "Obtém todos os cursos", Description = "Retorna uma lista com todos os cursos.")]
        [ProducesResponseType(typeof(IEnumerable<UsuarioViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> ObterTodos()
        {
            var cursos = await _cursoConsultaAppService.ObterTodos();
            return CustomResponse(HttpStatusCode.OK, cursos);
        }


        [HttpGet("obter{id:guid}")]
        [SwaggerOperation(Summary = "Obtém um curso por ID", Description = "Retorna os dados de um curso específico.")]
        [ProducesResponseType(typeof(UsuarioViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> ObterPorId(Guid id)
        {
            var curso = await _cursoConsultaAppService.ObterPorId(id);
            if (curso == null)
            {
                return CustomResponse(HttpStatusCode.NotFound);
            }

            return CustomResponse(HttpStatusCode.OK, curso);
        }

    }
}
