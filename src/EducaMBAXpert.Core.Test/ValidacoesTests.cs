using EducaMBAXpert.Core.DomainObjects;
using Xunit;

namespace EducaMBAXpert.Core.Test
{
    public class ValidacoesTests
    {
        [Fact(DisplayName = "Validar se objetos são iguais deve lançar exceção")]
        public void ValidarSeIgual_ObjetosIguais_DeveLancarExcecao()
        {
            var ex = Assert.Throws<DomainException>(() => Validacoes.ValidarSeIgual(10, 10, "Valores iguais"));

            Assert.Equal("Valores iguais", ex.Message);
        }

        [Fact(DisplayName = "Validar se objetos diferentes não deve lançar exceção")]
        public void ValidarSeIgual_ObjetosDiferentes_NaoDeveLancar()
        {
            Validacoes.ValidarSeIgual(10, 20, "Valores iguais");
        }

        [Fact(DisplayName = "Validar se diferentes deve lançar quando objetos não forem iguais")]
        public void ValidarSeDiferente_ObjetosDiferentes_DeveLancarExcecao()
        {
            var ex = Assert.Throws<DomainException>(() => Validacoes.ValidarSeDiferente(10, 20, "Valores diferentes"));

            Assert.Equal("Valores diferentes", ex.Message);
        }

        [Fact(DisplayName = "Validar regex com valor inválido deve lançar exceção")]
        public void ValidarSeDiferente_RegexInvalido_DeveLancarExcecao()
        {
            var pattern = @"^\d{3}$"; // 3 dígitos
            var ex = Assert.Throws<DomainException>(() =>  Validacoes.ValidarSeDiferente(pattern, "12", "Formato inválido"));

            Assert.Equal("Formato inválido", ex.Message);
        }

        [Fact(DisplayName = "Validar tamanho com valor maior que o permitido deve lançar exceção")]
        public void ValidarTamanho_ValorMaior_DeveLancarExcecao()
        {
            var ex = Assert.Throws<DomainException>(() =>  Validacoes.ValidarTamanho("123456", 5, "Tamanho inválido"));

            Assert.Equal("Tamanho inválido", ex.Message);
        }
    }
}
