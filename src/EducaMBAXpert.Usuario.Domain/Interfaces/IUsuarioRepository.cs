using EducaMBAXpert.Core.Data;

namespace EducaMBAXpert.Usuarios.Domain.Interfaces
{
    public interface IUsuarioRepository : IRepository<Entities.Usuario>
    {
        Task<IEnumerable<Entities.Usuario>> ObterTodos();
        Task<Entities.Usuario> ObterPorId(Guid id);

        void Adicionar(Entities.Usuario usuario);
        void Atualizar(Entities.Usuario usuario);
    }
}
