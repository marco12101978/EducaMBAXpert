namespace EducaMBAXpert.Usuarios.Domain.Interfaces
{
    public interface IUsuarioService : IDisposable
    {
        Task<bool> Inativar(Guid cursoId);
        Task<bool> Ativar(Guid cursoId);
    }
}
