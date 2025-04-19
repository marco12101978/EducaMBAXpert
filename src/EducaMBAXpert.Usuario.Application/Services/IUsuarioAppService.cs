using EducaMBAXpert.Usuarios.Application.ViewModels;

namespace EducaMBAXpert.Usuarios.Application.Services
{
    public interface IUsuarioAppService : IDisposable
    {
        Task<IEnumerable<UsuarioViewModel>> ObterTodos();
        Task<UsuarioViewModel> ObterPorId(Guid id);

        void Adicionar(UsuarioViewModel usuarioViewModel);
        void Atualizar(UsuarioViewModel usuarioViewModel);

        Task<bool> Inativar(Guid id);
        Task<bool> Ativar(Guid id);
    }
}
