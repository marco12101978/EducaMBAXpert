using EducaMBAXpert.Alunos.Application.ViewModels;
using EducaMBAXpert.Api.Tests.Integration.Config;
using EducaMBAXpert.Api.ViewModels.User;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Runtime.InteropServices.ObjectiveC;
using System.Threading.Tasks;
using Xunit;

namespace EducaMBAXpert.Api.Tests.Integration
{
    [TestCaseOrderer("EducaMBAXpert.Api.Tests.Integration.Config.PriorityOrderer", "EducaMBAXpert.Api.Tests.Integration")]
    [Collection(nameof(IntegrationWebTestsFixtureCollection))]
    public class AlunosTests
    {
        private readonly IntegrationTestsFixture<Program> _testsFixture;

        public AlunosTests(IntegrationTestsFixture<Program> testsFixture)
        {
            _testsFixture = testsFixture;
        }

        #region Cadastro e Login

        [Fact(DisplayName = "Adicionar novo aluno"), TestPriority(1)]
        [Trait("Alunos", "Integração API - Alunos")]
        public async Task RegistrarAluno_DeveRetornarOK()
        {
            var usuario = CriarNovoAluno();

            var response = await _testsFixture.Client.PostAsJsonAsync("/api/v1/alunos/registrar", usuario);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact(DisplayName = "Login com dados válidos"), TestPriority(2)]
        [Trait("Alunos", "Integração API - Alunos")]
        public async Task LoginAluno_DadosValidos_DeveRetornarOK()
        {
            var usuario = CriarLoginValido();

            var response = await _testsFixture.Client.PostAsJsonAsync("/api/v1/alunos/login", usuario);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact(DisplayName = "Login com dados inválidos deve retornar BadRequest"), TestPriority(3)]
        [Trait("Alunos", "Integração API - Alunos")]
        public async Task LoginAluno_DadosInvalidos_DeveRetornarBadRequest()
        {
            var usuario = CriarLoginInvalido();

            var response = await _testsFixture.Client.PostAsJsonAsync("/api/v1/alunos/login", usuario);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact(DisplayName = "Login com usuário inexistente deve retornar NotFound"), TestPriority(4)]
        [Trait("Alunos", "Integração API - Alunos")]
        public async Task LoginAluno_UsuarioInexistente_DeveRetornarNotFound()
        {
            var usuario = new LoginUserViewModel { Email = "teste@teste.com", Password = "Teste@123456" };

            var response = await _testsFixture.Client.PostAsJsonAsync("/api/v1/alunos/login", usuario);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        #endregion

        #region Consultas - Obter Alunos

        [Fact(DisplayName = "Obter todos os alunos sem autenticação deve retornar Unauthorized"), TestPriority(5)]
        [Trait("Alunos", "Integração API - Alunos")]
        public async Task ObterTodosAlunos_SemAutenticacao_DeveRetornarUnauthorized()
        {
            LimparToken();

            var response = await _testsFixture.Client.GetAsync("/api/v1/alunos/obter_todos");

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact(DisplayName = "Obter todos os alunos como aluno comum deve retornar Forbidden"), TestPriority(6)]
        [Trait("Alunos", "Integração API - Alunos")]
        public async Task ObterTodosAlunos_ComoAlunoComum_DeveRetornarForbidden()
        {
            await AutenticarComoAluno();

            var response = await _testsFixture.Client.GetAsync("/api/v1/alunos/obter_todos");

            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact(DisplayName = "Obter todos os alunos como admin deve retornar OK"), TestPriority(7)]
        [Trait("Alunos", "Integração API - Alunos")]
        public async Task ObterTodosAlunos_ComoAdmin_DeveRetornarOK()
        {
            await AutenticarComoAdmin();

            var response = await _testsFixture.Client.GetAsync("/api/v1/alunos/obter_todos");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact(DisplayName = "Obter aluno por ID como admin deve retornar OK"), TestPriority(8)]
        [Trait("Alunos", "Integração API - Alunos")]
        public async Task ObterAlunoPorId_ComoAdmin_DeveRetornarOK()
        {
            await AutenticarComoAdmin();

            var response = await _testsFixture.Client.GetAsync($"/api/v1/alunos/obter/{_testsFixture.IdAluno}");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        #endregion

        #region Endereço do aluno

        [Fact(DisplayName = "Adicionar endereço deve retornar NoContent"), TestPriority(9)]
        [Trait("Alunos", "Integração API - Alunos")]
        public async Task AdicionarEndereco_DeveRetornarNoContent()
        {
            await AutenticarComoAluno();

            var endereco = CriarEndereco(_testsFixture.IdAluno);

            var response = await _testsFixture.Client.PostAsJsonAsync($"/api/v1/alunos/{_testsFixture.IdAluno}/enderecos", endereco);

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact(DisplayName = "Adicionar endereço para aluno inexistente deve retornar NotFound"), TestPriority(10)]
        [Trait("Alunos", "Integração API - Alunos")]
        public async Task AdicionarEndereco_AlunoInexistente_DeveRetornarNotFound()
        {
            await AutenticarComoAluno();

            var endereco = CriarEndereco(_testsFixture.IdAluno);

            var response = await _testsFixture.Client.PostAsJsonAsync($"/api/v1/alunos/{Guid.NewGuid()}/enderecos", endereco);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        #endregion

        #region Ativar/Inativar

        [Fact(DisplayName = "Inativar aluno como admin deve retornar NoContent"), TestPriority(11)]
        [Trait("Alunos", "Integração API - Alunos")]
        public async Task InativarAluno_ComoAdmin_DeveRetornarNoContent()
        {
            await AutenticarComoAdmin();

            var response = await _testsFixture.Client.PutAsync($"/api/v1/alunos/{_testsFixture.IdAluno}/inativar", null);

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact(DisplayName = "Ativar aluno como admin deve retornar NoContent"), TestPriority(12)]
        [Trait("Alunos", "Integração API - Alunos")]
        public async Task AtivarAluno_ComoAdmin_DeveRetornarNoContent()
        {
            await AutenticarComoAdmin();

            var response = await _testsFixture.Client.PutAsync($"/api/v1/alunos/{_testsFixture.IdAluno}/ativar", null);

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact(DisplayName = "Inativar aluno como aluno comum deve retornar Forbidden"), TestPriority(13)]
        [Trait("Alunos", "Integração API - Alunos")]
        public async Task InativarAluno_ComoAlunoComum_DeveRetornarForbidden()
        {
            await AutenticarComoAluno();

            var response = await _testsFixture.Client.PutAsync($"/api/v1/alunos/{_testsFixture.IdAluno}/inativar", null);

            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact(DisplayName = "Ativar aluno como aluno comum deve retornar Forbidden"), TestPriority(14)]
        [Trait("Alunos", "Integração API - Alunos")]
        public async Task AtivarAluno_ComoAlunoComum_DeveRetornarForbidden()
        {
            await AutenticarComoAluno();

            var response = await _testsFixture.Client.PutAsync($"/api/v1/alunos/{_testsFixture.IdAluno}/ativar", null);

            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        #endregion

        #region Helpers

        private async Task AutenticarComoAluno()
        {
            await _testsFixture.RealizarLoginComoUsuario();
            _testsFixture.Client.AtribuirToken(_testsFixture.Token);
        }

        private async Task AutenticarComoAdmin()
        {
            await _testsFixture.RealizarLoginComoAdmim();
            _testsFixture.Client.AtribuirToken(_testsFixture.Token);
        }

        private void LimparToken() => _testsFixture.Client.AtribuirToken(null);

        private RegisterUserViewModel CriarNovoAluno() => new()
        {
            Nome = "Aluno Teste de Integracao",
            Email = "teste@teste.com",
            Password = "Teste@123",
            ConfirmPassword = "Teste@123"
        };

        private LoginUserViewModel CriarLoginValido() => new()
        {
            Email = "teste@teste.com",
            Password = "Teste@123"
        };

        private LoginUserViewModel CriarLoginInvalido() => new()
        {
            Email = "testeXteste.com",
            Password = "Teste@123456"
        };

        private EnderecoInputModel CriarEndereco(Guid alunoId) => new()
        {
            Rua = "Rua José Bonifácio",
            Numero = "123",
            Complemento = "Apto 202",
            Bairro = "Centro",
            Cidade = "Sao Paulo",
            Estado = "SP",
            Cep = "23564-448",
            AlunoId = alunoId
        };

        #endregion
    }
}
