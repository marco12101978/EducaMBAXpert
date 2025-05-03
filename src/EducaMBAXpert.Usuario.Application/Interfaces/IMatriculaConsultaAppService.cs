using EducaMBAXpert.Usuarios.Application.ViewModels;

namespace EducaMBAXpert.Usuarios.Application.Interfaces
{
    public interface IMatriculaConsultaAppService
    {
        Task<bool> PodeEmitirCertificado(Guid matriculaId);
        Task<byte[]> GerarCertificadoPDF(Guid matriculaId);
        Task<MatriculaViewModel> ObterMatricula(Guid matriculaId);


    }
}
