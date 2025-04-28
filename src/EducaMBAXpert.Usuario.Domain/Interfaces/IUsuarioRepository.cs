using EducaMBAXpert.Core.Data;
using EducaMBAXpert.Usuarios.Domain.Entities;

namespace EducaMBAXpert.Usuarios.Domain.Interfaces
{
    public interface IUsuarioRepository : IRepository<Entities.Usuario>
    {
        Task<IEnumerable<Usuario>> ObterTodos();
        Task<Usuario> ObterPorId(Guid id);
        Task<Matricula> ObterMatriculaPorId(Guid matriculaId);

        void Adicionar(Entities.Usuario usuario);
        void Atualizar(Entities.Usuario usuario);
        void AdicionarEndereco(Endereco endereco);
        void AdicionarMatricula(Matricula matricula);


    }
}
