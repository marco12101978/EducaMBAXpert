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

            _matriculaAppService = new MatriculaAppService(_matriculaRepositoryMock.Object,
                                                           _cursoConsultaServiceMock.Object,
                                                           _alunoRepositoryMock.Object,
                                                           _mediatrHandlerMock.Object,
                                                            _mapperMock.Object
            );
        }

        [Fact]
        public async Task ConcluirAula_DeveAdicionarAulaConcluida_QuandoNaoConcluida()
        {
            // Arrange
            var matriculaId = Guid.NewGuid();
            var aulaId = Guid.NewGuid();
            var matricula = new Matricula(Guid.NewGuid(), Guid.NewGuid());

            _matriculaRepositoryMock.Setup(r => r.ObterPorIdAsync(matriculaId)).ReturnsAsync(matricula);
            _matriculaRepositoryMock.Setup(r => r.AulaJaConcluida(matriculaId, aulaId)).ReturnsAsync(false);
            _matriculaRepositoryMock.Setup(r => r.UnitOfWork.Commit()).ReturnsAsync(true);

            // Act
            await _matriculaAppService.ConcluirAula(matriculaId, aulaId);

            // Assert
            _matriculaRepositoryMock.Verify(r => r.AdicionarAulaConcluida(It.Is<AulaConcluida>(a => a.AulaId == aulaId)), Times.Once);
            _matriculaRepositoryMock.Verify(r => r.UnitOfWork.Commit(), Times.Once);
        }

        [Fact]
        public async Task ConcluirAula_DevePublicarNotificacao_QuandoAulaJaConcluida()
        {
            // Arrange
            var matriculaId = Guid.NewGuid();
            var aulaId = Guid.NewGuid();
            var matricula = new Matricula(Guid.NewGuid(), Guid.NewGuid());

            _matriculaRepositoryMock.Setup(r => r.ObterPorIdAsync(matriculaId)).ReturnsAsync(matricula);
            _matriculaRepositoryMock.Setup(r => r.AulaJaConcluida(matriculaId, aulaId)).ReturnsAsync(true);

            // Act
            await _matriculaAppService.ConcluirAula(matriculaId, aulaId);

            // Assert
            _mediatrHandlerMock.Verify(m => m.PublicarNotificacao(It.Is<DomainNotification>(n =>
                n.Key == "ConcluirAula" && n.Value.Contains("Aula já se encontra concluida"))), Times.Once);

            _matriculaRepositoryMock.Verify(r => r.AdicionarAulaConcluida(It.IsAny<AulaConcluida>()), Times.Never);
        }

        [Fact]
        public async Task ObterMatricula_DeveRetornarViewModel_QuandoMatriculaExiste()
        {
            // Arrange
            var matriculaId = Guid.NewGuid();
            var matricula = new Matricula(Guid.NewGuid(), Guid.NewGuid());

            var matriculaVm = new MatriculaViewModel
            {
                Id = matriculaId,
                AlunoId = matricula.AlunoId,
                CursoId = matricula.CursoId
            };

            _matriculaRepositoryMock.Setup(r => r.ObterPorIdAsync(matriculaId)).ReturnsAsync(matricula);

            _mapperMock.Setup(m => m.Map<MatriculaViewModel>(matricula)).Returns(matriculaVm);

            // Act
            var resultado = await _matriculaAppService.ObterMatricula(matriculaId);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(matricula.AlunoId, resultado.AlunoId);
            Assert.Equal(matricula.CursoId, resultado.CursoId);
        }

        [Fact]
        public async Task ObterMatricula_DeveRetornarNull_QuandoMatriculaNaoExiste()
        {
            // Arrange
            var matriculaId = Guid.NewGuid();

            _matriculaRepositoryMock.Setup(r => r.ObterPorIdAsync(matriculaId)).ReturnsAsync((Matricula)null);

            _mapperMock.Setup(m => m.Map<MatriculaViewModel>(null)).Returns((MatriculaViewModel)null);

            // Act
            var resultado = await _matriculaAppService.ObterMatricula(matriculaId);

            // Assert
            Assert.Null(resultado);
        }


        [Fact]
        public async Task PodeEmitirCertificado_DeveRetornarTrue_SeConcluido()
        {
            // Arrange
            var matriculaId = Guid.NewGuid();
            var cursoId = Guid.NewGuid();
            var matricula = new Mock<Matricula>(Guid.NewGuid(), cursoId) { CallBase = true };

            _matriculaRepositoryMock.Setup(r => r.ObterPorIdAsync(matriculaId)).ReturnsAsync(matricula.Object);
            _cursoConsultaServiceMock.Setup(c => c.ObterTotalAulasPorCurso(cursoId)).ReturnsAsync(Result<int>.Ok(10));
            matricula.Setup(m => m.PodeEmitirCertificado(10)).Returns(true);

            // Act
            var podeEmitir = await _matriculaAppService.PodeEmitirCertificado(matriculaId);

            // Assert
            Assert.True(podeEmitir);
        }


        [Fact]
        public async Task GerarCertificadoPDF_DeveRetornarNull_SeNaoConcluido()
        {
            // Arrange
            var matriculaId = Guid.NewGuid();
            var cursoId = Guid.NewGuid();
            var alunoId = Guid.NewGuid();
            var matricula = new Mock<Matricula>(alunoId, cursoId) { CallBase = true };

            _matriculaRepositoryMock.Setup(r => r.ObterPorIdAsync(matriculaId)).ReturnsAsync(matricula.Object);
            _cursoConsultaServiceMock.Setup(c => c.ObterTotalAulasPorCurso(cursoId)).ReturnsAsync(Result<int>.Ok(10));
            matricula.Setup(m => m.PodeEmitirCertificado(10)).Returns(false);

            // Act
            var resultado = await _matriculaAppService.GerarCertificadoPDF(matriculaId);

            // Assert
            Assert.Null(resultado);
            _mediatrHandlerMock.Verify(m => m.PublicarNotificacao(It.Is<DomainNotification>(n => n.Key == "GerarCertificadoPDF"
                                       && n.Value.Contains("Aluno ainda não concluiu"))), Times.Once);
        }
    }
}
