namespace EducaMBAXpert.Alunos.Application.Interfaces
{
    public interface IMatriculaComandoAppService
    {
        Task ConcluirAula(Guid matriculaId, Guid aulaId);
    }
}
