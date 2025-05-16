namespace EducaMBAXpert.Alunos.Application.Interfaces
{
    public interface ICertificadoService
    {
        byte[] GerarCertificado(string nomeAluno, string nomeCurso, DateTime dataEmissao);
    }
}
