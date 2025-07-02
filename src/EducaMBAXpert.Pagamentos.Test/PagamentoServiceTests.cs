using EducaMBAXpert.Core.Bus;
using EducaMBAXpert.Core.DomainObjects.DTO;
using EducaMBAXpert.Pagamentos.Business.Entities;
using EducaMBAXpert.Pagamentos.Business.Enum;
using EducaMBAXpert.Pagamentos.Business.Interfaces;
using EducaMBAXpert.Pagamentos.Business.Models;
using EducaMBAXpert.Pagamentos.Business.Services;
using Moq;
using Xunit;

namespace EducaMBAXpert.Pagamentos.Test
{
    public class PagamentoServiceTests
    {
        private readonly Mock<IPagamentoCartaoCreditoFacade> _facadeMock;
        private readonly Mock<IPagamentoRepository> _repositoryMock;
        private readonly Mock<IMediatrHandler> _mediatorMock;
        private readonly PagamentoService _service;

        public PagamentoServiceTests()
        {
            _facadeMock = new Mock<IPagamentoCartaoCreditoFacade>();
            _repositoryMock = new Mock<IPagamentoRepository>();
            _mediatorMock = new Mock<IMediatrHandler>();

            _service = new PagamentoService( _facadeMock.Object,_repositoryMock.Object, _mediatorMock.Object);
        }

        [Fact(DisplayName = "Realizar pagamento com sucesso")]
        [Trait("Pagamento", "Service")]
        public async Task RealizarPagamentoPedido_Sucesso_DeveSalvarECommitar()
        {
            // Arrange
            var input = new PagamentoCurso
            {
                CursoId = Guid.NewGuid(),
                ClienteId = Guid.NewGuid(),
                Total = 199.90m,
                NomeCartao = "Teste",
                NumeroCartao = "4111111111111111",
                ExpiracaoCartao = "12/26",
                CvvCartao = "123"
            };

            var transacao = new Transacao
            {
                Id = Guid.NewGuid(),
                PagamentoId = Guid.NewGuid(),
                StatusTransacao = StatusTransacao.Pago
            };

            _facadeMock.Setup(f => f.RealizarPagamento(It.IsAny<CobrancaCurso>(), It.IsAny<Pagamento>()))
                       .Returns(transacao);

            _repositoryMock.Setup(r => r.UnitOfWork.Commit())
                           .ReturnsAsync(true);

            // Act
            var resultado = await _service.RealizarPagamentoPedido(input);

            // Assert
            Assert.Equal(StatusTransacao.Pago, resultado.StatusTransacao);
            _repositoryMock.Verify(r => r.Adicionar(It.IsAny<Pagamento>()), Times.Once);
            _repositoryMock.Verify(r => r.AdicionarTransacao(transacao), Times.Once);
            _repositoryMock.Verify(r => r.UnitOfWork.Commit(), Times.Once);
        }

        [Fact(DisplayName = "Realizar pagamento negado - não deve salvar ou commitar")]
        [Trait("Pagamento", "Service")]
        public async Task RealizarPagamentoPedido_Negado_NaoDevePersistir()
        {
            // Arrange
            var input = new PagamentoCurso
            {
                CursoId = Guid.NewGuid(),
                ClienteId = Guid.NewGuid(),
                Total = 199.90m,
                NomeCartao = "Teste",
                NumeroCartao = "4111111111111111",
                ExpiracaoCartao = "12/26",
                CvvCartao = "123"
            };

            var transacao = new Transacao
            {
                Id = Guid.NewGuid(),
                PagamentoId = Guid.NewGuid(),
                StatusTransacao = StatusTransacao.Recusado
            };

            _facadeMock.Setup(f => f.RealizarPagamento(It.IsAny<CobrancaCurso>(), It.IsAny<Pagamento>()))
                       .Returns(transacao);

            // Act
            var resultado = await _service.RealizarPagamentoPedido(input);

            // Assert
            Assert.Equal(StatusTransacao.Recusado, resultado.StatusTransacao);
            _repositoryMock.Verify(r => r.Adicionar(It.IsAny<Pagamento>()), Times.Never);
            _repositoryMock.Verify(r => r.AdicionarTransacao(It.IsAny<Transacao>()), Times.Never);
            _repositoryMock.Verify(r => r.UnitOfWork.Commit(), Times.Never);
        }

        [Fact(DisplayName = "Erro ao realizar commit deve propagar exceção")]
        [Trait("Pagamento", "Service")]
        public async Task RealizarPagamentoPedido_ErroCommit_DeveLancarExcecao()
        {
            // Arrange
            var input = new PagamentoCurso
            {
                CursoId = Guid.NewGuid(),
                ClienteId = Guid.NewGuid(),
                Total = 150m,
                NomeCartao = "Erro Teste",
                NumeroCartao = "4111111111111111",
                ExpiracaoCartao = "01/30",
                CvvCartao = "321"
            };

            var transacao = new Transacao
            {
                Id = Guid.NewGuid(),
                PagamentoId = Guid.NewGuid(),
                StatusTransacao = StatusTransacao.Pago
            };

            _facadeMock.Setup(f => f.RealizarPagamento(It.IsAny<CobrancaCurso>(), It.IsAny<Pagamento>()))
                       .Returns(transacao);

            _repositoryMock.Setup(r => r.UnitOfWork.Commit())
                           .ThrowsAsync(new InvalidOperationException("Erro ao commitar"));

            // Act & Assert
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                     _service.RealizarPagamentoPedido(input));

            Assert.Equal("Erro ao commitar", ex.Message);
            _repositoryMock.Verify(r => r.Adicionar(It.IsAny<Pagamento>()), Times.Once);
            _repositoryMock.Verify(r => r.AdicionarTransacao(transacao), Times.Once);
            _repositoryMock.Verify(r => r.UnitOfWork.Commit(), Times.Once);
        }
    }
}
