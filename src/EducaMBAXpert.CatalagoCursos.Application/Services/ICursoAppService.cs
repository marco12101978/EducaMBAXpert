using EducaMBAXpert.CatalagoCursos.Application.ViewModels;
using EducaMBAXpert.CatalagoCursos.Domain;

namespace EducaMBAXpert.CatalagoCursos.Application.Services
{
    public interface ICursoAppService : IDisposable
    {
        Task<IEnumerable<CursoViewModel>> ObterTodos();
        Task<CursoViewModel> ObterPorId(Guid id);

        void Adicionar(CursoCompletoViewModel cursoViewModel);
        void Atualizar(CursoCompletoViewModel cursoViewModel);

        Task<bool> Inativar(Guid id);
        Task<bool> Ativar(Guid id);

    }
}
