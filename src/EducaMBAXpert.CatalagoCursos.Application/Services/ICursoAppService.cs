using EducaMBAXpert.CatalagoCursos.Application.ViewModels;
using EducaMBAXpert.CatalagoCursos.Domain;

namespace EducaMBAXpert.CatalagoCursos.Application.Services
{
    public interface ICursoAppService : IDisposable
    {
        Task<IEnumerable<CursoViewModel>> ObterTodos();
        Task<CursoViewModel> ObterPorId(Guid id);

        void Adicionar(CursoViewModel cursoViewModel);
        void Atualizar(CursoViewModel cursoViewModel);

        bool Inativar(Guid id);
        bool Ativar(Guid id);

    }
}
