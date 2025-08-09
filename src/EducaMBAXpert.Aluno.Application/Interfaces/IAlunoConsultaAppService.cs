using EducaMBAXpert.Alunos.Application.ViewModels;

namespace EducaMBAXpert.Alunos.Application.Interfaces
{
    public interface IAlunoConsultaAppService
    {
        Task<IEnumerable<AlunoViewModel>> ObterTodos();
        Task<AlunoViewModel> ObterPorId(Guid id);

        Task<MatriculaViewModel> ObterMatriculaPorId(Guid id);

        Task<IEnumerable<MatriculaViewModel>> ObterTodasMatriculasPorAlunoId(Guid id, bool ativas);
    }
}
