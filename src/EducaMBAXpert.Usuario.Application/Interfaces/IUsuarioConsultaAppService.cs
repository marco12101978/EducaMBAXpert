using EducaMBAXpert.Usuarios.Application.ViewModels;

namespace EducaMBAXpert.Usuarios.Application.Interfaces
{
    public interface IUsuarioConsultaAppService
    {
        Task<IEnumerable<UsuarioViewModel>> ObterTodos();
        Task<UsuarioViewModel> ObterPorId(Guid id);

        Task<MatriculaViewModel> ObterMatriculaPorUsuarioId(Guid id);
        Task<IEnumerable<MatriculaViewModel>> ObterTodasMatriculasPorUsuarioId(Guid id, bool ativas);
    }
}
