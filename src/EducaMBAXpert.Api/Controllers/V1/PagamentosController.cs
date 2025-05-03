using EducaMBAXpert.Api.Authentication;
using EducaMBAXpert.Core.Messages.CommonMessages.IntegrationEvents;
using EducaMBAXpert.Core.Messages.CommonMessages.Notifications;
using EducaMBAXpert.Pagamentos.Application.Interfaces;
using EducaMBAXpert.Pagamentos.Application.ViewModels;
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
    [Route("api/v{version:apiVersion}/pagamentos")]
    [Authorize]
    public class PagamentosController : MainController
    {
        private readonly IMediator _mediator;
        private readonly IPagamentoConsultaAppService _pagamentoConsultaAppService;
        private readonly IPagamentoComandoAppService _pagamentoComandoAppService;
        private readonly IUsuarioConsultaAppService _usuarioConsultaAppService;

        public PagamentosController(IMediator mediator,
                                    IPagamentoConsultaAppService pagamentoConsultaAppService,
                                    IPagamentoComandoAppService pagamentoComandoAppService,
                                    IUsuarioConsultaAppService usuarioConsultaAppService,
                                    IAppIdentityUser appIdentityUser,
                                    NotificationContext _notificationContext ) : base(mediator, _notificationContext, appIdentityUser)
        {
            _mediator = mediator;
            _pagamentoConsultaAppService = pagamentoConsultaAppService;
            _pagamentoComandoAppService = pagamentoComandoAppService;
            _usuarioConsultaAppService = usuarioConsultaAppService;
        }

        [HttpPost("pagamento")]
        [SwaggerOperation(Summary = "Executa pagamento", Description = "Executa o pagamento do curso.")]
        [ProducesResponseType(typeof(PagamentoCursoInputModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Pagamento([FromBody] PagamentoCursoInputModel pagamento)
        {
            if (!ModelState.IsValid)
                return CustomResponse(HttpStatusCode.BadRequest);

            var usuario = await _usuarioConsultaAppService.ObterPorId(pagamento.UsuarioId);
            if (usuario == null)
                return NotFoundResponse("Usuário não encontrado.");


            var matricula = await _usuarioConsultaAppService.ObterMatriculaPorUsuarioId(pagamento.MatriculaId);
            if (matricula == null)
                return NotFoundResponse("Matrícula no curso não encontrada.");


            var pedidoEvent = new PagamentoCursoEvent(pagamento.MatriculaId,
                                                      pagamento.UsuarioId,
                                                      pagamento.Total,
                                                      pagamento.NomeCartao,
                                                      pagamento.NumeroCartao,
                                                      pagamento.ExpiracaoCartao,
                                                      pagamento.CvvCartao);



            await _mediator.Publish(pedidoEvent);

            return CustomResponse(HttpStatusCode.OK);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("obter_todos")]
        [SwaggerOperation(Summary = "Obtém todos os pagamentos", Description = "Retorna uma lista com todos os pagamentos.")]
        [ProducesResponseType(typeof(IEnumerable<PagamentoViewModel>), StatusCodes.Status200OK)]
        public async Task<ActionResult> ObterTodos()
        {
            var usuarios = await _pagamentoConsultaAppService.ObterTodos();
            return CustomResponse(HttpStatusCode.OK, usuarios);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("obter/{id:guid}")]
        [SwaggerOperation(Summary = "Obtém pagamento por ID", Description = "Retorna os dados de um pagamento específico.")]
        [ProducesResponseType(typeof(PagamentoViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ObterPorId(Guid id)
        {
            var usuario = await _pagamentoConsultaAppService.ObterPorId(id);
            if (usuario == null)
                return NotFoundResponse("Pagamento não encontrado.");

            return CustomResponse(HttpStatusCode.OK, usuario);
        }

        private IActionResult NotFoundResponse(string message)
        {
            NotificarErro(message);
            return CustomResponse(HttpStatusCode.NotFound);
        }
    }
}
