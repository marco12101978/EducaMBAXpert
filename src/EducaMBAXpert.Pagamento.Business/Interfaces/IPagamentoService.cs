using EducaMBAXpert.Core.DomainObjects.DTO;
using EducaMBAXpert.Pagamentos.Business.Entities;

namespace EducaMBAXpert.Pagamentos.Business.Interfaces
{
    public interface IPagamentoService
    {
        Task<Transacao> RealizarPagamentoPedido(PagamentoAnuidade pagamentoPedido);
    }
}
