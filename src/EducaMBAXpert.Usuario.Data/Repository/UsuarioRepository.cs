using EducaMBAXpert.Core.Data;
using EducaMBAXpert.Usuarios.Data.Context;
using EducaMBAXpert.Usuarios.Domain.Entities;
using EducaMBAXpert.Usuarios.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EducaMBAXpert.Usuarios.Data.Repository
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly UsuarioContext _context;

        public UsuarioRepository(UsuarioContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;

        public async Task<IEnumerable<Usuario>> ObterTodos()
        {
            return await _context.Usuarios
                 .Include(u => u.Enderecos)
                 .Include(u => u.Matriculas) 
                 .AsNoTracking()
                 .ToListAsync();
        }

        public async Task<Usuario?> ObterPorId(Guid id)
        {
            return await _context.Usuarios
                .Include(u => u.Enderecos)
                .Include(u => u.Matriculas) 
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);
        }



        public void Adicionar(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
        }

        public void Atualizar(Usuario usuario)
        {
           _context.Usuarios.Update(usuario);
        }

        public void AdicionarEndereco(Endereco endereco)
        {
            _context.Enderecos.Add(endereco);
        }

        public void AdicionarMatricula(Matricula matricula)
        {
            _context.Matriculas.AddAsync(matricula);
        }

        public void AtualizarMatricula(Matricula matricula)
        {
            _context.Matriculas.Update(matricula);
        }

        public async Task<Matricula?> ObterMatriculaPorId(Guid matriculaId)
        {
            return await _context.Matriculas
                 .AsNoTracking()
                 .FirstOrDefaultAsync(m => m.Id == matriculaId);
        }

        public async Task<IEnumerable<Matricula>> ObterTodasMatriculasPorUsuarioId(Guid Usuarioid,bool ativas)
        {
            var mat = await _context.Matriculas
                .AsNoTracking()
                .Where(m => m.UsuarioId == Usuarioid && m.Ativo == ativas)
                .ToListAsync();

            return await _context.Matriculas
                .AsNoTracking()
                .Where(m => m.UsuarioId == Usuarioid && m.Ativo == ativas)
                .ToListAsync();
        }


        public void Dispose()
        {
            _context?.Dispose();
        }


    }
}
