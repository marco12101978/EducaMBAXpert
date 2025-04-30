namespace EducaMBAXpert.Usuarios.Application.Services
{
    public interface IMatriculaAppService
    {
        Task ConcluirAula(Guid matriculaId, Guid aulaId);
        Task<bool> PodeEmitirCertificado(Guid matriculaId);
        Task<byte[]> GerarCertificadoPDF(Guid matriculaId);
    }
}
