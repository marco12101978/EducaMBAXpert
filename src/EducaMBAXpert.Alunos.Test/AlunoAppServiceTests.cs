using AutoMapper;
using EducaMBAXpert.Alunos.Application.Services;
using EducaMBAXpert.Alunos.Application.ViewModels;
using EducaMBAXpert.Alunos.Domain.Entities;
using EducaMBAXpert.Alunos.Domain.Interfaces;
using EducaMBAXpert.Core.DomainObjects;
using Moq;

namespace EducaMBAXpert.Alunos.Test
{
    public class AlunoAppServiceTests
    {
        private readonly Mock<IAlunoRepository> _alunoRepositoryMock;
        private readonly Mock<IAlunoService> _alunoServiceMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly AlunoAppService _alunoAppService;

        public AlunoAppServiceTests()
        {
            _alunoRepositoryMock = new Mock<IAlunoRepository>();
            _alunoServiceMock = new Mock<IAlunoService>();
            _mapperMock = new Mock<IMapper>();

            _alunoAppService = new AlunoAppService(_alunoRepositoryMock.Object, _alunoServiceMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task ObterPorId_DeveRetornarAlunoViewModel()
        {
            // Arrange
            var alunoId = Guid.NewGuid();
            var aluno = new Aluno(Guid.NewGuid(), "João", "joao@email.com",true);
            var alunoVm = new AlunoViewModel { Id = alunoId, Nome = "João", Email = "joao@gmail.com" };

            _alunoRepositoryMock.Setup(r => r.ObterPorId(alunoId)).ReturnsAsync(aluno);
            _mapperMock.Setup(m => m.Map<AlunoViewModel>(aluno)).Returns(alunoVm);

            // Act
            var resultado = await _alunoAppService.ObterPorId(alunoId);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal("João", resultado.Nome);
        }

        [Fact]
        public async Task ObterTodos_DeveRetornarListaDeAlunoViewModel()
        {
            // Arrange
            var alunos = new List<Aluno> { new Aluno(Guid.NewGuid(), "Carlos", "carlos@email.com", true) };
            var alunosVm = new List<AlunoViewModel> { new AlunoViewModel { Nome = "Carlos", Email = "carlos@gmail.com" } };

            _alunoRepositoryMock.Setup(r => r.ObterTodos()).ReturnsAsync(alunos);
            _mapperMock.Setup(m => m.Map<IEnumerable<AlunoViewModel>>(alunos)).Returns(alunosVm);

            // Act
            var resultado = await _alunoAppService.ObterTodos();

            // Assert
            Assert.NotEmpty(resultado);
            Assert.Contains(resultado, a => a.Nome == "Carlos");
        }

        [Fact]
        public async Task Adicionar_DeveChamarAdicionarNoRepositorio()
        {
            // Arrange
            var inputModel = new AlunoInputModel(Guid.NewGuid(),"Maria","maria@gmail.com" , true);
            var aluno = new Aluno(Guid.NewGuid(), "Maria", "maria@email.com", true);

            _mapperMock.Setup(m => m.Map<Aluno>(inputModel)).Returns(aluno);
            _alunoRepositoryMock.Setup(r => r.UnitOfWork.Commit()).ReturnsAsync(true);

            // Act
            await _alunoAppService.Adicionar(inputModel);

            // Assert
            _alunoRepositoryMock.Verify(r => r.Adicionar(aluno), Times.Once);
            _alunoRepositoryMock.Verify(r => r.UnitOfWork.Commit(), Times.Once);
        }

        [Fact]
        public async Task Adicionar_DeveChamarAtualizarNoRepositorio()
        {
            // Arrange
            var inputModel = new AlunoInputModel(Guid.NewGuid(), "Maria", "maria@gmail.com", true);
            var aluno = new Aluno(Guid.NewGuid(), "Maria", "maria@email.com", true);

            _mapperMock.Setup(m => m.Map<Aluno>(inputModel)).Returns(aluno);
            _alunoRepositoryMock.Setup(r => r.UnitOfWork.Commit()).ReturnsAsync(true);

            // Act
            await _alunoAppService.Atualizar(inputModel);

            // Assert
            _alunoRepositoryMock.Verify(r => r.Atualizar(aluno), Times.Once);
            _alunoRepositoryMock.Verify(r => r.UnitOfWork.Commit(), Times.Once);
        }

        [Fact]
        public async Task Ativar_DeveRetornarTrue_QuandoSucesso()
        {
            // Arrange
            var alunoId = Guid.NewGuid();
            _alunoServiceMock.Setup(s => s.Ativar(alunoId)).ReturnsAsync(true);

            // Act
            var resultado = await _alunoAppService.Ativar(alunoId);

            // Assert
            Assert.True(resultado);
        }

        [Fact]
        public async Task Ativar_DeveLancarExcecao_QuandoFalha()
        {
            // Arrange
            var alunoId = Guid.NewGuid();
            _alunoServiceMock.Setup(s => s.Ativar(alunoId)).ReturnsAsync(false);

            // Act & Assert
            var excecao = await Assert.ThrowsAsync<DomainException>(() => _alunoAppService.Ativar(alunoId));
            Assert.Equal("Falha ao Inativar Aluno", excecao.Message);
        }

        [Fact]
        public async Task Inativar_DeveRetornarTrue_QuandoSucesso()
        {
            // Arrange
            var alunoId = Guid.NewGuid();
            _alunoServiceMock.Setup(s => s.Inativar(alunoId)).ReturnsAsync(true);

            // Act
            var resultado = await _alunoAppService.Inativar(alunoId);

            // Assert
            Assert.True(resultado);
        }

        [Fact]
        public async Task Inativar_DeveLancarExcecao_QuandoFalha()
        {
            // Arrange
            var alunoId = Guid.NewGuid();
            _alunoServiceMock.Setup(s => s.Inativar(alunoId)).ReturnsAsync(false);

            // Act & Assert
            var excecao = await Assert.ThrowsAsync<DomainException>(() => _alunoAppService.Inativar(alunoId));
            Assert.Equal("Falha ao Inativar Aluno", excecao.Message);
        }

        [Fact]
        public async Task ObterPorId_DeveRetornarNull_SeNaoEncontrado()
        {
            // Arrange
            var alunoId = Guid.NewGuid();
            _alunoRepositoryMock?.Setup(r => r.ObterPorId(alunoId)).ReturnsAsync((Aluno)null);
            _mapperMock.Setup(m => m.Map<AlunoViewModel>(null)).Returns((AlunoViewModel)null);

            // Act
            var resultado = await _alunoAppService.ObterPorId(alunoId);

            // Assert
            Assert.Null(resultado);
        }


    }
}
