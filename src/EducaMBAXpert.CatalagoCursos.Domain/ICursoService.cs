namespace EducaMBAXpert.CatalagoCursos.Domain
{
    public interface ICursoService : IDisposable
    {
        Task<bool> Inativar(Guid cursoId);
        Task<bool> Ativar(Guid cursoId);
    }
}
