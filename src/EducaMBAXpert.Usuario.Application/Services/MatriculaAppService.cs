using EducaMBAXpert.Contracts.Certificados;
using EducaMBAXpert.Contracts.Cursos;
using EducaMBAXpert.Usuarios.Domain.Entities;
using EducaMBAXpert.Usuarios.Domain.Interfaces;

namespace EducaMBAXpert.Usuarios.Application.Services
{
    public class MatriculaAppService : IMatriculaAppService
    {
        private readonly IMatriculaRepository _matriculaRepository;
        private readonly ICursoConsultaService _cursoConsultaService;
        private readonly ICertificadoService _certificadoService;
        private readonly IUsuarioRepository _usuarioRepository;

        public MatriculaAppService(IMatriculaRepository matriculaRepository,
                                   ICursoConsultaService cursoConsultaService,
                                   ICertificadoService certificadoService,
                                   IUsuarioRepository usuarioRepository)
        {
            _matriculaRepository = matriculaRepository;
            _cursoConsultaService = cursoConsultaService;
            _certificadoService = certificadoService;
            _usuarioRepository = usuarioRepository;
        }

        public async Task ConcluirAula(Guid matriculaId, Guid aulaId)
        {

            //var matricula = await _matriculaRepository.ObterPorIdAsync(matriculaId)
            //                 ?? throw new Exception("Matrícula não encontrada.");

            //matricula.MarcarAulaComoConcluida(aulaId);
            //await _matriculaRepository.Atualizar(matricula);

            //await _matriculaRepository.UnitOfWork.Commit();

            var matricula = await _matriculaRepository.ObterPorIdAsync(matriculaId)
                     ?? throw new Exception("Matrícula não encontrada.");

            var aulaJaConcluida = await _matriculaRepository.AulaJaConcluida(matriculaId, aulaId);

            if (!aulaJaConcluida)
            {
                var novaAula = new AulaConcluida(matriculaId, aulaId);
                await _matriculaRepository.AdicionarAulaConcluida(novaAula);
            }

            await _matriculaRepository.UnitOfWork.Commit();

        }

        public async Task<bool> PodeEmitirCertificado(Guid matriculaId)
        {
            var matricula = await _matriculaRepository.ObterPorIdAsync(matriculaId)
                             ?? throw new Exception("Matrícula não encontrada.");

            var totalAulas = await _cursoConsultaService.ObterTotalAulasPorCurso(matricula.CursoId);
            return matricula.PodeEmitirCertificado(totalAulas);
        }

        public async Task<byte[]> GerarCertificadoPDF(Guid matriculaId)
        {
            var matricula = await _matriculaRepository.ObterPorIdAsync(matriculaId)
                             ?? throw new Exception("Matrícula não encontrada.");

            var totalAulas = await _cursoConsultaService.ObterTotalAulasPorCurso(matricula.CursoId);
            if (!matricula.PodeEmitirCertificado(totalAulas))
                throw new Exception("Aluno ainda não concluiu todas as aulas.");

            var aluno = await _usuarioRepository.ObterPorId(matricula.UsuarioId);
            var curso = await _cursoConsultaService.ObterNomeCurso(matricula.CursoId);

            return _certificadoService.GerarCertificado(aluno.Nome, curso, DateTime.Now);
        }
    }
}
