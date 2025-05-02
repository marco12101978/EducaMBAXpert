using EducaMBAXpert.Pagamentos.AntiCorruption.Interfaces;
using EducaMBAXpert.Pagamentos.Business.Entities;
using EducaMBAXpert.Pagamentos.Business.Enum;
using EducaMBAXpert.Pagamentos.Business.Interfaces;
using EducaMBAXpert.Pagamentos.Business.Models;

namespace EducaMBAXpert.Pagamentos.AntiCorruption.Services
{
    public class PagamentoCartaoCreditoFacade : IPagamentoCartaoCreditoFacade
    {
        private readonly IPayPalGateway _payPalGateway;
        private readonly IConfigurationManager _configManager;

        public PagamentoCartaoCreditoFacade(IPayPalGateway payPalGateway, IConfigurationManager configManager)
        {
            _payPalGateway = payPalGateway;
            _configManager = configManager;
        }

        public Transacao RealizarPagamento(CobrancaCurso cobrancaCurso, Pagamento pagamento)
        {
            var apiKey = _configManager.GetValue("apiKey");
            var encriptionKey = _configManager.GetValue("encriptionKey");

            var serviceKey = _payPalGateway.GetPayPalServiceKey(apiKey, encriptionKey);
            var cardHashKey = _payPalGateway.GetCardHashKey(serviceKey, pagamento.NumeroCartao);

            var pagamentoResult = _payPalGateway.CommitTransaction(cardHashKey, cobrancaCurso.Id.ToString(), pagamento.Valor);

            var transacao = new Transacao
            {
                CobrancaCursoId = cobrancaCurso.Id,
                Total = cobrancaCurso.Valor,
                PagamentoId = pagamento.Id
            };

            if (pagamentoResult)
            {
                transacao.StatusTransacao = StatusTransacao.Pago;
                return transacao;
            }

            transacao.StatusTransacao = StatusTransacao.Recusado;
            return transacao;
        }
    }
}
