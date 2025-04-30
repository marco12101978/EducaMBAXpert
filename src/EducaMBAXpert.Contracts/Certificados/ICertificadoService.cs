namespace EducaMBAXpert.Contracts.Certificados
{
    public interface ICertificadoService
    {
        byte[] GerarCertificado(string nomeAluno, string nomeCurso, DateTime dataEmissao);
    }
}
