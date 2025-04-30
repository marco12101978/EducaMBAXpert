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
            await Task.CompletedTask;
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
