using EducaMBAXpert.Api.Tests.Integration.Config;
using EducaMBAXpert.Api.ViewModels.User;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace EducaMBAXpert.Api.Tests.Integration
{
    public class AlunosTests :  TesteIntegracaoBase
    {
        //[Fact]
        //public async Task HttpPost_api_v1_alunos_registrar()
        //{
        //    //// Arrange
        //    //var jsonBody = "{\"example\": \"value\"}";
        //    //var content = new StringContent(jsonBody, System.Text.Encoding.UTF8, "application/json");

        //    //// Act
        //    //var response = await TestClient.PostAsJsonAsync("/api/v1/alunos/registrar", content);

        //    //// Assert
        //    //Assert.True(response.StatusCode == HttpStatusCode.OK, $"Esperado que o status code fosse OK , mas foi {response.StatusCode}");


        //    var usuario = new RegisterUserViewModel
        //    {
        //        Nome = "João da Silva",
        //        Email = "joao@email.com",
        //        Password = "Senha@123",           
        //        ConfirmPassword = "Senha@123"
        //    };

        //    var content = new StringContent(JsonConvert.SerializeObject(usuario), Encoding.UTF8, "application/json");

        //    // Act
        //    var response = await TestClient.PostAsync("/api/v1/alunos/registrar", content);

        //    // Assert
        //    Assert.True(response.StatusCode == HttpStatusCode.OK,
        //        $"Esperado que o status code fosse OK, mas foi {response.StatusCode}. Resposta: {await response.Content.ReadAsStringAsync()}");



        //}
        //[Fact]
        //public async Task HttpPost_api_v1_alunos_login_deve_retornar_OK_para_dados_validos()
        //{
        //    // Arrange
        //    var jsonBody = "{\"email\": \"usuario@teste.com\", \"senha\": \"123456\"}";
        //    var content = new StringContent(jsonBody, System.Text.Encoding.UTF8, "application/json");

        //    // Act
        //    var response = await TestClient.PostAsync("/api/v1/alunos/login", content);

        //    // Assert
        //    Assert.True(response.StatusCode == HttpStatusCode.OK,
        //        $"Esperado que o status code fosse OK, mas foi {response.StatusCode}");
        //}

        //[Fact]
        //public async Task HttpPost_api_v1_alunos_login_deve_retornar_BadRequest_para_dados_invalidos()
        //{
        //    // Arrange
        //    var jsonBody = "{\"campoInvalido\": \"valor\"}";
        //    var content = new StringContent(jsonBody, System.Text.Encoding.UTF8, "application/json");

        //    // Act
        //    var response = await TestClient.PostAsync("/api/v1/alunos/login", content);

        //    // Assert
        //    Assert.True(response.StatusCode == HttpStatusCode.BadRequest,
        //        $"Esperado que o status code fosse BadRequest, mas foi {response.StatusCode}");
        //}

        //[Fact]
        //public async Task HttpPost_api_v1_alunos_login_deve_retornar_Unauthorized_para_usuario_nao_autenticado()
        //{
        //    // Arrange
        //    var jsonBody = "{\"email\": \"usuario@teste.com\", \"senha\": \"senhaIncorreta\"}";
        //    var content = new StringContent(jsonBody, System.Text.Encoding.UTF8, "application/json");

        //    // Act
        //    var response = await TestClient.PostAsync("/api/v1/alunos/login", content);

        //    // Assert
        //    Assert.True(response.StatusCode == HttpStatusCode.Unauthorized,
        //        $"Esperado que o status code fosse Unauthorized, mas foi {response.StatusCode}");
        //}

        //[Fact]
        //public async Task HttpPost_api_v1_alunos_login_deve_retornar_InternalServerError_para_erro_interno()
        //{
        //    // Arrange
        //    var jsonBody = "{\"email\": \"causaErro@teste.com\", \"senha\": \"123456\"}";
        //    var content = new StringContent(jsonBody, System.Text.Encoding.UTF8, "application/json");

        //    // Act
        //    var response = await TestClient.PostAsync("/api/v1/alunos/login", content);

        //    // Assert
        //    Assert.True(response.StatusCode == HttpStatusCode.InternalServerError,
        //        $"Esperado que o status code fosse InternalServerError, mas foi {response.StatusCode}");
        //}


    }
}
