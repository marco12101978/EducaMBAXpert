using EducaMBAXpert.Pagamentos.Application.ViewModels;

namespace EducaMBAXpert.Pagamentos.Application.Interfaces
{
    public interface IPagamentoConsultaAppService
    {
        Task<IEnumerable<PagamentoViewModel>> ObterTodos();
        Task<PagamentoViewModel> ObterPorId(Guid id);
    }
}
