using EducaMBAXpert.CatalagoCursos.Application.ViewModels;
using EducaMBAXpert.Core.DomainObjects;

namespace EducaMBAXpert.CatalagoCursos.Application.Interfaces
{
    public interface ICursoComandoAppService
    {
        Task<Result> Adicionar(CursoInputModel cursoViewModel);
        Task<Result> Atualizar(CursoInputModel cursoViewModel);
        Task<Result> Inativar(Guid id);
        Task<Result> Ativar(Guid id);
    }
}
