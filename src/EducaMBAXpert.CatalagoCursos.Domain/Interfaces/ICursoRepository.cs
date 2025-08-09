using EducaMBAXpert.CatalagoCursos.Domain.Entities;
using EducaMBAXpert.Core.Data;
using EducaMBAXpert.Core.DomainObjects;

namespace EducaMBAXpert.CatalagoCursos.Domain.Interfaces
{
    public interface ICursoRepository : IRepository<Curso>
    {
        Task<IEnumerable<Curso>> ObterTodos();
        Task<Curso> ObterPorId(Guid id);
        Task<Result<string>> ObterNomeCurso(Guid cursoId);
        Task<Result<bool>> ExisteAulaNoCurso(Guid cursoId, Guid aulaId);
        Task<Result<int>> ObterTotalAulasPorCurso(Guid cursoId);

        void Adicionar(Curso curso);
        void Atualizar(Curso curso);
    }
}
