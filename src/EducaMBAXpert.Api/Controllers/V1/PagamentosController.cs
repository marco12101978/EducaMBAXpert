using EducaMBAXpert.Api.Authentication;
using EducaMBAXpert.Api.ViewModels.Pagamento;
using EducaMBAXpert.Core.Messages.CommonMessages.IntegrationEvents;
using EducaMBAXpert.Core.Messages.CommonMessages.Notifications;
using EducaMBAXpert.Pagamentos.Application.Services;
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
    public class PagamentosController : MainController
    {
        private readonly IMediator _mediator;
        private readonly IPagamentoAppService _pagamentoAppService;
        private readonly IUsuarioAppService _usuarioAppService;

        public PagamentosController(IMediator mediator,
                                    IPagamentoAppService pagamentoAppService,
                                    IUsuarioAppService usuarioAppService,
                                    IAppIdentityUser appIdentityUser,
                                    NotificationContext _notificationContext ) : base(mediator, _notificationContext, appIdentityUser)
        {
            _mediator = mediator;
            _pagamentoAppService = pagamentoAppService;
            _usuarioAppService = usuarioAppService;
        }

        [HttpPost("pagamento")]
        [SwaggerOperation(Summary = "Pagamento", Description = "Executa o pagamento do curso.")]
        [ProducesResponseType(typeof(ObjectResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Pagamento([FromBody] PagamentoCursoViewModel pedido)
        {
            if (!ModelState.IsValid)
                return CustomResponse(HttpStatusCode.BadRequest);

            var usuario = await _usuarioAppService.ObterPorId(pedido.ClienteId);

            if (usuario == null)
            {
                NotificarErro("Usuario não encontrado.");
                return CustomResponse(HttpStatusCode.NotFound);
            }

            var pedidoEvent = new PagamentoCursoEvent(pedido.CursoId,
                                                          pedido.ClienteId,
                                                          pedido.Total,
                                                          pedido.NomeCartao,
                                                          pedido.NumeroCartao,
                                                          pedido.ExpiracaoCartao,
                                                          pedido.CvvCartao);



            await _mediator.Publish(pedidoEvent);

            return CustomResponse(HttpStatusCode.OK);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("obter_todos")]
        [SwaggerOperation(Summary = "Obtém todos os pagamentos", Description = "Retorna uma lista com todos os pagamentos.")]
        [ProducesResponseType(typeof(IEnumerable<UsuarioViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> ObterTodos()
        {
            var usuarios = await _pagamentoAppService.ObterTodos();
            return CustomResponse(HttpStatusCode.OK, usuarios);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("obter{id:guid}")]
        [SwaggerOperation(Summary = "Obtém um pagamento por ID", Description = "Retorna os dados de um pagamento específico.")]
        [ProducesResponseType(typeof(UsuarioViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> ObterPorId(Guid id)
        {
            var usuario = await _pagamentoAppService.ObterPorId(id);
            if (usuario == null)
            {
                NotificarErro("Pagamento não encontrado.");
                return CustomResponse(HttpStatusCode.NotFound);
            }

            return CustomResponse(HttpStatusCode.OK, usuario);
        }


    }
}
