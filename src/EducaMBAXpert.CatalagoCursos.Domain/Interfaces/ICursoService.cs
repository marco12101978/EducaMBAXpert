namespace EducaMBAXpert.CatalagoCursos.Domain.Interfaces
{
    public interface ICursoService : IDisposable
    {
        Task<bool> Inativar(Guid cursoId);
        Task<bool> Ativar(Guid cursoId);
    }
}
