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
    [TestCaseOrderer("EducaMBAXpert.Api.Tests.Integration.Config.PriorityOrderer",
                 "EducaMBAXpert.Api.Tests.Integration")]
    [Collection(nameof(IntegrationWebTestsFixtureCollection))]
    public class MatriculasTestes
    {
        private readonly IntegrationTestsFixture<Program> _testsFixture;

        public MatriculasTestes(IntegrationTestsFixture<Program> testsFixture)
        {
            _testsFixture = testsFixture;
        }

        #region Matrícula

        [Fact(DisplayName = "Matricular aluno com sucesso"), TestPriority(1)]
        [Trait("Matriculas", "Integração API - Matriculas")]
        public async Task MatricularAluno_DeveRetornarNoContent()
        {
            // Arrange
            await AutenticarComoAdmin();
            var idAluno = _testsFixture.IdAluno;
            var idCurso = await _testsFixture.ObterIdPrimeiroCursoAsync();

            var matricula = new MatriculaInputModel
            {
                AlunoId = idAluno,
                CursoId = idCurso
            };

            // Act
            var response = await _testsFixture.Client
                .PostAsJsonAsync($"/api/v1/matriculas/matricular/{idAluno}", matricula);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact(DisplayName = "Matricular aluno com dados inconsistentes deve retornar BadRequest")]
        [Trait("Matriculas", "Integração API - Matriculas")]
        public async Task MatricularAluno_DadosInvalidos_DeveRetornarBadRequest()
        {
            // Arrange
            await AutenticarComoAdmin();

            var matricula = new MatriculaInputModel
            {
                AlunoId = Guid.NewGuid(),
                CursoId = await _testsFixture.ObterIdPrimeiroCursoAsync()
            };

            // Act
            var response = await _testsFixture.Client
                .PostAsJsonAsync($"/api/v1/matriculas/matricular/{Guid.NewGuid()}", matricula);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        #endregion

        #region Matrículas Inativas e Ativação

        [Fact(DisplayName = "Listar matrículas inativas do aluno"), TestPriority(2)]
        [Trait("Matriculas", "Integração API - Matriculas")]
        public async Task ListarMatriculasInativas_DeveRetornarOK()
        {
            // Arrange
            await AutenticarComoAdmin();

            // Act
            var response = await _testsFixture.Client
                .GetAsync($"/api/v1/matriculas/aluno/{_testsFixture.IdAluno}/matriculas-inativas");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact(DisplayName = "Ativar matrícula do aluno"), TestPriority(3)]
        [Trait("Matriculas", "Integração API - Matriculas")]
        public async Task AtivarMatricula_DeveRetornarNoContent()
        {
            // Arrange
            await AutenticarComoAdmin();

            var responseInativas = await _testsFixture.Client
                .GetAsync($"/api/v1/matriculas/aluno/{_testsFixture.IdAluno}/matriculas-inativas");

            var matriculas = JsonConvert.DeserializeObject<List<MatriculaViewModel>>(
                await responseInativas.Content.ReadAsStringAsync());

            var matriculaId = matriculas.FirstOrDefault()?.Id;

            Assert.NotNull(matriculaId);

            // Act
            var response = await _testsFixture.Client
                .PutAsync($"/api/v1/matriculas/aluno/matriculas/{matriculaId}/ativar", null);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        #endregion

        #region Certificado

        [Fact(DisplayName = "Certificado - não permitido inicialmente"), TestPriority(4)]
        [Trait("Matriculas", "Integração API - Matriculas")]
        public async Task VerificarCertificado_NaoPermitido()
        {
            // Arrange
            await AutenticarComoAdmin();
            var matricula = await _testsFixture.ObterPrimeiraMatriculaAsync(_testsFixture.IdAluno);

            // Act
            var response = await _testsFixture.Client
                .GetAsync($"/api/v1/matriculas/matricula/{matricula.Id}/certificado");

            var resultado = JsonConvert.DeserializeObject<ResponseCertificado>(
                await response.Content.ReadAsStringAsync());

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.False(resultado.podeEmitir);
        }

        [Fact(DisplayName = "Concluir todas as aulas da matrícula"), TestPriority(5)]
        [Trait("Matriculas", "Integração API - Matriculas")]
        public async Task ConcluirAulasMatricula_DeveRetornarSucesso()
        {
            // Arrange
            await AutenticarComoAdmin();

            var matricula = await _testsFixture.ObterPrimeiraMatriculaAsync(_testsFixture.IdAluno);
            var curso = await _testsFixture.ObterAulasCursoAsync(matricula.CursoId);

            // Act & Assert
            foreach (var modulo in curso.Modulos)
            {
                foreach (var aula in modulo.Aulas)
                {
                    var endpoint = $"/api/v1/matriculas/matricula/{matricula.Id}/aula/{aula.Id}/concluir";

                    var response = await _testsFixture.Client.PostAsync(endpoint, null);

                    Assert.True(response.IsSuccessStatusCode,
                        $"Falha ao concluir aula {aula.Id}. StatusCode: {response.StatusCode}");
                }
            }
        }

        [Fact(DisplayName = "Certificado - permitido após conclusão"), TestPriority(6)]
        [Trait("Matriculas", "Integração API - Matriculas")]
        public async Task VerificarCertificado_Permitido()
        {
            // Arrange
            await AutenticarComoAdmin();
            var matricula = await _testsFixture.ObterPrimeiraMatriculaAsync(_testsFixture.IdAluno);

            // Act
            var response = await _testsFixture.Client
                .GetAsync($"/api/v1/matriculas/matricula/{matricula.Id}/certificado");

            var resultado = JsonConvert.DeserializeObject<ResponseCertificado>(
                await response.Content.ReadAsStringAsync());

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(resultado.podeEmitir);
        }

        #endregion

        #region Helpers

        private async Task AutenticarComoAdmin()
        {
            await _testsFixture.RealizarLoginComoAdmim();
            _testsFixture.Client.AtribuirToken(_testsFixture.Token);
        }

        private class ResponseCertificado
        {
            public bool podeEmitir { get; set; }
        }

        #endregion
    }
}


