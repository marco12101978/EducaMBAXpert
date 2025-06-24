using EducaMBAXpert.CatalagoCursos.Data.Context;
using EducaMBAXpert.CatalagoCursos.Domain.Entities;
using EducaMBAXpert.CatalagoCursos.Domain.Interfaces;
using EducaMBAXpert.Core.Data;
using EducaMBAXpert.Core.DomainObjects;
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

        public async Task<Result<string>> ObterNomeCurso(Guid cursoId)
        {
            var curso = await _context.Cursos
                            .Include(u => u.Modulos)
                            .ThenInclude(m => m.Aulas)
                            .AsNoTracking()
                            .FirstOrDefaultAsync(p => p.Id == cursoId);

            if (curso == null)
                return Result<string>.Fail("Curso não encontrado.");

            return Result<string>.Ok(curso.Descricao);
        }

        public async Task<Result<bool>> ExisteAulaNoCurso(Guid cursoId, Guid aulaId)
        {
            var curso = await ObterPorId(cursoId);
            if (curso == null)
                return CursoNaoEncontrado<bool>(cursoId);

            var exists = curso.Modulos.SelectMany(m => m.Aulas)
                                      .Any(a => a.Id == aulaId);

            return Result<bool>.Ok(exists);

        }

        public async Task<Result<int>> ObterTotalAulasPorCurso(Guid cursoId)
        {
            var curso = await ObterPorId(cursoId);
            if (curso == null)
                return CursoNaoEncontrado<int>(cursoId);

            var total = curso.Modulos?.Sum(m => m.Aulas?.Count ?? 0) ?? 0;

            return Result<int>.Ok(total);
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

        private Result<T> CursoNaoEncontrado<T>(Guid cursoId)
        {
            var message = $"Curso com ID {cursoId} não encontrado.";
            return Result<T>.Fail(message);
        }


    }
}
