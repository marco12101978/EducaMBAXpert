using EducaMBAXpert.Usuarios.Application.ViewModels;

namespace EducaMBAXpert.Usuarios.Application.Interfaces
{
    public interface IUsuarioComandoAppService
    {
        Task Adicionar(UsuarioViewModel usuarioViewModel);
        Task Atualizar(UsuarioViewModel usuarioViewModel);
        Task AdicionarEndereco(EnderecoViewModel enderecoViewModel);
        Task AdicionarMatriculaCurso(MatriculaInputModel matriculaInputModel);
        Task<bool> Inativar(Guid id);
        Task<bool> Ativar(Guid id);
    }
}
