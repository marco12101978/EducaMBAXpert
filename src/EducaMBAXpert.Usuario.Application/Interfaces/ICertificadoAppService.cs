namespace EducaMBAXpert.Usuarios.Application.Interfaces
{
    public interface ICertificadoAppService
    {
        Task<bool> PodeEmitir(Guid matriculaId, Guid cursoId);
    }
}
