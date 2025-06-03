using EducaMBAXpert.Api.Tests.Integration.Config;
using EducaMBAXpert.CatalagoCursos.Application.ViewModels;
using EducaMBAXpert.CatalagoCursos.Domain.Entities;
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
    public class CursosTestes
    {
        private readonly IntegrationTestsFixture<Program> _testsFixture;

        public CursosTestes(IntegrationTestsFixture<Program> testsFixture)
        {
            _testsFixture = testsFixture;
        }

        [Fact(DisplayName = "Adicionar Novo Curso Retornar OK")]
        [Trait("Cursos", "Integração API - Curso")]
        public async Task HttpPost_api_v1_catalogo_curso_novo_ok()
        {
            // Arrange
            await AutenticarComoAdmin();

            CursoInputModel curso = CriarCursoValido();

            // Act
            var response = await _testsFixture.Client.PostAsJsonAsync("/api/v1/catalogo_curso/novo", curso);

            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.OK, $"Esperado que o status code fosse OK , mas foi {response.StatusCode}");
        }

        [Fact(DisplayName = "Adicionar Novo Curso Retornar Unauthorized")]
        [Trait("Cursos", "Integração API - Curso")]
        public async Task HttpPost_api_v1_catalogo_curso_unauthorized()
        {
            // Arrange

            LimparToken();

            CursoInputModel curso = new()
            {
                Titulo = "Design UX/UI para Iniciantes",
                Descricao = "Aprenda os fundamentos do design de experiência do usuário e interface com foco em aplicações modernas.",
                Valor = 129.50m,
                Ativo = true,
                Categoria = CategoriaCurso.Programacao,
                Nivel = NivelDificuldade.Iniciante,
                Modulos = new List<ModuloInputModel>
                {
                    new ModuloInputModel
                    {
                        Nome = "Introdução ao Design",
                        Aulas = new List<AulaInputModel>
                        {
                            new AulaInputModel
                            {
                                Titulo = "Princípios do Design",
                                Duracao = TimeSpan.FromMinutes(40),
                                Url = "https://example.com/video3"
                            },
                            new AulaInputModel
                            {
                                Titulo = "Elementos de Interface",
                                Duracao = TimeSpan.FromMinutes(50),
                                Url = "https://example.com/video4"
                            }
                        }
                    }
                }
            };

            // Act
            var response = await _testsFixture.Client.PostAsJsonAsync("/api/v1/catalogo_curso/novo", curso);

            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.Unauthorized, $"Esperado que o status code fosse Unauthorized , mas foi {response.StatusCode}");
        }

        [Fact(DisplayName = "Adicionar Novo Curso Retornar Bad Request")]
        [Trait("Cursos", "Integração API - Curso")]
        public async Task HttpPost_api_v1_catalogo_curso_BadRequest()
        {
            // Arrange

            await AutenticarComoAdmin();

            CursoInputModel curso = new()
            {

            };

            // Act
            var response = await _testsFixture.Client.PostAsJsonAsync("/api/v1/catalogo_curso/novo", curso);

            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.BadRequest, $"Esperado que o status code fosse BadRequest , mas foi {response.StatusCode}");
        }

        [Fact(DisplayName = "Obter Todos os Cursos Retornar OK")]
        [Trait("Cursos", "Integração API - Curso")]
        public async Task HttpGet_api_v1_catalogo_curso_obter_todos_ok()
        {
            // Arrange
            await AutenticarComoAdmin();

            // Act
            var response = await _testsFixture.Client.GetAsync("/api/v1/catalogo_curso/obter_todos");

            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.OK, $"Esperado que o status code fosse OK , mas foi {response.StatusCode}");
        }

        [Fact(DisplayName = "Obter Curso por ID Retornar OK")]
        [Trait("Cursos", "Integração API - Curso")]
        public async Task HttpGet_api_v1_catalogo_curso_obter_id_guid_ok()
        {
            // Arrange
            await AutenticarComoAdmin();

            var idCurso = await _testsFixture.ObterIdPrimeiroCursoAsync();

            // Act
            var response = await _testsFixture.Client.GetAsync($"/api/v1/catalogo_curso/obter/{idCurso}");

            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.OK, $"Esperado que o status code fosse OK, mas foi {response.StatusCode}");
        }

        [Fact(DisplayName = "Obter Curso por ID Retornar NotFound")]
        [Trait("Cursos", "Integração API - Curso")]
        public async Task HttpGet_api_v1_catalogo_curso_obter_id_guid_notfound()
        {
            // Arrange
            await AutenticarComoAdmin();

            // Act
            var response = await _testsFixture.Client.GetAsync($"/api/v1/catalogo_curso/obter/{Guid.NewGuid()}");

            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.NotFound, $"Esperado que o status code fosse NotFound, mas foi {response.StatusCode}");
        }

        [Fact(DisplayName = "Obter Curso por ID Retornar Unauthorized")]
        [Trait("Cursos", "Integração API - Curso")]
        public async Task HttpGet_api_v1_catalogo_curso_obter_id_guid_unauthorized()
        {
            // Arrange
            await AutenticarComoAdmin();

            var idCurso = await _testsFixture.ObterIdPrimeiroCursoAsync();
            LimparToken();

            // Act
            var response = await _testsFixture.Client.GetAsync($"/api/v1/catalogo_curso/obter/{idCurso}");



            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.Unauthorized, $"Esperado que o status code fosse Unauthorized, mas foi {response.StatusCode}");
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
        private CursoInputModel CriarCursoValido()
        {
            return new CursoInputModel
            {
                Titulo = "Design UX/UI para Iniciantes",
                Descricao = "Aprenda os fundamentos do design de experiência do usuário e interface com foco em aplicações modernas.",
                Valor = 129.50m,
                Ativo = true,
                Categoria = CategoriaCurso.Programacao,
                Nivel = NivelDificuldade.Iniciante,
                Modulos = new List<ModuloInputModel>
        {
            new ModuloInputModel
            {
                Nome = "Introdução ao Design",
                Aulas = new List<AulaInputModel>
                {
                    new AulaInputModel
                    {
                        Titulo = "Princípios do Design",
                        Duracao = TimeSpan.FromMinutes(40),
                        Url = "https://example.com/video3"
                    },
                    new AulaInputModel
                    {
                        Titulo = "Elementos de Interface",
                        Duracao = TimeSpan.FromMinutes(50),
                        Url = "https://example.com/video4"
                    }
                }
            }
        }
            };
        }
    }
}
