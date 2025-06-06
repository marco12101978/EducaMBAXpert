using EducaMBAXpert.Api.Tests.Integration.Config;
using EducaMBAXpert.CatalagoCursos.Domain.Entities;
using EducaMBAXpert.Pagamentos.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using System.Net.Http;
using EducaMBAXpert.Pagamentos.Business.Entities;

namespace EducaMBAXpert.Api.Tests.Integration
{
    [TestCaseOrderer("EducaMBAXpert.Api.Tests.Integration.Config.PriorityOrderer", "EducaMBAXpert.Api.Tests.Integration")]
    [Collection(nameof(IntegrationWebTestsFixtureCollection))]
    public class PagamentoTestes
    {
        private readonly IntegrationTestsFixture<Program> _testsFixture;

        public PagamentoTestes(IntegrationTestsFixture<Program> testsFixture)
        {
            _testsFixture = testsFixture;
        }

        #region Pagamento - Casos de sucesso e falha

        [Fact(DisplayName = "Executar pagamento novo curso com tentativas deve retornar NoContent"), TestPriority(1)]
        [Trait("Pagamento", "Integração API - Pagamento")]
        public async Task ExecutarPagamento_ComTentativas_DeveRetornarNoContent()
        {
            await AutenticarComoAdminAsync();

            var idAluno = _testsFixture.IdAluno;
            var idMatricula = (await _testsFixture.ObterPrimeiraMatriculaAsync(idAluno)).Id;
            var inputModel = CriarPagamentoInputModel(idAluno, idMatricula);

            HttpResponseMessage response = null;
            int tentativas;

            for (tentativas = 1; tentativas <= 10; tentativas++)
            {
                response = await _testsFixture.Client.PostAsJsonAsync("/api/v1/pagamentos/pagamento", inputModel);
                if (response.StatusCode == HttpStatusCode.NoContent)
                    break;

                await Task.Delay(1000);
            }

            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact(DisplayName = "Pagamento com AlunoId inválido deve retornar NotFound"), TestPriority(2)]
        [Trait("Pagamento", "Integração API - Pagamento")]
        public async Task ExecutarPagamento_AlunoIdInvalido_DeveRetornarNotFound()
        {
            await AutenticarComoAdminAsync();

            var idAlunoInvalido = Guid.NewGuid();
            var idMatricula = (await _testsFixture.ObterPrimeiraMatriculaAsync(_testsFixture.IdAluno)).Id;
            var inputModel = CriarPagamentoInputModel(idAlunoInvalido, idMatricula);

            var response = await _testsFixture.Client.PostAsJsonAsync("/api/v1/pagamentos/pagamento", inputModel);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact(DisplayName = "Pagamento com MatriculaId inválido deve retornar NotFound"), TestPriority(3)]
        [Trait("Pagamento", "Integração API - Pagamento")]
        public async Task ExecutarPagamento_MatriculaIdInvalido_DeveRetornarNotFound()
        {
            await AutenticarComoAdminAsync();

            var idAluno = _testsFixture.IdAluno;
            var idMatriculaInvalido = Guid.NewGuid();
            var inputModel = CriarPagamentoInputModel(idAluno, idMatriculaInvalido);

            var response = await _testsFixture.Client.PostAsJsonAsync("/api/v1/pagamentos/pagamento", inputModel);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Theory(DisplayName = "Pagamento com dados de cartão inválidos deve retornar BadRequest"), TestPriority(4)]
        [Trait("Pagamento", "Integração API - Pagamento")]
        [InlineData("", "11/24", "123", "Número do cartão vazio")]
        [InlineData("123456", "11/24", "123", "Número do cartão inválido")]
        [InlineData("1111222233334444", "00/00", "123", "Expiração inválida")]
        [InlineData("1111222233334444", "11/24", "", "CVV vazio")]
        [InlineData("1111222233334444", "11/24", "abc", "CVV com letras")]
        public async Task ExecutarPagamento_DadosCartaoInvalidos_DeveRetornarBadRequest(string numeroCartao, string expiracao, string cvv, string descricao)
        {
            await AutenticarComoAdminAsync();

            var idAluno = _testsFixture.IdAluno;
            var idMatricula = (await _testsFixture.ObterPrimeiraMatriculaAsync(idAluno)).Id;
            var inputModel = CriarPagamentoInputModel(idAluno, idMatricula, numeroCartao, expiracao, cvv);

            var response = await _testsFixture.Client.PostAsJsonAsync("/api/v1/pagamentos/pagamento", inputModel);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        #endregion

        #region Pagamento - Consultas com Admin

        [Fact(DisplayName = "Obter todos os pagamentos deve retornar OK"), TestPriority(5)]
        [Trait("Pagamento", "Integração API - Pagamento")]
        public async Task ObterTodosPagamentos_DeveRetornarOK()
        {
            await AutenticarComoAdminAsync();

            var response = await _testsFixture.Client.GetAsync("/api/v1/pagamentos/obter_todos");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact(DisplayName = "Obter pagamento por ID deve retornar OK"), TestPriority(6)]
        [Trait("Pagamento", "Integração API - Pagamento")]
        public async Task ObterPagamentoPorId_DeveRetornarOK()
        {
            await AutenticarComoAdminAsync();

            var idPagamento = (await _testsFixture.ObterPrimeiraPagamentoAsync()).Id;
            var response = await _testsFixture.Client.GetAsync($"/api/v1/pagamentos/obter/{idPagamento}");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        #endregion

        #region Autorização - sem token

        [Fact(DisplayName = "Obter todos os pagamentos sem autenticação deve retornar Unauthorized"), TestPriority(7)]
        [Trait("Pagamento", "Integração API - Pagamento")]
        public async Task ObterTodosPagamentos_SemAutenticacao_DeveRetornarUnauthorized()
        {
            _testsFixture.Client.AtribuirToken(null);

            var response = await _testsFixture.Client.GetAsync("/api/v1/pagamentos/obter_todos");

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact(DisplayName = "Obter pagamento por ID sem autenticação deve retornar Unauthorized"), TestPriority(8)]
        [Trait("Pagamento", "Integração API - Pagamento")]
        public async Task ObterPagamentoPorId_SemAutenticacao_DeveRetornarUnauthorized()
        {
            _testsFixture.Client.AtribuirToken(null);

            var idPagamento = Guid.NewGuid();
            var response = await _testsFixture.Client.GetAsync($"/api/v1/pagamentos/obter/{idPagamento}");

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        #endregion

        #region Permissão - Usuário comum sem role ADMIN

        [Fact(DisplayName = "Obter todos os pagamentos como usuário comum deve retornar Forbidden"), TestPriority(9)]
        [Trait("Pagamento", "Integração API - Pagamento")]
        public async Task ObterTodosPagamentos_ComoUsuarioComum_DeveRetornarForbidden()
        {
            await AutenticarComoUsuarioAsync();

            var response = await _testsFixture.Client.GetAsync("/api/v1/pagamentos/obter_todos");

            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact(DisplayName = "Obter pagamento por ID como usuário comum deve retornar Forbidden"), TestPriority(10)]
        [Trait("Pagamento", "Integração API - Pagamento")]
        public async Task ObterPagamentoPorId_ComoUsuarioComum_DeveRetornarForbidden()
        {
            await AutenticarComoUsuarioAsync();

            var idPagamento = Guid.NewGuid(); // mesmo que exista, não importa: não tem permissão
            var response = await _testsFixture.Client.GetAsync($"/api/v1/pagamentos/obter/{idPagamento}");

            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        #endregion

        #region Métodos Auxiliares

        private async Task AutenticarComoAdminAsync()
        {
            await _testsFixture.RealizarLoginComoAdmim();
            _testsFixture.Client.AtribuirToken(_testsFixture.Token);
        }

        private async Task AutenticarComoUsuarioAsync()
        {
            await _testsFixture.RealizarLoginComoUsuario();
            _testsFixture.Client.AtribuirToken(_testsFixture.Token);
        }

        private PagamentoCursoInputModel CriarPagamentoInputModel(
            Guid alunoId,
            Guid matriculaId,
            string numeroCartao = "1111222233334444",
            string expiracao = "11/24",
            string cvv = "123")
        {
            return new PagamentoCursoInputModel
            {
                AlunoId = alunoId,
                MatriculaId = matriculaId,
                Total = 0.01m,
                NomeCartao = "TESTE TESTE",
                NumeroCartao = numeroCartao,
                ExpiracaoCartao = expiracao,
                CvvCartao = cvv
            };
        }

        #endregion
    }
}
