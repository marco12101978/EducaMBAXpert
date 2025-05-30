using EducaMBAXpert.Api.Tests.Integration.Config;
using EducaMBAXpert.Api.ViewModels.User;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
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


        [Fact(DisplayName = "Adicionar Novo Aluno"), TestPriority(1)]
        [Trait("ALUNOS", "Integração API - Alunos")]
        public async Task HttpPost_api_v1_alunos_registrar()
        {
            // Arrange
            var usuario = new RegisterUserViewModel
            {
                Nome = "Aluno Teste de Integracao",
                Email = "teste@teste.com",
                Password = "Teste@123",
                ConfirmPassword = "Teste@123"
            };


            // Act
            var response = await _testsFixture.Client.PostAsJsonAsync("/api/v1/alunos/registrar", usuario);

            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.OK,
                $"Esperado que o status code fosse OK, mas foi {response.StatusCode}. Resposta: {await response.Content.ReadAsStringAsync()}");

        }

        [Fact(DisplayName = "Efetuar Login Aluno"), TestPriority(2)]
        [Trait("ALUNOS", "Integração API - Alunos")]
        public async Task HttpPost_api_v1_alunos_login_deve_retornar_OK_para_dados_validos()
        {
            // Arrange
            var usuario = new LoginUserViewModel
            {
                Email = "teste@teste.com",
                Password = "Teste@123",
            };

            // Act
            var response = await _testsFixture.Client.PostAsJsonAsync("/api/v1/alunos/login", usuario);

            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.OK,
                $"Esperado que o status code fosse OK, mas foi {response.StatusCode}");
        }

        [Fact(DisplayName = "Efetuar Login Aluno Retornar BadRequest"), TestPriority(3)]
        [Trait("ALUNOS", "Integração API - Alunos")]
        public async Task HttpPost_api_v1_alunos_login_deve_retornar_BadRequest_para_dados_invalidos()
        {
            // Arrange
            var usuario = new LoginUserViewModel
            {
                Email = "testeXteste.com",
                Password = "Teste@123456",
            };

            // Act
            var response = await _testsFixture.Client.PostAsJsonAsync("/api/v1/alunos/login", usuario);

            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.BadRequest,
                $"Esperado que o status code fosse BadRequest, mas foi {response.StatusCode}");
        }

        [Fact(DisplayName = "Efetuar Login Aluno Retornar Nao Encontrado"), TestPriority(4)]
        [Trait("ALUNOS", "Integração API - Alunos")]
        public async Task HttpPost_api_v1_alunos_login_deve_retornar_NotFound_para_usuario_nao_encontrado()
        {
            // Arrange
            var usuario = new LoginUserViewModel
            {
                Email = "teste@teste.com",
                Password = "Teste@123456",
            };

            // Act
            var response = await _testsFixture.Client.PostAsJsonAsync("/api/v1/alunos/login", usuario);

            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.NotFound,
                $"Esperado que o status code fosse NotFound, mas foi {response.StatusCode}");
        }

        [Fact(DisplayName = "Obter todos Alunos Retornar Nao Autenticado"), TestPriority(5)]
        [Trait("ALUNOS", "Integração API - Alunos")]
        public async Task HttpPost_api_v1_alunos_login_deve_retornar_Unauthorized_para_usuario_nao_autenticado()
        {
            // Arrange

            // Act
            var response = await _testsFixture.Client.GetAsync("/api/v1/alunos/obter_todos");

            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.Unauthorized,
                $"Esperado que o status code fosse Unauthorized, mas foi {response.StatusCode}");
        }

        [Fact(DisplayName = "Obter todos Alunos Retornar Nao Autorizado"), TestPriority(6)]
        [Trait("ALUNOS", "Integração API - Alunos")]
        public async Task HttpPost_api_v1_alunos_login_deve_retornar_Forbidden()
        {
            // Arrange
            await _testsFixture.RealizarLoginApi();
            _testsFixture.Client.AtribuirToken(_testsFixture.TokenAluno);

            // Act
            var response = await _testsFixture.Client.GetAsync("/api/v1/alunos/obter_todos");

            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.Forbidden,
                $"Esperado que o status code fosse Forbidden, mas foi {response.StatusCode}");
        }


        [Fact(DisplayName = "Obter todos Alunos Retornar OK"), TestPriority(7)]
        [Trait("ALUNOS", "Integração API - Alunos")]
        public async Task HttpPost_api_v1_alunos_login_deve_retornar_OK()
        {
            // Arrange

            await _testsFixture.RealizarLoginAdmimApi();
            _testsFixture.Client.AtribuirToken(_testsFixture.TokenAluno);

            // Act
            var response = await _testsFixture.Client.GetAsync("/api/v1/alunos/obter_todos");

            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.OK,
                $"Esperado que o status code fosse OK, mas foi {response.StatusCode}");
        }

    }
}
