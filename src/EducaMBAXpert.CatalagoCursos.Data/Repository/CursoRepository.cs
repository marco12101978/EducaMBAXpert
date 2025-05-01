using EducaMBAXpert.CatalagoCursos.Data.Context;
using EducaMBAXpert.CatalagoCursos.Domain.Entities;
using EducaMBAXpert.CatalagoCursos.Domain.Interfaces;
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
            return await _context.Cursos
                         .Include(u => u.Modulos)
                         .ThenInclude(m => m.Aulas)
                         .AsNoTracking()
                         .ToListAsync();  
        }

        public async Task<Curso?> ObterPorId(Guid id)
        {
            return await _context.Cursos
                         .Include(u => u.Modulos)
                         .ThenInclude(m => m.Aulas)
                         .AsNoTracking()
                         .FirstOrDefaultAsync(p => p.Id == id);
        }

        public void Adicionar(Curso curso)
        {
            _context.Cursos.Add(curso);
        }

        public void Atualizar(Curso curso)
        {
            _context.Cursos.Update(curso);
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
