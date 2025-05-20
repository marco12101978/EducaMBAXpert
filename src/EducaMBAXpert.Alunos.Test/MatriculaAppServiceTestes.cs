using AutoMapper;
using EducaMBAXpert.Alunos.Application.Services;
using EducaMBAXpert.Alunos.Domain.Entities;
using EducaMBAXpert.Alunos.Domain.Interfaces;
using EducaMBAXpert.Core.Bus;
using EducaMBAXpert.Core.Data;
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
    }
}
