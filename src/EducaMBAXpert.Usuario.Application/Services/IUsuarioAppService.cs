using EducaMBAXpert.Usuarios.Application.ViewModels;
using EducaMBAXpert.Usuarios.Domain.Entities;

namespace EducaMBAXpert.Usuarios.Application.Services
{
    public interface IUsuarioAppService : IDisposable
    {
        Task<IEnumerable<UsuarioViewModel>> ObterTodos();
        Task<UsuarioViewModel> ObterPorId(Guid id);

        Task Adicionar(UsuarioViewModel usuarioViewModel);
        Task Atualizar(UsuarioViewModel usuarioViewModel);

        Task AdicionarEndereco(EnderecoViewModel enderecoViewModel);



        Task<bool> Inativar(Guid id);
        Task<bool> Ativar(Guid id);
    }
}
