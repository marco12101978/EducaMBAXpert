namespace EducaMBAXpert.Alunos.Application.Interfaces
{
    public interface ICertificadoConsultaAppService
    {
        Task<bool> PodeEmitir(Guid matriculaId, Guid cursoId);
    }
}
