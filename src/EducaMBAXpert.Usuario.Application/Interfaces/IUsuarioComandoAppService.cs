using EducaMBAXpert.Usuarios.Application.ViewModels;

namespace EducaMBAXpert.Usuarios.Application.Interfaces
{
    public interface IUsuarioComandoAppService
    {
        Task Adicionar(UsuarioInputModel usuarioViewModel);
        Task Atualizar(UsuarioInputModel usuarioViewModel);
        Task AdicionarEndereco(EnderecoInputModel enderecoViewModel);
        Task AdicionarMatriculaCurso(MatriculaInputModel matriculaInputModel);
        Task<bool> Inativar(Guid id);
        Task<bool> Ativar(Guid id);
    }
}
