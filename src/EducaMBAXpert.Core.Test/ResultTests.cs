using EducaMBAXpert.Core.DomainObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace EducaMBAXpert.Core.Test
{
    public class ResultTests
    {
        [Fact(DisplayName = "Result.Ok deve retornar sucesso sem mensagem")]
        public void Result_Ok_DeveSerSucesso()
        {
            var result = Result.Ok();

            Assert.True(result.Success);
            Assert.Null(result.Message);
        }

        [Fact(DisplayName = "Result.Fail deve retornar falha com mensagem")]
        public void Result_Fail_DeveSerFalha()
        {
            var result = Result.Fail("Erro ao salvar");

            Assert.False(result.Success);
            Assert.Equal("Erro ao salvar", result.Message);
        }

        [Fact(DisplayName = "Result<T>.Ok deve retornar sucesso com dados")]
        public void ResultT_Ok_DeveRetornarComDados()
        {
            var result = Result<string>.Ok("dados");

            Assert.True(result.Success);
            Assert.Equal("dados", result.Data);
            Assert.Null(result.Message);
        }

        [Fact(DisplayName = "Result<T>.Fail deve retornar falha com mensagem e sem dados")]
        public void ResultT_Fail_DeveRetornarComErro()
        {
            var result = Result<string>.Fail("falhou");

            Assert.False(result.Success);
            Assert.Equal("falhou", result.Message);
            Assert.Null(result.Data);
        }
    }
}
