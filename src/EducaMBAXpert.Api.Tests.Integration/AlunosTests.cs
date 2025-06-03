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


        [Fact(DisplayName = "Adicionar Novo Aluno"), TestPriority(1)]
        [Trait("ALUNOS", "Integração API - Alunos")]
        public async Task HttpPost_api_v1_alunos_registrar()
        {
            // Arrange
            var usuario = CriarNovoAluno();


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
            var usuario = CriarLoginValido();

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
            var usuario = CriarLoginInvalido();

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
        public async Task HttpGet_api_v1_alunos_login_deve_retornar_Unauthorized_para_usuario_nao_autenticado()
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
        public async Task HttpGet_api_v1_alunos_login_deve_retornar_Forbidden()
        {
            // Arrange
            await AutenticarComoAluno();

            // Act
            var response = await _testsFixture.Client.GetAsync("/api/v1/alunos/obter_todos");

            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.Forbidden,
                $"Esperado que o status code fosse Forbidden, mas foi {response.StatusCode}");
        }

        [Fact(DisplayName = "Obter todos Alunos Retornar OK"), TestPriority(7)]
        [Trait("ALUNOS", "Integração API - Alunos")]
        public async Task HttpGet_api_v1_alunos_obter_todos_deve_retornar_OK()
        {
            // Arrange
            await AutenticarComoAdmin();

            // Act
            var response = await _testsFixture.Client.GetAsync("/api/v1/alunos/obter_todos");

            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.OK,
                $"Esperado que o status code fosse OK, mas foi {response.StatusCode}");
        }

        [Fact(DisplayName = "Obter Aluno por Id Retornar OK"), TestPriority(7)]
        [Trait("ALUNOS", "Integração API - Alunos")]
        public async Task HttpGet_api_v1_alunos_obter_aluno_por_id_deve_retornar_OK()
        {
            // Arrange
            await AutenticarComoAdmin();
          
            // Act
            var response = await _testsFixture.Client.GetAsync($"/api/v1/alunos/obter/{_testsFixture.IdAluno}");

            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.OK,
                $"Esperado que o status code fosse OK, mas foi {response.StatusCode}");
        }

        [Fact(DisplayName = "Adicionar Endereco Alunos Existente Retornar OK"), TestPriority(8)]
        [Trait("ALUNOS", "Integração API - Alunos")]
        public async Task HttpPost_api_v1_alunos_adicionarEndereco_deve_retornar_NoContent()
        {
            // Arrange

            await AutenticarComoAluno();
            var idAluno = _testsFixture.IdAluno;

            EnderecoInputModel enderecoInputModel = CriarEndereco(idAluno);

            // Act
            var response = await _testsFixture.Client.PostAsJsonAsync($"/api/v1/alunos/{idAluno}/enderecos", enderecoInputModel);

            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.NoContent,
                $"Esperado que o status code fosse NoContent, mas foi {response.StatusCode}");
        }

        [Fact(DisplayName = "Adicionar Endereco Alunos nao Cadastrado Retornar NotFound"), TestPriority(9)]
        [Trait("ALUNOS", "Integração API - Alunos")]
        public async Task HttpPost_api_v1_alunos_adicionarEndereco_deve_retornar_NotFound()
        {
            // Arrange
            await AutenticarComoAluno();
            var idAluno = _testsFixture.IdAluno;

            EnderecoInputModel enderecoInputModel = CriarEndereco(idAluno);


            // Act
            var response = await _testsFixture.Client.PostAsJsonAsync($"/api/v1/alunos/{Guid.NewGuid()}/enderecos", enderecoInputModel);

            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.NotFound,
                $"Esperado que o status code fosse NotFound, mas foi {response.StatusCode}");
        }

        [Fact(DisplayName = "Inativar Alunos deve retonar NoContent"), TestPriority(10)]
        [Trait("ALUNOS", "Integração API - Alunos")]
        public async Task HttpPut_api_v1_alunos_inativar_deve_retornar_NoContent()
        {
            // Arrange

            await AutenticarComoAluno();
            var idAluno = _testsFixture.IdAluno;

            await AutenticarComoAdmin();


            // Act
            var response = await _testsFixture.Client.PutAsync($"/api/v1/alunos/{idAluno}/inativar",null);

            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.NoContent,
                $"Esperado que o status code fosse NoContent, mas foi {response.StatusCode}");
        }

        [Fact(DisplayName = "Ativar Alunos deve retonar NoContent"), TestPriority(11)]
        [Trait("ALUNOS", "Integração API - Alunos")]
        public async Task HttpPut_api_v1_alunos_ativar_deve_retornar_NoContent()
        {
            // Arrange

            await AutenticarComoAluno();
            var idAluno = _testsFixture.IdAluno;

            await AutenticarComoAdmin();

            // Act
            var response = await _testsFixture.Client.PutAsync($"/api/v1/alunos/{idAluno}/ativar", null);

            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.NoContent,
                $"Esperado que o status code fosse NoContent, mas foi {response.StatusCode}");
        }

        [Fact(DisplayName = "Inativar Alunos deve retonar Nao Autorizado"), TestPriority(12)]
        [Trait("ALUNOS", "Integração API - Alunos")]
        public async Task HttpPut_api_v1_alunos_inativar_deve_retornar_Forbidden()
        {
            // Arrange
            await AutenticarComoAluno();
            var idAluno = _testsFixture.IdAluno;

            // Act
            var response = await _testsFixture.Client.PutAsync($"/api/v1/alunos/{idAluno}/inativar", null);

            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.Forbidden,
                $"Esperado que o status code fosse Forbidden, mas foi {response.StatusCode}");
        }

        [Fact(DisplayName = "Ativar Alunos deve retonar Nao Autorizado"), TestPriority(13)]
        [Trait("ALUNOS", "Integração API - Alunos")]
        public async Task HttpPut_api_v1_alunos_ativar_deve_retornar_Forbidden()
        {
            // Arrange
            await AutenticarComoAluno();
            var idAluno = _testsFixture.IdAluno;

            // Act
            var response = await _testsFixture.Client.PutAsync($"/api/v1/alunos/{idAluno}/ativar", null);

            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.Forbidden,
                $"Esperado que o status code fosse Forbidden, mas foi {response.StatusCode}");
        }


        private async Task AutenticarComoAluno()
        {
            await _testsFixture.RealizarLoginApi();
            _testsFixture.Client.AtribuirToken(_testsFixture.Token);
        }

        private async Task AutenticarComoAdmin()
        {
            await _testsFixture.RealizarLoginAdmimApi();
            _testsFixture.Client.AtribuirToken(_testsFixture.Token);
        }

        private void LimparToken()
        {
            _testsFixture.Client.AtribuirToken("");
        }

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
    }
}
