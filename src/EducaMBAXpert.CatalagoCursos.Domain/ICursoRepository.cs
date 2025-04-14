using EducaMBAXpert.Core.Data;

namespace EducaMBAXpert.CatalagoCursos.Domain
{
    public interface ICursoRepository : IRepository<Curso>
    {
        Task<IEnumerable<Curso>> ObterTodos();
        Task<Curso> ObterPorId(Guid id);

        void Adicionar(Curso curso);
        void Atualizar(Curso curso);
    }
}
