using EducaMBAXpert.CatalagoCursos.Data.Context;
using EducaMBAXpert.CatalagoCursos.Domain;
using EducaMBAXpert.Core.Data;
using Microsoft.EntityFrameworkCore;

namespace EducaMBAXpert.CatalagoCursos.Data.Repository
{
    public class CursoRepository : ICursoRepository
    {
         private readonly CursoContext _context;

        public CursoRepository(CursoContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;

        public async Task<IEnumerable<Curso>> ObterTodos()
        {
            return await _context.Cursos.AsNoTracking().ToListAsync();  
        }

        public async Task<Curso> ObterPorId(Guid id)
        {
            return await _context.Cursos.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
        }

        public void Adicionar(Curso produto)
        {
            _context.Cursos.Add(produto);
        }

        public void Atualizar(Curso produto)
        {
            _context.Cursos.Update(produto);
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
