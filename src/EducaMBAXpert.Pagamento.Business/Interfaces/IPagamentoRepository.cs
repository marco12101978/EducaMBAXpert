using EducaMBAXpert.Core.Data;
using EducaMBAXpert.Pagamentos.Business.Entities;

namespace EducaMBAXpert.Pagamentos.Business.Interfaces
{
    public interface IPagamentoRepository : IRepository<Pagamento>
    {
        Task<IEnumerable<Pagamento>> ObterTodos();
        Task<Pagamento> ObterPorId(Guid id);

        void Adicionar(Pagamento pagamento);

        void AdicionarTransacao(Transacao transacao);
    }
}
