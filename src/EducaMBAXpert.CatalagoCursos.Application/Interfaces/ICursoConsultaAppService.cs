using EducaMBAXpert.CatalagoCursos.Application.ViewModels;
using EducaMBAXpert.Core.DomainObjects;

namespace EducaMBAXpert.CatalagoCursos.Application.Interfaces
{
    public interface ICursoConsultaAppService
    {
        Task<IEnumerable<CursoViewModel>> ObterTodos();
        Task<CursoViewModel?> ObterPorId(Guid id);
    }
}
