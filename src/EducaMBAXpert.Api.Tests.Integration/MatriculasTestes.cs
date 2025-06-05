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


        [Fact(DisplayName = "Verificar certificado deve retornar não permitido"), TestPriority(4)]
        [Trait("Matricula", "Integração API - Matricula")]
        public async Task HttpGet_api_v1_matriculas_verificar_certificado_nao_permitido()
        {
            // Arrange
            await Autenticar();
            Guid idMatricula =  _testsFixture.ObterPrimeiraMatriculaAsync(_testsFixture.IdAluno).Result.Id;

            // Act
            var response = await _testsFixture.Client.GetAsync($"/api/v1/matriculas/matricula/{idMatricula}/certificado");
            var _sucesso = JsonConvert.DeserializeObject<ResponseCertificado>(await response.Content.ReadAsStringAsync());

            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.OK, $"Esperado que o status code fosse OK , mas foi {response.StatusCode}");
            Assert.True(!_sucesso.podeEmitir, $"Esperado que o status code fosse False , mas foi {_sucesso.podeEmitir}");

        }


        [Fact(DisplayName = "Concluir todas as aulas da matrícula com sucesso"), TestPriority(5)]
        [Trait("Matricula", "Integração API - Matricula")]
        public async Task HttpPost_api_v1_aula_concluir_ok()
        {
            // Arrange
            await Autenticar();
            var idAluno = _testsFixture.IdAluno;
            var matricula = await _testsFixture.ObterPrimeiraMatriculaAsync(idAluno);
            var aulaCurso = await _testsFixture.ObterAulasCursoAsync(matricula.CursoId);

            // Act & Assert
            foreach (var modulo in aulaCurso.Modulos)
            {
                foreach (var aula in modulo.Aulas)
                {
                    var endpoint = $"/api/v1/matriculas/matricula/{matricula.Id}/aula/{aula.Id}/concluir";
                    var response = await _testsFixture.Client.PostAsync(endpoint, null);

                    Assert.True(response.IsSuccessStatusCode,
                        $"Falha ao concluir aula {aula.Id}. Status: {response.StatusCode}. Endpoint: {endpoint}");
                }
            }
        }


        [Fact(DisplayName = "Verificar certificado deve retornar permitido"), TestPriority(6)]
        [Trait("Matricula", "Integração API - Matricula")]
        public async Task HttpGet_api_v1_matriculas_verificar_certificado_permitido()
        {
            // Arrange
            await Autenticar();
            var idMatricula = _testsFixture.ObterPrimeiraMatriculaAsync(_testsFixture.IdAluno).Result.Id;

            // Act
            var response = await _testsFixture.Client.GetAsync($"/api/v1/matriculas/matricula/{idMatricula}/certificado");
            var _sucesso = JsonConvert.DeserializeObject<ResponseCertificado>(await response.Content.ReadAsStringAsync());

            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.OK, $"Esperado que o status code fosse OK , mas foi {response.StatusCode}");
            Assert.True(_sucesso.podeEmitir, $"Esperado que o status code fosse True , mas foi {_sucesso.podeEmitir}");

        }





        private async Task Autenticar()
        {
            await _testsFixture.RealizarLoginAdmimApi();
            _testsFixture.Client.AtribuirToken(_testsFixture.Token);
        }

        private class ResponseCertificado
        {
            public bool podeEmitir { get; set; }
        }
    }


}
