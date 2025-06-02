
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using EducaMBAXpert.Api.Tests.Integration.Config;
using Xunit;

namespace EducaMBAXpert.Api.Tests.Integration
{
    public class TestesApi : TesteIntegracaoBase
    {

        //[Fact]
        //public async Task HttpPost_api_v1_matriculas_matricular_idAluno_guid()
        //{
        //    // Arrange
        //    var jsonBody = "{\"example\": \"value\"}";
        //    var content = new StringContent(jsonBody, System.Text.Encoding.UTF8, "application/json");

        //    // Act
        //    var response = await TestClient.PostAsync("/api/v1/matriculas/matricular/00000000-0000-0000-0000-000000000000", content);

        //    // Assert
        //    Assert.True(response.StatusCode == HttpStatusCode.OK, $"Esperado que o status code fosse OK , mas foi {response.StatusCode}");
        //}

        //[Fact]
        //public async Task HttpGet_api_v1_matriculas_aluno_idAluno_guid_matriculas_ativas()
        //{
        //    // Arrange

        //    // Act
        //    var response = await TestClient.GetAsync("/api/v1/matriculas/aluno/00000000-0000-0000-0000-000000000000/matriculas-ativas");

        //    // Assert
        //    Assert.True(response.StatusCode == HttpStatusCode.OK, $"Esperado que o status code fosse OK , mas foi {response.StatusCode}");
        //}

        //[Fact]
        //public async Task HttpGet_api_v1_matriculas_Aluno_idAluno_guid_matriculas_inativas()
        //{
        //    // Arrange

        //    // Act
        //    var response = await TestClient.GetAsync("/api/v1/matriculas/Aluno/00000000-0000-0000-0000-000000000000/matriculas-inativas");

        //    // Assert
        //    Assert.True(response.StatusCode == HttpStatusCode.OK,$"Esperado que o status code fosse OK , mas foi {response.StatusCode}");
        //}

        //[Fact]
        //public async Task HttpPost_api_v1_matriculas_matricula_matriculaId_aula_aulaId_concluir()
        //{
        //    // Arrange
        //    var jsonBody = "{\"example\": \"value\"}";
        //    var content = new StringContent(jsonBody, System.Text.Encoding.UTF8, "application/json");

        //    // Act
        //    var response = await TestClient.PostAsync("/api/v1/matriculas/matricula/1/aula/1/concluir", content);

        //    // Assert
        //    Assert.True(response.StatusCode == HttpStatusCode.OK ,$"Esperado que o status code fosse OK , mas foi {response.StatusCode}");
        //}

        //[Fact]
        //public async Task HttpGet_api_v1_matriculas_matricula_matriculaId_certificado()
        //{
        //    // Arrange

        //    // Act
        //    var response = await TestClient.GetAsync("/api/v1/matriculas/matricula/1/certificado");

        //    // Assert
        //    Assert.True(response.StatusCode == HttpStatusCode.OK ,$"Esperado que o status code fosse OK , mas foi {response.StatusCode}");
        //}

        //[Fact]
        //public async Task HttpGet_api_v1_matriculas_matricula_matriculaId_certificado_download()
        //{
        //    // Arrange

        //    // Act
        //    var response = await TestClient.GetAsync("/api/v1/matriculas/matricula/1/certificado/download");

        //    // Assert
        //    Assert.True(response.StatusCode == HttpStatusCode.OK, $"Esperado que o status code fosse OK , mas foi {response.StatusCode}");
        //}

        //[Fact]
        //public async Task HttpPost_api_v1_pagamentos_pagamento()
        //{
        //    // Arrange
        //    var jsonBody = "{\"example\": \"value\"}";
        //    var content = new StringContent(jsonBody, System.Text.Encoding.UTF8, "application/json");

        //    // Act
        //    var response = await TestClient.PostAsync("/api/v1/pagamentos/pagamento", content);

        //    // Assert
        //    Assert.True(response.StatusCode == HttpStatusCode.OK,$"Esperado que o status code fosse OK , mas foi {response.StatusCode}");
        //}

        //[Fact]
        //public async Task HttpGet_api_v1_pagamentos_obter_todos()
        //{
        //    // Arrange

        //    // Act
        //    var response = await TestClient.GetAsync("/api/v1/pagamentos/obter_todos");

        //    // Assert
        //    Assert.True(response.StatusCode == HttpStatusCode.OK, $"Esperado que o status code fosse OK , mas foi {response.StatusCode}");
        //}

        //[Fact]
        //public async Task HttpGet_api_v1_pagamentos_obter_id_guid()
        //{
        //    // Arrange

        //    // Act
        //    var response = await TestClient.GetAsync("/api/v1/pagamentos/obter/00000000-0000-0000-0000-000000000000");

        //    // Assert
        //    Assert.True(response.StatusCode == HttpStatusCode.OK , $"Esperado que o status code fosse OK , mas foi {response.StatusCode}");
        //}

    }
}