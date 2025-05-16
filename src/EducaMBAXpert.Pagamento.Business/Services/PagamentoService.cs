using EducaMBAXpert.Core.Bus;
using EducaMBAXpert.Core.DomainObjects.DTO;
using EducaMBAXpert.Core.Messages.CommonMessages.IntegrationEvents;
using EducaMBAXpert.Core.Messages.CommonMessages.Notifications;
using EducaMBAXpert.Pagamentos.Business.Entities;
using EducaMBAXpert.Pagamentos.Business.Enum;
using EducaMBAXpert.Pagamentos.Business.Interfaces;
using EducaMBAXpert.Pagamentos.Business.Models;

namespace EducaMBAXpert.Pagamentos.Business.Services
{
    public class PagamentoService : IPagamentoService
    {
        private readonly IPagamentoCartaoCreditoFacade _pagamentoCartaoCreditoFacade;
        private readonly IPagamentoRepository _pagamentoRepository;
        private readonly IMediatrHandler _mediatorHandler;

        public PagamentoService(IPagamentoCartaoCreditoFacade pagamentoCartaoCreditoFacade,
                                IPagamentoRepository pagamentoRepository,
                                IMediatrHandler mediatorHandler)
        {
            _pagamentoCartaoCreditoFacade = pagamentoCartaoCreditoFacade;
            _pagamentoRepository = pagamentoRepository;
            _mediatorHandler = mediatorHandler;
        }

        public async Task<Transacao> RealizarPagamentoPedido(PagamentoCurso pagamentoAnuidade)
        {
            var pedido = new CobrancaCurso
            {
                Id = pagamentoAnuidade.CursoId,
                Valor = pagamentoAnuidade.Total
            };

            var pagamento = new Pagamento
            {
                Valor = pagamentoAnuidade.Total,
                NomeCartao = pagamentoAnuidade.NomeCartao,
                NumeroCartao = pagamentoAnuidade.NumeroCartao,
                ExpiracaoCartao = pagamentoAnuidade.ExpiracaoCartao,
                CvvCartao = pagamentoAnuidade.CvvCartao,
                CobrancaCursoId = pagamentoAnuidade.CursoId,
                AlunoId = pagamentoAnuidade.ClienteId
            };

            var transacao = _pagamentoCartaoCreditoFacade.RealizarPagamento(pedido, pagamento);

            if (transacao.StatusTransacao == StatusTransacao.Pago)
            {
                pagamento.AdicionarEvento(new PagamentoRealizadoEvent(pedido.Id, pagamentoAnuidade.ClienteId, transacao.PagamentoId, transacao.Id, pedido.Valor));

                pagamento.Status = transacao.StatusTransacao.ToString();
                pagamento.Transacao = transacao;

                _pagamentoRepository.Adicionar(pagamento);
                _pagamentoRepository.AdicionarTransacao(transacao);

                await _pagamentoRepository.UnitOfWork.Commit();
                return transacao;
            }

            await _mediatorHandler.PublicarNotificacao(new DomainNotification("pagamento", "A operadora recusou o pagamento"));
            await _mediatorHandler.PublicarEvento(new PagamentoRecusadoEvent(pedido.Id, pagamentoAnuidade.ClienteId, transacao.PagamentoId, transacao.Id, pedido.Valor));

            return transacao;
        }
    }
}
