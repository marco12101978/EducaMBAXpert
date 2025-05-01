using EducaMBAXpert.Contracts.Certificados;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace EducaMBAXpert.Certificados.Services
{
    public class CertificadoService : ICertificadoService
    {
        public CertificadoService()
        {
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;
        }
        public byte[] GerarCertificado(string nomeAluno, string nomeCurso, DateTime dataEmissao)
        {
            var documento = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(50);
                    page.PageColor(Colors.White);

                    page.Content()
                        .Column(coluna =>
                        {
                            coluna.Spacing(20);

                            coluna.Item().AlignCenter().Text("CERTIFICADO DE CONCLUSÃO").FontSize(24).Bold();
                            coluna.Item().AlignCenter().Text($"Certificamos que {nomeAluno}").FontSize(18);
                            coluna.Item().AlignCenter().Text($"concluiu o curso {nomeCurso}").FontSize(18);
                            coluna.Item().AlignCenter().Text($"em {dataEmissao:dd/MM/yyyy}").FontSize(16);

                            coluna.Item().Height(100);


                            coluna.Item().AlignRight().Text("__________________________").FontSize(14);
                            coluna.Item().AlignRight().Text("Assinatura da Instituição").FontSize(12);
                        });
                });
            });





            using var memoryStream = new System.IO.MemoryStream();
            documento.GeneratePdf(memoryStream);
            return memoryStream.ToArray();
        }
    }
}
