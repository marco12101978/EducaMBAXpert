using AutoMapper;
using EducaMBAXpert.Alunos.Application.Services;
using EducaMBAXpert.Alunos.Application.ViewModels;
using EducaMBAXpert.Alunos.Domain.Entities;
using EducaMBAXpert.Alunos.Domain.Interfaces;
using EducaMBAXpert.Core.Bus;
using EducaMBAXpert.Core.Data;
using EducaMBAXpert.Core.DomainObjects;
using EducaMBAXpert.Core.Messages.CommonMessages.Notifications;
using Moq;

namespace EducaMBAXpert.Alunos.Test
{
    public class MatriculaAppServiceTestes
    {
        private readonly Mock<IMatriculaRepository> _matriculaRepositoryMock;
        private readonly Mock<ICursoConsultaService> _cursoConsultaServiceMock;
        private readonly Mock<IAlunoRepository> _alunoRepositoryMock;
        private readonly Mock<IMediatrHandler> _mediatrHandlerMock;
        private readonly Mock<IMapper> _mapperMock;

        private readonly MatriculaAppService _matriculaAppService;

        public MatriculaAppServiceTestes()
        {
            _matriculaRepositoryMock = new Mock<IMatriculaRepository>();
            _cursoConsultaServiceMock = new Mock<ICursoConsultaService>();
            _alunoRepositoryMock = new Mock<IAlunoRepository>();
            _mediatrHandlerMock = new Mock<IMediatrHandler>();
            _mapperMock = new Mock<IMapper>();

            _matriculaAppService = new MatriculaAppService(
                _matriculaRepositoryMock.Object,
                _cursoConsultaServiceMock.Object,
                _alunoRepositoryMock.Object,
                _mediatrHandlerMock.Object,
                _mapperMock.Object);
        }

        [Fact(DisplayName = "Concluir aula quando não concluída deve adicionar e commitar")]
        [Trait("Matricula", "AppService")]
        public async Task ConcluirAula_DeveAdicionarAulaConcluida_QuandoNaoConcluida()
        {
            var matriculaId = Guid.NewGuid();
            var aulaId = Guid.NewGuid();
            var matricula = new Matricula(Guid.NewGuid(), Guid.NewGuid());

            _matriculaRepositoryMock.Setup(r => r.ObterPorIdAsync(matriculaId)).ReturnsAsync(matricula);
            _matriculaRepositoryMock.Setup(r => r.AulaJaConcluida(matriculaId, aulaId)).ReturnsAsync(false);
            _matriculaRepositoryMock.Setup(r => r.UnitOfWork.Commit()).ReturnsAsync(true);

            await _matriculaAppService.ConcluirAula(matriculaId, aulaId);

            _matriculaRepositoryMock.Verify(r => r.AdicionarAulaConcluida(It.Is<AulaConcluida>(a => a.AulaId == aulaId)), Times.Once);
            _matriculaRepositoryMock.Verify(r => r.UnitOfWork.Commit(), Times.Once);
        }

        [Fact(DisplayName = "Concluir aula já concluída deve publicar notificação")]
        [Trait("Matricula", "AppService")]
        public async Task ConcluirAula_DevePublicarNotificacao_QuandoAulaJaConcluida()
        {
            var matriculaId = Guid.NewGuid();
            var aulaId = Guid.NewGuid();
            var matricula = new Matricula(Guid.NewGuid(), Guid.NewGuid());

            _matriculaRepositoryMock.Setup(r => r.ObterPorIdAsync(matriculaId)).ReturnsAsync(matricula);
            _matriculaRepositoryMock.Setup(r => r.AulaJaConcluida(matriculaId, aulaId)).ReturnsAsync(true);

            await _matriculaAppService.ConcluirAula(matriculaId, aulaId);

            _mediatrHandlerMock.Verify(m => m.PublicarNotificacao(It.Is<DomainNotification>(n =>
                n.Key == "ConcluirAula" && n.Value.Contains("Aula já se encontra concluida"))), Times.Once);

            _matriculaRepositoryMock.Verify(r => r.AdicionarAulaConcluida(It.IsAny<AulaConcluida>()), Times.Never);
        }

        [Fact(DisplayName = "Obter matrícula existente deve retornar viewmodel")]
        [Trait("Matricula", "AppService")]
        public async Task ObterMatricula_DeveRetornarViewModel_QuandoMatriculaExiste()
        {
            var matriculaId = Guid.NewGuid();
            var matricula = new Matricula(Guid.NewGuid(), Guid.NewGuid());
            var matriculaVm = new MatriculaViewModel { Id = matriculaId, AlunoId = matricula.AlunoId, CursoId = matricula.CursoId };

            _matriculaRepositoryMock.Setup(r => r.ObterPorIdAsync(matriculaId)).ReturnsAsync(matricula);
            _mapperMock.Setup(m => m.Map<MatriculaViewModel>(matricula)).Returns(matriculaVm);

            var resultado = await _matriculaAppService.ObterMatricula(matriculaId);

            Assert.NotNull(resultado);
            Assert.Equal(matricula.AlunoId, resultado.AlunoId);
            Assert.Equal(matricula.CursoId, resultado.CursoId);
        }

        [Fact(DisplayName = "Obter matrícula inexistente deve retornar null")]
        [Trait("Matricula", "AppService")]
        public async Task ObterMatricula_DeveRetornarNull_QuandoMatriculaNaoExiste()
        {
            var matriculaId = Guid.NewGuid();

            _matriculaRepositoryMock.Setup(r => r.ObterPorIdAsync(matriculaId)).ReturnsAsync((Matricula)null);
            _mapperMock.Setup(m => m.Map<MatriculaViewModel>(null)).Returns((MatriculaViewModel)null);

            var resultado = await _matriculaAppService.ObterMatricula(matriculaId);

            Assert.Null(resultado);
        }

        [Fact(DisplayName = "Pode emitir certificado deve retornar true se concluído")]
        [Trait("Matricula", "AppService")]
        public async Task PodeEmitirCertificado_DeveRetornarTrue_SeConcluido()
        {
            var matriculaId = Guid.NewGuid();
            var cursoId = Guid.NewGuid();
            var matricula = new Mock<Matricula>(Guid.NewGuid(), cursoId) { CallBase = true };

            _matriculaRepositoryMock.Setup(r => r.ObterPorIdAsync(matriculaId)).ReturnsAsync(matricula.Object);
            _cursoConsultaServiceMock.Setup(c => c.ObterTotalAulasPorCurso(cursoId)).ReturnsAsync(Result<int>.Ok(10));
            matricula.Setup(m => m.PodeEmitirCertificado(10)).Returns(true);

            var podeEmitir = await _matriculaAppService.PodeEmitirCertificado(matriculaId);

            Assert.True(podeEmitir);
        }

        [Fact(DisplayName = "Gerar certificado PDF deve retornar null se não concluído")]
        [Trait("Matricula", "AppService")]
        public async Task GerarCertificadoPDF_DeveRetornarNull_SeNaoConcluido()
        {
            var matriculaId = Guid.NewGuid();
            var cursoId = Guid.NewGuid();
            var alunoId = Guid.NewGuid();
            var matricula = new Mock<Matricula>(alunoId, cursoId) { CallBase = true };

            _matriculaRepositoryMock.Setup(r => r.ObterPorIdAsync(matriculaId)).ReturnsAsync(matricula.Object);
            _cursoConsultaServiceMock.Setup(c => c.ObterTotalAulasPorCurso(cursoId)).ReturnsAsync(Result<int>.Ok(10));
            matricula.Setup(m => m.PodeEmitirCertificado(10)).Returns(false);

            var resultado = await _matriculaAppService.GerarCertificadoPDF(matriculaId);

            Assert.Null(resultado);
            _mediatrHandlerMock.Verify(m => m.PublicarNotificacao(It.Is<DomainNotification>(n =>
                n.Key == "GerarCertificadoPDF" && n.Value.Contains("Aluno ainda não concluiu"))), Times.Once);
        }
    }

}
