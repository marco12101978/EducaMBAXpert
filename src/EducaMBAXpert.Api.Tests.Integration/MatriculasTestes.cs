using EducaMBAXpert.Alunos.Application.ViewModels;
using EducaMBAXpert.Api.Tests.Integration.Config;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace EducaMBAXpert.Api.Tests.Integration
{
    [TestCaseOrderer("EducaMBAXpert.Api.Tests.Integration.Config.PriorityOrderer", "EducaMBAXpert.Api.Tests.Integration")]
    [Collection(nameof(IntegrationWebTestsFixtureCollection))]
    public class MatriculasTestes
    {
        private readonly IntegrationTestsFixture<Program> _testsFixture;

        public MatriculasTestes(IntegrationTestsFixture<Program> testsFixture)
        {
            _testsFixture = testsFixture;
        }

        [Fact(DisplayName = "Matricular aluno com sucesso"),TestPriority(1)]
        [Trait("Matricula", "Integração API - Matricula")]
        public async Task HttpPost_api_v1_matriculas_matricular_ok()
        {
            // Arrange
            await Autenticar();
            var idAluno = _testsFixture.IdAluno;
            var idCurso = await _testsFixture.ObterIdPrimeiroCursoAsync();

            var matricula = new MatriculaInputModel
            {
                AlunoId = idAluno,
                CursoId = idCurso
            };


            // Act
            var response = await _testsFixture.Client.PostAsJsonAsync($"/api/v1/matriculas/matricular/{idAluno}", matricula);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.True(response.StatusCode == HttpStatusCode.NoContent, $"Esperado que o status code fosse NoContent , mas foi {response.StatusCode}");
        }

        [Fact(DisplayName = "Matricular aluno inválido retorna BadRequest")]
        [Trait("Matricula", "Integração API - Matricula")]
        public async Task HttpPost_api_v1_matriculas_matricular_bad_request()
        {
            // Arrange
            await Autenticar();
            var idAluno = Guid.NewGuid(); // ID inválido diferente do enviado no body
            var idCurso = await _testsFixture.ObterIdPrimeiroCursoAsync();

            var matricula = new MatriculaInputModel
            {
                AlunoId = Guid.NewGuid(),
                CursoId = idCurso
            };

            // Act
            var response = await _testsFixture.Client.PostAsJsonAsync($"/api/v1/matriculas/matricular/{idAluno}", matricula);

            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.BadRequest, $"Esperado que o status code fosse BadRequest , mas foi {response.StatusCode}");
        }

        [Fact(DisplayName = "Listar matrículas inativas do aluno"), TestPriority(2)]
        [Trait("Matricula", "Integração API - Matricula")]
        public async Task HttpGet_api_v1_matricula_inativas_ok()
        {
            // Arrange
            await Autenticar();
            var idAluno = _testsFixture.IdAluno;

            // Act
            var response = await _testsFixture.Client.GetAsync($"/api/v1/matriculas/aluno/{idAluno}/matriculas-inativas");

            var json = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.OK, $"Esperado que o status code fosse OK , mas foi {response.StatusCode}");
        }

        [Fact(DisplayName = "Ativar matricular aluno com sucesso"), TestPriority(3)]
        [Trait("Matricula", "Integração API - Matricula")]
        public async Task HttpPost_api_v1_matricula_ativar_ok()
        {
            // Arrange
            await Autenticar();
            var idAluno = _testsFixture.IdAluno;

            var response = await _testsFixture.Client.GetAsync($"/api/v1/matriculas/aluno/{idAluno}/matriculas-inativas");
            var json = JsonConvert.DeserializeObject<List<MatriculaViewModel>>(await response.Content.ReadAsStringAsync());

            var matriculaId = json.First().Id;

            // Act
            var response2 = await _testsFixture.Client.PutAsync($"/api/v1/matriculas/aluno/matriculas/{matriculaId}/ativar",null);
                                                                 

            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.OK, $"Esperado que o status code fosse OK , mas foi {response.StatusCode}");
        }


        [Fact(DisplayName = "Verificar certificado deve retornar OK"), TestPriority(4)]
        [Trait("Matricula", "Integração API - Matricula")]
        public async Task HttpGet_api_v1_matriculas_verificar_certificado_ok()
        {
            // Arrange
            await Autenticar();
            var idMatricula = await _testsFixture.ObterIdPrimeiraMatriculaAsync(_testsFixture.IdAluno);

            // Act
            var response = await _testsFixture.Client.GetAsync($"/api/v1/matriculas/matricula/{idMatricula}/certificado");

            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.OK, $"Esperado que o status code fosse OK , mas foi {response.StatusCode}");
        }

        private async Task Autenticar()
        {
            await _testsFixture.RealizarLoginAdmimApi();
            _testsFixture.Client.AtribuirToken(_testsFixture.Token);
        }

    }
}
