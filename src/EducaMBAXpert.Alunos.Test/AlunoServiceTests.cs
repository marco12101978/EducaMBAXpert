using EducaMBAXpert.Alunos.Domain.Entities;
using EducaMBAXpert.Alunos.Domain.Events;
using EducaMBAXpert.Alunos.Domain.Interfaces;
using EducaMBAXpert.Alunos.Domain.Services;
using EducaMBAXpert.Core.Bus;
using Moq;

namespace EducaMBAXpert.Alunos.Test
{
    public class AlunoServiceTests
    {
        private readonly Mock<IAlunoRepository> _alunoRepositoryMock;
        private readonly Mock<IMediatrHandler> _mediatrHandlerMock;
        private readonly AlunoService _alunoService;

        public AlunoServiceTests()
        {
            _alunoRepositoryMock = new Mock<IAlunoRepository>();
            _mediatrHandlerMock = new Mock<IMediatrHandler>();

            _alunoService = new AlunoService(_mediatrHandlerMock.Object, _alunoRepositoryMock.Object);
        }

        #region Ativar

        [Fact(DisplayName = "Ativar aluno com sucesso")]
        [Trait("Aluno", "Service")]
        public async Task Ativar_DeveAtivarAlunoComSucesso()
        {
            // Arrange
            var aluno = new Aluno(Guid.NewGuid(), "Joao", "joao@email.com", false);

            _alunoRepositoryMock.Setup(r => r.ObterPorId(aluno.Id))
                                .ReturnsAsync(aluno);

            _alunoRepositoryMock.Setup(r => r.UnitOfWork.Commit())
                                .ReturnsAsync(true);

            // Act
            var resultado = await _alunoService.Ativar(aluno.Id);

            // Assert
            Assert.True(resultado);
            Assert.True(aluno.Ativo);

            _mediatrHandlerMock.Verify(m =>
                m.PublicarEvento(It.IsAny<AlunoInativarEvent>()), Times.Once);
        }

        [Fact(DisplayName = "Ativar aluno já ativo deve retornar false")]
        [Trait("Aluno", "Service")]
        public async Task Ativar_DeveRetornarFalse_SeAlunoJaEstiverAtivo()
        {
            // Arrange
            var aluno = new Aluno(Guid.NewGuid(), "Ana", "ana@email.com", true);

            _alunoRepositoryMock.Setup(r => r.ObterPorId(aluno.Id))
                                .ReturnsAsync(aluno);

            // Act
            var resultado = await _alunoService.Ativar(aluno.Id);

            // Assert
            Assert.False(resultado);

            _mediatrHandlerMock.Verify(m =>
                m.PublicarEvento(It.IsAny<AlunoInativarEvent>()), Times.Never);
        }

        [Fact(DisplayName = "Ativar aluno inexistente deve retornar false")]
        [Trait("Aluno", "Service")]
        public async Task Ativar_DeveRetornarFalse_SeAlunoNaoExistir()
        {
            // Arrange
            var id = Guid.NewGuid();

            _alunoRepositoryMock.Setup(r => r.ObterPorId(id))
                                .ReturnsAsync((Aluno)null);

            // Act
            var resultado = await _alunoService.Ativar(id);

            // Assert
            Assert.False(resultado);
        }

        #endregion

        #region Inativar

        [Fact(DisplayName = "Inativar aluno com sucesso")]
        [Trait("Aluno", "Service")]
        public async Task Inativar_DeveInativarAlunoComSucesso()
        {
            // Arrange
            var aluno = new Aluno(Guid.NewGuid(), "Lucas", "lucas@email.com", true);

            _alunoRepositoryMock.Setup(r => r.ObterPorId(aluno.Id))
                                .ReturnsAsync(aluno);

            _alunoRepositoryMock.Setup(r => r.UnitOfWork.Commit())
                                .ReturnsAsync(true);

            // Act
            var resultado = await _alunoService.Inativar(aluno.Id);

            // Assert
            Assert.True(resultado);
            Assert.False(aluno.Ativo);
        }

        [Fact(DisplayName = "Inativar aluno já inativo deve retornar false")]
        [Trait("Aluno", "Service")]
        public async Task Inativar_DeveRetornarFalse_SeAlunoJaEstiverInativo()
        {
            // Arrange
            var aluno = new Aluno(Guid.NewGuid(), "Julia", "julia@email.com", false);

            _alunoRepositoryMock.Setup(r => r.ObterPorId(aluno.Id))
                                .ReturnsAsync(aluno);

            // Act
            var resultado = await _alunoService.Inativar(aluno.Id);

            // Assert
            Assert.False(resultado);
        }

        #endregion
    }
}
