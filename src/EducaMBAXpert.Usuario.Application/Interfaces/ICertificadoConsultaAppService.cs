namespace EducaMBAXpert.Usuarios.Application.Interfaces
{
    public interface ICertificadoConsultaAppService
    {
        Task<bool> PodeEmitir(Guid matriculaId, Guid cursoId);
    }
}
