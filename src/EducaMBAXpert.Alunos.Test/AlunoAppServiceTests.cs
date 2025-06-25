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

            _alunoAppService = new AlunoAppService(
                _alunoRepositoryMock.Object,
                _alunoServiceMock.Object,
                _mapperMock.Object);
        }

        #region Obter

        [Fact(DisplayName = "Obter aluno por ID com sucesso")]
        [Trait("Aluno", "AppService")]
        public async Task ObterPorId_DeveRetornarAlunoViewModel()
        {
            // Arrange
            var alunoId = Guid.NewGuid();
            var aluno = new Aluno(alunoId, "João", "joao@email.com", true);
            var alunoVm = new AlunoViewModel { Id = alunoId, Nome = "João", Email = "joao@gmail.com" };

            _alunoRepositoryMock.Setup(r => r.ObterPorId(alunoId)).ReturnsAsync(aluno);
            _mapperMock.Setup(m => m.Map<AlunoViewModel>(aluno)).Returns(alunoVm);

            // Act
            var resultado = await _alunoAppService.ObterPorId(alunoId);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal("João", resultado.Nome);
        }

        [Fact(DisplayName = "Obter aluno por ID inexistente")]
        [Trait("Aluno", "AppService")]
        public async Task ObterPorId_DeveRetornarNull_SeNaoEncontrado()
        {
            // Arrange
            var alunoId = Guid.NewGuid();

            _alunoRepositoryMock.Setup(r => r.ObterPorId(alunoId)).ReturnsAsync((Aluno)null);
            _mapperMock.Setup(m => m.Map<AlunoViewModel>(null)).Returns((AlunoViewModel)null);

            // Act
            var resultado = await _alunoAppService.ObterPorId(alunoId);

            // Assert
            Assert.Null(resultado);
        }

        [Fact(DisplayName = "Obter todos os alunos")]
        [Trait("Aluno", "AppService")]
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

        #endregion

        #region Manipulação Aluno

        [Fact(DisplayName = "Adicionar novo aluno")]
        [Trait("Aluno", "AppService")]
        public async Task Adicionar_DeveChamarAdicionarNoRepositorio()
        {
            // Arrange
            var inputModel = new AlunoInputModel(Guid.NewGuid(), "Maria", "maria@gmail.com", true);
            var aluno = new Aluno(Guid.NewGuid(), "Maria", "maria@email.com", true);

            _mapperMock.Setup(m => m.Map<Aluno>(inputModel)).Returns(aluno);
            _alunoRepositoryMock.Setup(r => r.UnitOfWork.Commit()).ReturnsAsync(true);

            // Act
            await _alunoAppService.Adicionar(inputModel);

            // Assert
            _alunoRepositoryMock.Verify(r => r.Adicionar(aluno), Times.Once);
            _alunoRepositoryMock.Verify(r => r.UnitOfWork.Commit(), Times.Once);
        }

        [Fact(DisplayName = "Atualizar aluno existente")]
        [Trait("Aluno", "AppService")]
        public async Task Atualizar_DeveChamarAtualizarNoRepositorio()
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

        #endregion

        #region Endereço

        [Fact(DisplayName = "Adicionar endereço ao aluno")]
        [Trait("Aluno", "AppService")]
        public async Task AdicionarEndereco_DeveChamarAdicionarEnderecoECommit()
        {
            // Arrange
            var enderecoInput = new EnderecoInputModel
            {
                Rua = "Rua Teste",
                Numero = "555",
                Cidade = "Ribeirao Preto",
                Complemento = "sala 2207",
                Bairro = "Centro",
                Estado = "SP",
                Cep = "14096730"
            };

            var endereco = new Endereco("Rua Teste", "555", "sala 2207", "Centro", "Ribeirao Preto", "SP", "14096730", Guid.NewGuid());

            _mapperMock.Setup(m => m.Map<Endereco>(enderecoInput)).Returns(endereco);
            _alunoRepositoryMock.Setup(r => r.UnitOfWork.Commit()).ReturnsAsync(true);

            // Act
            await _alunoAppService.AdicionarEndereco(enderecoInput);

            // Assert
            _alunoRepositoryMock.Verify(r => r.AdicionarEndereco(endereco), Times.Once);
            _alunoRepositoryMock.Verify(r => r.UnitOfWork.Commit(), Times.Once);
        }

        #endregion

        #region Matrículas

        [Fact(DisplayName = "Adicionar matrícula para curso")]
        [Trait("Aluno", "AppService")]
        public async Task AdicionarMatriculaCurso_DeveChamarAdicionarMatriculaECommit()
        {
            // Arrange
            var matriculaInput = new MatriculaInputModel
            {
                CursoId = Guid.NewGuid(),
                AlunoId = Guid.NewGuid()
            };

            var matricula = new Matricula(matriculaInput.AlunoId, matriculaInput.CursoId);

            _mapperMock.Setup(m => m.Map<Matricula>(matriculaInput)).Returns(matricula);
            _alunoRepositoryMock.Setup(r => r.UnitOfWork.Commit()).ReturnsAsync(true);

            // Act
            await _alunoAppService.AdicionarMatriculaCurso(matriculaInput);

            // Assert
            _alunoRepositoryMock.Verify(r => r.AdicionarMatricula(matricula), Times.Once);
            _alunoRepositoryMock.Verify(r => r.UnitOfWork.Commit(), Times.Once);
        }

        [Fact(DisplayName = "Obter matrícula por ID do aluno")]
        [Trait("Aluno", "AppService")]
        public async Task ObterMatriculaPorAlunoId_DeveRetornarMatriculaViewModel()
        {
            // Arrange
            var alunoId = Guid.NewGuid();
            var matricula = new Matricula(alunoId, Guid.NewGuid());
            var matriculaVm = new MatriculaViewModel { AlunoId = alunoId };

            _alunoRepositoryMock.Setup(r => r.ObterMatriculaPorId(alunoId)).ReturnsAsync(matricula);
            _mapperMock.Setup(m => m.Map<MatriculaViewModel>(matricula)).Returns(matriculaVm);

            // Act
            var resultado = await _alunoAppService.ObterMatriculaPorId(alunoId);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(alunoId, resultado.AlunoId);
        }

        [Fact(DisplayName = "Obter todas as matrículas por aluno")]
        [Trait("Aluno", "AppService")]
        public async Task ObterTodasMatriculasPorAlunoId_DeveRetornarListaDeMatriculaViewModel()
        {
            // Arrange
            var alunoId = Guid.NewGuid();
            var ativas = true;

            var matriculas = new List<Matricula> { new Matricula(alunoId, Guid.NewGuid()) };
            var matriculasVm = new List<MatriculaViewModel> { new MatriculaViewModel { AlunoId = alunoId } };

            _alunoRepositoryMock.Setup(r => r.ObterTodasMatriculasPorAlunoId(alunoId, ativas)).ReturnsAsync(matriculas);
            _mapperMock.Setup(m => m.Map<IEnumerable<MatriculaViewModel>>(matriculas)).Returns(matriculasVm);

            // Act
            var resultado = await _alunoAppService.ObterTodasMatriculasPorAlunoId(alunoId, ativas);

            // Assert
            Assert.NotNull(resultado);
            Assert.Single(resultado);
            Assert.All(resultado, m => Assert.Equal(alunoId, m.AlunoId));
        }

        #endregion

        #region Ativação / Inativação

        [Fact(DisplayName = "Ativar aluno com sucesso")]
        [Trait("Aluno", "AppService")]
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

        [Fact(DisplayName = "Ativar aluno com falha")]
        [Trait("Aluno", "AppService")]
        public async Task Ativar_DeveLancarExcecao_QuandoFalha()
        {
            // Arrange
            var alunoId = Guid.NewGuid();
            _alunoServiceMock.Setup(s => s.Ativar(alunoId)).ReturnsAsync(false);

            // Act & Assert
            var excecao = await Assert.ThrowsAsync<DomainException>(() =>
                _alunoAppService.Ativar(alunoId));

            Assert.Equal("Falha ao Inativar Aluno", excecao.Message);
        }

        [Fact(DisplayName = "Inativar aluno com sucesso")]
        [Trait("Aluno", "AppService")]
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

        [Fact(DisplayName = "Inativar aluno com falha")]
        [Trait("Aluno", "AppService")]
        public async Task Inativar_DeveLancarExcecao_QuandoFalha()
        {
            // Arrange
            var alunoId = Guid.NewGuid();
            _alunoServiceMock.Setup(s => s.Inativar(alunoId)).ReturnsAsync(false);

            // Act & Assert
            var excecao = await Assert.ThrowsAsync<DomainException>(() =>
                _alunoAppService.Inativar(alunoId));

            Assert.Equal("Falha ao Inativar Aluno", excecao.Message);
        }

        #endregion
    }
}
