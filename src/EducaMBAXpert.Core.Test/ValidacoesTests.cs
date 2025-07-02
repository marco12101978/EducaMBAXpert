using EducaMBAXpert.Core.DomainObjects;
using Xunit;

namespace EducaMBAXpert.Core.Test
{
    public class ValidacoesTests
    {
        [Fact(DisplayName = "Validar se objetos são iguais deve lançar exceção")]
        [Trait("Validacoes", "Core")]
        public void ValidarSeIgual_ObjetosIguais_DeveLancarExcecao()
        {
            // Arrange
            var a = 10;
            var b = 10;
            var mensagem = "Valores iguais";

            // Act
            var ex = Assert.Throws<DomainException>(() =>
                Validacoes.ValidarSeIgual(a, b, mensagem));

            // Assert
            Assert.Equal(mensagem, ex.Message);
        }

        [Fact(DisplayName = "Validar se objetos diferentes não deve lançar exceção")]
        [Trait("Validacoes", "Core")]
        public void ValidarSeIgual_ObjetosDiferentes_NaoDeveLancar()
        {
            // Arrange
            var a = 10;
            var b = 20;
            var mensagem = "Valores iguais";

            // Act & Assert
            var exception = Record.Exception(() =>
                Validacoes.ValidarSeIgual(a, b, mensagem));

            Assert.Null(exception);
        }

        [Fact(DisplayName = "Validar se diferentes deve lançar quando objetos não forem iguais")]
        [Trait("Validacoes", "Core")]
        public void ValidarSeDiferente_ObjetosDiferentes_DeveLancarExcecao()
        {
            // Arrange
            var a = 10;
            var b = 20;
            var mensagem = "Valores diferentes";

            // Act
            var ex = Assert.Throws<DomainException>(() =>
                Validacoes.ValidarSeDiferente(a, b, mensagem));

            // Assert
            Assert.Equal(mensagem, ex.Message);
        }

        [Fact(DisplayName = "Validar regex com valor inválido deve lançar exceção")]
        [Trait("Validacoes", "Core")]
        public void ValidarSeDiferente_RegexInvalido_DeveLancarExcecao()
        {
            // Arrange
            var _regex = @"^\d{3}$"; // 3 dígitos
            var input = "12";
            var mensagem = "Formato inválido";

            // Act
            var ex = Assert.Throws<DomainException>(() =>
                Validacoes.ValidarSeDiferente(_regex, input, mensagem));

            // Assert
            Assert.Equal(mensagem, ex.Message);
        }

        [Fact(DisplayName = "Validar tamanho com valor maior que o permitido deve lançar exceção")]
        [Trait("Validacoes", "Core")]
        public void ValidarTamanho_ValorMaior_DeveLancarExcecao()
        {
            // Arrange
            var valor = "123456";
            var tamanhoMaximo = 5;
            var mensagem = "Tamanho inválido";

            // Act
            var ex = Assert.Throws<DomainException>(() =>
                Validacoes.ValidarTamanho(valor, tamanhoMaximo, mensagem));

            // Assert
            Assert.Equal(mensagem, ex.Message);
        }
    }
}
