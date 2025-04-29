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


        Task AdicionarMatriculaCurso(MatriculaInputModel matriculaInputModel);

        Task<MatriculaViewModel> ObterMatriculaPorId(Guid id);

        Task<IEnumerable<MatriculaViewModel>> ObterTodasMatriculasPorUsuarioId(Guid id,bool ativas);

        Task<bool> Inativar(Guid id);
        Task<bool> Ativar(Guid id);
    }
}
