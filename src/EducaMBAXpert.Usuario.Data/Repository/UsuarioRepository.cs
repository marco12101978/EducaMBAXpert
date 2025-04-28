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
            return await _context.Usuarios.Include(u => u.Enderecos).AsNoTracking().ToListAsync();
        }

        public async Task<Usuario> ObterPorId(Guid id)
        {
            return await _context.Usuarios.Include(u => u.Enderecos).AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
        }

        public void Adicionar(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
        }

        public void Atualizar(Usuario usuario)
        {
           _context.Usuarios.Update(usuario);
        }

        public async void AdicionarEndereco(Endereco endereco)
        {
            _context.Enderecos.Add(endereco);
        }


        public void Dispose()
        {
            _context?.Dispose();
        }


    }
}
