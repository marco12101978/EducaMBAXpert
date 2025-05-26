
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace EducaMBAXpert.Api.Tests.Integration
{
    public class TestesApiGerados : TesteIntegracaoBase
    {

        [Fact]
        public async Task HttpPost_api_v1_catalogo_curso_novo()
        {
            // Arrange
            var jsonBody = "{\"example\": \"value\"}";
            var content = new StringContent(jsonBody, System.Text.Encoding.UTF8, "application/json");

            // Act
            var response = await TestClient.PostAsync("/api/v1/catalogo_curso/novo", content);

            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.OK , $"Esperado que o status code fosse OK ou BadRequest, mas foi {response.StatusCode}");
        }

        [Fact]
        public async Task HttpGet_api_v1_catalogo_curso_obter_todos()
        {
            // Arrange

            // Act
            var response = await TestClient.GetAsync("/api/v1/catalogo_curso/obter_todos");

            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.OK , $"Esperado que o status code fosse OK ou BadRequest, mas foi {response.StatusCode}");
        }

        [Fact]
        public async Task HttpGet_api_v1_catalogo_curso_obter_id_guid()
        {
            // Arrange

            // Act
            var response = await TestClient.GetAsync("/api/v1/catalogo_curso/obter/00000000-0000-0000-0000-000000000000");

            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.OK , $"Esperado que o status code fosse OK ou BadRequest, mas foi {response.StatusCode}");
        }

        [Fact]
        public async Task HttpPost_api_v1_matriculas_matricular_idAluno_guid()
        {
            // Arrange
            var jsonBody = "{\"example\": \"value\"}";
            var content = new StringContent(jsonBody, System.Text.Encoding.UTF8, "application/json");

            // Act
            var response = await TestClient.PostAsync("/api/v1/matriculas/matricular/00000000-0000-0000-0000-000000000000", content);

            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.OK, $"Esperado que o status code fosse OK ou BadRequest, mas foi {response.StatusCode}");
        }

        [Fact]
        public async Task HttpGet_api_v1_matriculas_aluno_idAluno_guid_matriculas_ativas()
        {
            // Arrange

            // Act
            var response = await TestClient.GetAsync("/api/v1/matriculas/aluno/00000000-0000-0000-0000-000000000000/matriculas-ativas");

            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.OK, $"Esperado que o status code fosse OK ou BadRequest, mas foi {response.StatusCode}");
        }

        [Fact]
        public async Task HttpGet_api_v1_matriculas_Aluno_idAluno_guid_matriculas_inativas()
        {
            // Arrange

            // Act
            var response = await TestClient.GetAsync("/api/v1/matriculas/Aluno/00000000-0000-0000-0000-000000000000/matriculas-inativas");

            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.OK,$"Esperado que o status code fosse OK ou BadRequest, mas foi {response.StatusCode}");
        }

        [Fact]
        public async Task HttpPost_api_v1_matriculas_matricula_matriculaId_aula_aulaId_concluir()
        {
            // Arrange
            var jsonBody = "{\"example\": \"value\"}";
            var content = new StringContent(jsonBody, System.Text.Encoding.UTF8, "application/json");

            // Act
            var response = await TestClient.PostAsync("/api/v1/matriculas/matricula/1/aula/1/concluir", content);

            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.OK ,$"Esperado que o status code fosse OK ou BadRequest, mas foi {response.StatusCode}");
        }

        [Fact]
        public async Task HttpGet_api_v1_matriculas_matricula_matriculaId_certificado()
        {
            // Arrange

            // Act
            var response = await TestClient.GetAsync("/api/v1/matriculas/matricula/1/certificado");

            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.OK ,$"Esperado que o status code fosse OK ou BadRequest, mas foi {response.StatusCode}");
        }

        [Fact]
        public async Task HttpGet_api_v1_matriculas_matricula_matriculaId_certificado_download()
        {
            // Arrange

            // Act
            var response = await TestClient.GetAsync("/api/v1/matriculas/matricula/1/certificado/download");

            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.OK, $"Esperado que o status code fosse OK ou BadRequest, mas foi {response.StatusCode}");
        }

        [Fact]
        public async Task HttpPost_api_v1_pagamentos_pagamento()
        {
            // Arrange
            var jsonBody = "{\"example\": \"value\"}";
            var content = new StringContent(jsonBody, System.Text.Encoding.UTF8, "application/json");

            // Act
            var response = await TestClient.PostAsync("/api/v1/pagamentos/pagamento", content);

            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.OK,$"Esperado que o status code fosse OK ou BadRequest, mas foi {response.StatusCode}");
        }

        [Fact]
        public async Task HttpGet_api_v1_pagamentos_obter_todos()
        {
            // Arrange

            // Act
            var response = await TestClient.GetAsync("/api/v1/pagamentos/obter_todos");

            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.OK, $"Esperado que o status code fosse OK ou BadRequest, mas foi {response.StatusCode}");
        }

        [Fact]
        public async Task HttpGet_api_v1_pagamentos_obter_id_guid()
        {
            // Arrange

            // Act
            var response = await TestClient.GetAsync("/api/v1/pagamentos/obter/00000000-0000-0000-0000-000000000000");

            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.OK , $"Esperado que o status code fosse OK ou BadRequest, mas foi {response.StatusCode}");
        }

        [Fact]
        public async Task HttpPost_api_v1_alunos_registrar()
        {
            // Arrange
            var jsonBody = "{\"example\": \"value\"}";
            var content = new StringContent(jsonBody, System.Text.Encoding.UTF8, "application/json");

            // Act
            var response = await TestClient.PostAsync("/api/v1/alunos/registrar", content);

            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.OK,$"Esperado que o status code fosse OK ou BadRequest, mas foi {response.StatusCode}");
        }
        [Fact]
        public async Task HttpPost_api_v1_alunos_login_deve_retornar_OK_para_dados_validos()
        {
            // Arrange
            var jsonBody = "{\"email\": \"usuario@teste.com\", \"senha\": \"123456\"}";
            var content = new StringContent(jsonBody, System.Text.Encoding.UTF8, "application/json");

            // Act
            var response = await TestClient.PostAsync("/api/v1/alunos/login", content);

            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.OK,
                $"Esperado que o status code fosse OK, mas foi {response.StatusCode}");
        }

        [Fact]
        public async Task HttpPost_api_v1_alunos_login_deve_retornar_BadRequest_para_dados_invalidos()
        {
            // Arrange
            var jsonBody = "{\"campoInvalido\": \"valor\"}";
            var content = new StringContent(jsonBody, System.Text.Encoding.UTF8, "application/json");

            // Act
            var response = await TestClient.PostAsync("/api/v1/alunos/login", content);

            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.BadRequest,
                $"Esperado que o status code fosse BadRequest, mas foi {response.StatusCode}");
        }

        [Fact]
        public async Task HttpPost_api_v1_alunos_login_deve_retornar_Unauthorized_para_usuario_nao_autenticado()
        {
            // Arrange
            var jsonBody = "{\"email\": \"usuario@teste.com\", \"senha\": \"senhaIncorreta\"}";
            var content = new StringContent(jsonBody, System.Text.Encoding.UTF8, "application/json");

            // Act
            var response = await TestClient.PostAsync("/api/v1/alunos/login", content);

            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.Unauthorized,
                $"Esperado que o status code fosse Unauthorized, mas foi {response.StatusCode}");
        }

        [Fact]
        public async Task HttpPost_api_v1_alunos_login_deve_retornar_InternalServerError_para_erro_interno()
        {
            // Arrange
            var jsonBody = "{\"email\": \"causaErro@teste.com\", \"senha\": \"123456\"}";
            var content = new StringContent(jsonBody, System.Text.Encoding.UTF8, "application/json");

            // Act
            var response = await TestClient.PostAsync("/api/v1/alunos/login", content);

            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.InternalServerError,
                $"Esperado que o status code fosse InternalServerError, mas foi {response.StatusCode}");
        }



        [Fact]
        public async Task HttpPost_api_v1_alunos_id_guid_enderecos()
        {
            // Arrange
            var jsonBody = "{\"example\": \"value\"}";
            var content = new StringContent(jsonBody, System.Text.Encoding.UTF8, "application/json");

            // Act
            var response = await TestClient.PostAsync("/api/v1/alunos/00000000-0000-0000-0000-000000000000/enderecos", content);

            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.OK , $"Esperado que o status code fosse OK ou BadRequest, mas foi {response.StatusCode}");
        }

        [Fact]
        public async Task HttpGet_api_v1_alunos_obter_todos()
        {
            // Arrange

            // Act
            var response = await TestClient.GetAsync("/api/v1/alunos/obter_todos");

            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.OK , $"Esperado que o status code fosse OK ou BadRequest, mas foi {response.StatusCode}");
        }

        [Fact]
        public async Task HttpGet_api_v1_alunos_obter_id_guid()
        {
            // Arrange

            // Act
            var response = await TestClient.GetAsync("/api/v1/alunos/obter/00000000-0000-0000-0000-000000000000");

            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.OK , $"Esperado que o status code fosse OK ou BadRequest, mas foi {response.StatusCode}");
        }

        [Fact]
        public async Task HttpPut_api_v1_alunos_id_guid_inativar()
        {
            // Arrange
            var jsonBody = "{\"example\": \"value\"}";
            var content = new StringContent(jsonBody, System.Text.Encoding.UTF8, "application/json");

            // Act
            var response = await TestClient.PutAsync("/api/v1/alunos/00000000-0000-0000-0000-000000000000/inativar", content);

            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.OK , $"Esperado que o status code fosse OK ou BadRequest, mas foi {response.StatusCode}");
        }

        [Fact]
        public async Task HttpPut_api_v1_alunos_id_guid_ativar()
        {
            // Arrange
            var jsonBody = "{\"example\": \"value\"}";
            var content = new StringContent(jsonBody, System.Text.Encoding.UTF8, "application/json");

            // Act
            var response = await TestClient.PutAsync("/api/v1/alunos/00000000-0000-0000-0000-000000000000/ativar", content);

            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.OK , $"Esperado que o status code fosse OK ou BadRequest, mas foi {response.StatusCode}");
        }

    }
}