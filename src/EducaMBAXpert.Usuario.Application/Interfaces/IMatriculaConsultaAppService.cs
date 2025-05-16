using EducaMBAXpert.Alunos.Application.ViewModels;

namespace EducaMBAXpert.Alunos.Application.Interfaces
{
    public interface IMatriculaConsultaAppService
    {
        Task<bool> PodeEmitirCertificado(Guid matriculaId);
        Task<byte[]> GerarCertificadoPDF(Guid matriculaId);
        Task<MatriculaViewModel> ObterMatricula(Guid matriculaId);


    }
}
