using EducaMBAXpert.Core.DomainObjects.DTO;
using EducaMBAXpert.Core.Messages.CommonMessages.IntegrationEvents;
using EducaMBAXpert.Pagamentos.Business.Interfaces;
using MediatR;

namespace EducaMBAXpert.Pagamentos.Business.Events
{
    public class PagamentoEventHandler : INotificationHandler<PagamentoCursoEvent>
    {
        private readonly IPagamentoService _pagamentoService;

        public PagamentoEventHandler(IPagamentoService pagamentoService)
        {
            _pagamentoService = pagamentoService;
        }

        public async Task Handle(PagamentoCursoEvent message, CancellationToken cancellationToken)
        {
            var pagamentoPedido = new PagamentoCurso
            {
                CursoId = message.PedidoId,
                ClienteId = message.ClienteId,
                Total = message.Total,
                NomeCartao = message.NomeCartao,
                NumeroCartao = message.NumeroCartao,
                ExpiracaoCartao = message.ExpiracaoCartao,
                CvvCartao = message.CvvCartao
            };

            await _pagamentoService.RealizarPagamentoPedido(pagamentoPedido);
        }
    }
}
