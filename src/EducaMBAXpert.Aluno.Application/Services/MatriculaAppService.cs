using AutoMapper;
using EducaMBAXpert.Alunos.Application.Interfaces;
using EducaMBAXpert.Alunos.Application.ViewModels;
using EducaMBAXpert.Alunos.Domain.Entities;
using EducaMBAXpert.Alunos.Domain.Interfaces;
using EducaMBAXpert.Core.Bus;
using EducaMBAXpert.Core.Messages.CommonMessages.Notifications;
using EducaMBAXpert.Core.Messages.CommonMessages.Queries;
using QuestPDF.Fluent;
using QuestPDF.Helpers;

namespace EducaMBAXpert.Alunos.Application.Services
{
    public class MatriculaAppService : IMatriculaComandoAppService , IMatriculaConsultaAppService
    {
        private readonly IMatriculaRepository _matriculaRepository;
        private readonly IAlunoRepository _alunoRepository;
        private readonly IMediatrHandler _mediatrHandler;
        private readonly IMapper _mapper;

        public MatriculaAppService(IMatriculaRepository matriculaRepository,
                                   IAlunoRepository alunoRepository,
                                   IMediatrHandler mediatrHandler,
                                   IMapper mapper)
        {
            _matriculaRepository = matriculaRepository;
            _alunoRepository = alunoRepository;
            _mediatrHandler = mediatrHandler;
            _mapper = mapper;

            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

        }

        public async Task ConcluirAula(Guid matriculaId, Guid aulaId)
        {

            var matricula = await _matriculaRepository.ObterPorIdAsync(matriculaId)
                     ?? throw new Exception("Matrícula não encontrada.");

            var aulaJaConcluida = await _matriculaRepository.AulaJaConcluida(matriculaId, aulaId);

            if (!aulaJaConcluida)
            {

                var novaAula = new AulaConcluida(matriculaId, aulaId);
                await _matriculaRepository.AdicionarAulaConcluida(novaAula);
            }
            else
            {
                await _mediatrHandler.PublicarNotificacao(new DomainNotification("ConcluirAula",
                                                                                 "Aula já se encontra concluida"));
                return;
            }


            await AtualizarPercentualConclusao(matricula);

            await _matriculaRepository.UnitOfWork.Commit();

        }



        public async Task<bool> PodeEmitirCertificado(Guid matriculaId)
        {
            Matricula? matricula = await _matriculaRepository.ObterPorIdAsync(matriculaId);

            if (matricula == null)
            {
                await _mediatrHandler.PublicarNotificacao(new DomainNotification("PodeEmitirCertificado",
                                                                                 "Matrícula não encontrada."));
                return false;
            }
            var totalAulas = await _mediatrHandler.EnviarQuery<ObterTotalAulasPorCursoQuery, Int32>(new ObterTotalAulasPorCursoQuery(matricula.CursoId));

            return matricula.PodeEmitirCertificado(Convert.ToInt32(totalAulas));
        }

        public async Task<byte[]> GerarCertificadoPDF(Guid matriculaId)
        {
            var matricula = await _matriculaRepository.ObterPorIdAsync(matriculaId);

            if (matricula == null)
            {
                await _mediatrHandler.PublicarNotificacao(new DomainNotification("GerarCertificadoPDF",
                                                                                 "Matrícula não encontrada."));
                return null;
            }

            var totalAulas = await _mediatrHandler.EnviarQuery<ObterTotalAulasPorCursoQuery, Int32>(new ObterTotalAulasPorCursoQuery(matricula.CursoId));

            if (!matricula.PodeEmitirCertificado(Convert.ToInt32(totalAulas)))
            {
                await _mediatrHandler.PublicarNotificacao(new DomainNotification("GerarCertificadoPDF",
                                                                                 "Aluno ainda não concluiu todas as aulas."));
                return null;
            }

            Aluno aluno = await _alunoRepository.ObterPorId(matricula.AlunoId);

            var nomeCurso = await _mediatrHandler.EnviarQuery<ObterNomeCursoQuery, string>(new ObterNomeCursoQuery(matricula.CursoId));

            return GerarCertificado(aluno.Nome, nomeCurso, DateTime.Now);
        }

        public async Task<MatriculaViewModel> ObterMatricula(Guid matriculaId)
        {
            return _mapper.Map<MatriculaViewModel>(await _matriculaRepository.ObterPorIdAsync(matriculaId));
        }

        private byte[] GerarCertificado(string nomeAluno, string nomeCurso, DateTime dataEmissao)
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

        private async Task AtualizarPercentualConclusao(Matricula matricula)
        {
            var totalAulasCurso = await _mediatrHandler.EnviarQuery<ObterTotalAulasPorCursoQuery, Int32>(new ObterTotalAulasPorCursoQuery(matricula.CursoId));

            var aulasConcluidas = matricula.ObterTotalAlunasConcluidas() + 1;

            if (aulasConcluidas > 0 && totalAulasCurso > 0)
            {
                decimal percentual = (decimal)aulasConcluidas / totalAulasCurso * 100;
                matricula.DefinirPercentualConclusao(Math.Round(percentual, 2));
                _alunoRepository.AtualizarMatricula(matricula);
                await _alunoRepository.UnitOfWork.Commit();
            }
        }
    }
}
