namespace EducaMBAXpert.Alunos.Application.Interfaces
{
    public interface ICertificadoAppService
    {
        Task<bool> PodeEmitir(Guid matriculaId, Guid cursoId);
        byte[] GerarCertificado(string nomeAluno, string nomeCurso, DateTime dataEmissao);
    }
}
