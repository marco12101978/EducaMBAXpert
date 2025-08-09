using EducaMBAXpert.Alunos.Application.ViewModels;

namespace EducaMBAXpert.Alunos.Application.Interfaces
{
    public interface IAlunoComandoAppService
    {
        Task Adicionar(AlunoInputModel alunoViewModel);
        Task Atualizar(AlunoInputModel alunoViewModel);
        Task AdicionarEndereco(EnderecoInputModel enderecoViewModel);
        Task AdicionarMatriculaCurso(MatriculaInputModel matriculaInputModel);
        Task AtualizarMatriculaCurso(MatriculaInputModel matriculaInputModel);
        Task<bool> Inativar(Guid id);
        Task<bool> Ativar(Guid id);
    }
}
