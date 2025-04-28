using EducaMBAXpert.Core.Data;
using EducaMBAXpert.Usuarios.Domain.Entities;

namespace EducaMBAXpert.Usuarios.Domain.Interfaces
{
    public interface IUsuarioRepository : IRepository<Entities.Usuario>
    {
        Task<IEnumerable<Entities.Usuario>> ObterTodos();
        Task<Entities.Usuario> ObterPorId(Guid id);

        void Adicionar(Entities.Usuario usuario);
        void Atualizar(Entities.Usuario usuario);
        void AdicionarEndereco(Endereco endereco);



    }
}
