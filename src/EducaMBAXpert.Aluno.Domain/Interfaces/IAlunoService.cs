namespace EducaMBAXpert.Alunos.Domain.Interfaces
{
    public interface IAlunoService : IDisposable
    {
        Task<bool> Inativar(Guid cursoId);
        Task<bool> Ativar(Guid cursoId);
    }
}
