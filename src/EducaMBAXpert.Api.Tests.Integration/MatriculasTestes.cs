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

        [Fact(DisplayName = "Matricular aluno com sucesso"), TestPriority(1)]
        [Trait("Matricula", "Integração API - Matricula")]
        public async Task Deve_Matricular_Aluno_Com_Sucesso()
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
        }

        [Fact(DisplayName = "Matricular aluno inválido retorna BadRequest")]
        [Trait("Matricula", "Integração API - Matricula")]
        public async Task Deve_Retornar_BadRequest_Para_Matricula_Invalida()
        {
            // Arrange
            await Autenticar();
            var idAlunoRota = Guid.NewGuid();
            var idCurso = await _testsFixture.ObterIdPrimeiroCursoAsync();

            var matricula = new MatriculaInputModel
            {
                AlunoId = Guid.NewGuid(),
                CursoId = idCurso
            };

            // Act
            var response = await _testsFixture.Client.PostAsJsonAsync($"/api/v1/matriculas/matricular/{idAlunoRota}", matricula);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact(DisplayName = "Listar matrículas inativas do aluno"), TestPriority(2)]
        [Trait("Matricula", "Integração API - Matricula")]
        public async Task Deve_Listar_Matriculas_Inativas()
        {
            // Arrange
            await Autenticar();
            var idAluno = _testsFixture.IdAluno;

            // Act
            var response = await _testsFixture.Client.GetAsync($"/api/v1/matriculas/aluno/{idAluno}/matriculas-inativas");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact(DisplayName = "Ativar matrícula do aluno com sucesso"), TestPriority(3)]
        [Trait("Matricula", "Integração API - Matricula")]
        public async Task Deve_Ativar_Matricula_Com_Sucesso()
        {
            // Arrange
            await Autenticar();
            var idAluno = _testsFixture.IdAluno;
            var responseInativas = await _testsFixture.Client.GetAsync($"/api/v1/matriculas/aluno/{idAluno}/matriculas-inativas");
            Assert.Equal(HttpStatusCode.OK, responseInativas.StatusCode);

            var matriculas = JsonConvert.DeserializeObject<List<MatriculaViewModel>>(await responseInativas.Content.ReadAsStringAsync());
            var matriculaId = matriculas.FirstOrDefault()?.Id;
            Assert.NotNull(matriculaId);

            // Act
            var responseAtivar = await _testsFixture.Client.PutAsync($"/api/v1/matriculas/aluno/matriculas/{matriculaId}/ativar", null);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, responseAtivar.StatusCode);
        }

        [Fact(DisplayName = "Verificar certificado - não permitido"), TestPriority(4)]
        [Trait("Matricula", "Integração API - Matricula")]
        public async Task Deve_Retornar_Nao_Permitido_Para_Emissao_Certificado()
        {
            // Arrange
            await Autenticar();
            var matricula = await _testsFixture.ObterPrimeiraMatriculaAsync(_testsFixture.IdAluno);

            // Act
            var response = await _testsFixture.Client.GetAsync($"/api/v1/matriculas/matricula/{matricula.Id}/certificado");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var resultado = JsonConvert.DeserializeObject<ResponseCertificado>(await response.Content.ReadAsStringAsync());

            // Assert
            Assert.False(resultado.podeEmitir, "Esperado que o certificado NÃO pudesse ser emitido, mas foi permitido.");
        }

        [Fact(DisplayName = "Concluir todas as aulas da matrícula com sucesso"), TestPriority(5)]
        [Trait("Matricula", "Integração API - Matricula")]
        public async Task Deve_Concluir_Todas_As_Aulas_Da_Matricula()
        {
            // Arrange
            await Autenticar();
            var idAluno = _testsFixture.IdAluno;
            var matricula = await _testsFixture.ObterPrimeiraMatriculaAsync(idAluno);
            var curso = await _testsFixture.ObterAulasCursoAsync(matricula.CursoId);

            // Act & Assert
            foreach (var modulo in curso.Modulos)
            {
                foreach (var aula in modulo.Aulas)
                {
                    var endpoint = $"/api/v1/matriculas/matricula/{matricula.Id}/aula/{aula.Id}/concluir";
                    var response = await _testsFixture.Client.PostAsync(endpoint, null);

                    Assert.True(response.IsSuccessStatusCode,
                        $"Falha ao concluir aula {aula.Id}. StatusCode: {response.StatusCode}. Endpoint: {endpoint}");
                }
            }
        }

        [Fact(DisplayName = "Verificar certificado - permitido"), TestPriority(6)]
        [Trait("Matricula", "Integração API - Matricula")]
        public async Task Deve_Permitir_Emissao_Certificado_Apos_Conclusao()
        {
            // Arrange
            await Autenticar();
            var matricula = await _testsFixture.ObterPrimeiraMatriculaAsync(_testsFixture.IdAluno);

            // Act
            var response = await _testsFixture.Client.GetAsync($"/api/v1/matriculas/matricula/{matricula.Id}/certificado");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var resultado = JsonConvert.DeserializeObject<ResponseCertificado>(await response.Content.ReadAsStringAsync());

            // Assert
            Assert.True(resultado.podeEmitir, "Esperado que o certificado pudesse ser emitido, mas foi negado.");
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


