using EducaMBAXpert.Core.DomainObjects;
using Xunit;

namespace EducaMBAXpert.Core.Test
{
    public class ResultTests
    {
        [Fact(DisplayName = "Result.Ok deve retornar sucesso sem mensagem")]
        [Trait("Result", "Core")]
        public void Result_Ok_DeveSerSucesso()
        {
            // Arrange

            // Act
            var result = Result.Ok();

            // Assert
            Assert.True(result.Success);
            Assert.Null(result.Message);
        }

        [Fact(DisplayName = "Result.Fail deve retornar falha com mensagem")]
        [Trait("Result", "Core")]
        public void Result_Fail_DeveSerFalha()
        {
            // Arrange
            var mensagem = "Erro ao salvar";

            // Act
            var result = Result.Fail(mensagem);

            // Assert
            Assert.False(result.Success);
            Assert.Equal(mensagem, result.Message);
        }

        [Fact(DisplayName = "Result<T>.Ok deve retornar sucesso com dados")]
        [Trait("Result", "Core")]
        public void ResultT_Ok_DeveRetornarComDados()
        {
            // Arrange
            var dados = "dados";

            // Act
            var result = Result<string>.Ok(dados);

            // Assert
            Assert.True(result.Success);
            Assert.Equal(dados, result.Data);
            Assert.Null(result.Message);
        }

        [Fact(DisplayName = "Result<T>.Fail deve retornar falha com mensagem e sem dados")]
        [Trait("Result", "Core")]
        public void ResultT_Fail_DeveRetornarComErro()
        {
            // Arrange
            var mensagem = "falhou";

            // Act
            var result = Result<string>.Fail(mensagem);

            // Assert
            Assert.False(result.Success);
            Assert.Equal(mensagem, result.Message);
            Assert.Null(result.Data);
        }
    }
}
