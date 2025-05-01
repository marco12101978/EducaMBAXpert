using EducaMBAXpert.Core.Data;
using EducaMBAXpert.Usuarios.Data.Context;
using EducaMBAXpert.Usuarios.Domain.Entities;
using EducaMBAXpert.Usuarios.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EducaMBAXpert.Usuarios.Data.Repository
{
    public class MatriculaRepository : IMatriculaRepository
    {
        private readonly UsuarioContext _context;
        
        public IUnitOfWork UnitOfWork => _context;

        public MatriculaRepository(UsuarioContext context)
        {
            _context = context;
        }

        public async Task<Matricula> ObterPorIdAsync(Guid id)
        {
            return await _context.Matriculas
                .Include(m => m.AulasConcluidas)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task Atualizar(Matricula matricula)
        {
            _context.Matriculas.Update(matricula);

            foreach (var aula in matricula.AulasConcluidas)
            {
                if (_context.Entry(aula).State == EntityState.Detached)
                {
                    _context.Entry(aula).State = EntityState.Added;
                }
            }

            await Task.CompletedTask;
        }

        public async Task<bool> AulaJaConcluida(Guid matriculaId, Guid aulaId)
        {
            return await _context.AulasConcluidas
                                 .AnyAsync(a => a.MatriculaId == matriculaId && a.AulaId == aulaId);
        }

        public async Task AdicionarAulaConcluida(AulaConcluida aulaConcluida)
        {
            await _context.AulasConcluidas.AddAsync(aulaConcluida);
        }


        public void Dispose()
        {
            _context?.Dispose();
        }


    }
}
