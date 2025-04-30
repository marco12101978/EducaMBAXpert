namespace EducaMBAXpert.Usuarios.Application.Services
{
    public interface ICertificadoAppService
    {
        Task<bool> PodeEmitir(Guid matriculaId, Guid cursoId);
    }
}
