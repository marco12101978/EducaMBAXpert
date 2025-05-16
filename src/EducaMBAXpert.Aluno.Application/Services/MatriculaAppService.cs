using AutoMapper;
using EducaMBAXpert.Contracts.Certificados;
using EducaMBAXpert.Contracts.Cursos;
using EducaMBAXpert.Core.Bus;
using EducaMBAXpert.Core.DomainObjects;
using EducaMBAXpert.Core.Messages.CommonMessages.Notifications;
using EducaMBAXpert.Alunos.Application.Interfaces;
using EducaMBAXpert.Alunos.Application.ViewModels;
using EducaMBAXpert.Alunos.Domain.Entities;
using EducaMBAXpert.Alunos.Domain.Interfaces;
using MediatR;

namespace EducaMBAXpert.Alunos.Application.Services
{
    public class MatriculaAppService : IMatriculaComandoAppService , IMatriculaConsultaAppService
    {
        private readonly IMatriculaRepository _matriculaRepository;
        private readonly ICursoConsultaService _cursoConsultaService;
        private readonly ICertificadoService _certificadoService;
        private readonly IAlunoRepository _alunoRepository;
        private readonly IMediatrHandler _mediatrHandler;
        private readonly IMapper _mapper;

        public MatriculaAppService(IMatriculaRepository matriculaRepository,
                                   ICursoConsultaService cursoConsultaService,
                                   ICertificadoService certificadoService,
                                   IAlunoRepository alunoRepository,
                                   IMediatrHandler mediatrHandler,
                                   IMapper mapper)
        {
            _matriculaRepository = matriculaRepository;
            _cursoConsultaService = cursoConsultaService;
            _certificadoService = certificadoService;
            _alunoRepository = alunoRepository;
            _mediatrHandler = mediatrHandler;
            _mapper = mapper;
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


            var totalAulas = await _cursoConsultaService.ObterTotalAulasPorCurso(matricula.CursoId);
            return matricula.PodeEmitirCertificado(totalAulas.Data);
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

            var totalAulas = await _cursoConsultaService.ObterTotalAulasPorCurso(matricula.CursoId);
            if (!matricula.PodeEmitirCertificado(totalAulas.Data))
            {
                await _mediatrHandler.PublicarNotificacao(new DomainNotification("GerarCertificadoPDF",
                                                                                 "Aluno ainda não concluiu todas as aulas."));
                return null;
            }

            Aluno aluno = await _alunoRepository.ObterPorId(matricula.AlunoId);
            Result<string> curso = await _cursoConsultaService.ObterNomeCurso(matricula.CursoId);

            return _certificadoService.GerarCertificado(aluno.Nome, curso.Data, DateTime.Now);
        }

        public async Task<MatriculaViewModel> ObterMatricula(Guid matriculaId)
        {
            return _mapper.Map<MatriculaViewModel>(await _matriculaRepository.ObterPorIdAsync(matriculaId));
        }
    }
}
