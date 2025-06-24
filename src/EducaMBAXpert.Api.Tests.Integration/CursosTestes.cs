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
    [TestCaseOrderer("EducaMBAXpert.Api.Tests.Integration.Config.PriorityOrderer",
                 "EducaMBAXpert.Api.Tests.Integration")]
    [Collection(nameof(IntegrationWebTestsFixtureCollection))]
    public class CursosTestes
    {
        private readonly IntegrationTestsFixture<Program> _testsFixture;

        public CursosTestes(IntegrationTestsFixture<Program> testsFixture)
        {
            _testsFixture = testsFixture;
        }

        #region Cadastro de Curso

        [Fact(DisplayName = "Adicionar novo curso deve retornar OK")]
        [Trait("Cursos", "Integração API - Cursos")]
        public async Task AdicionarCurso_ComoAdmin_DeveRetornarOK()
        {
            // Arrange
            await AutenticarComoAdmin();
            var curso = CriarCursoValido();

            // Act
            var response = await _testsFixture.Client.PostAsJsonAsync("/api/v1/catalogo_curso/novo", curso);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact(DisplayName = "Adicionar novo curso sem autenticação deve retornar Unauthorized")]
        [Trait("Cursos", "Integração API - Cursos")]
        public async Task AdicionarCurso_SemToken_DeveRetornarUnauthorized()
        {
            // Arrange
            LimparToken();
            var curso = CriarCursoValido();

            // Act
            var response = await _testsFixture.Client.PostAsJsonAsync("/api/v1/catalogo_curso/novo", curso);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact(DisplayName = "Adicionar novo curso com dados inválidos deve retornar BadRequest")]
        [Trait("Cursos", "Integração API - Cursos")]
        public async Task AdicionarCurso_DadosInvalidos_DeveRetornarBadRequest()
        {
            // Arrange
            await AutenticarComoAdmin();
            var curso = new CursoInputModel();

            // Act
            var response = await _testsFixture.Client.PostAsJsonAsync("/api/v1/catalogo_curso/novo", curso);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        #endregion

        #region Consultas de Curso

        [Fact(DisplayName = "Obter todos os cursos como admin deve retornar OK")]
        [Trait("Cursos", "Integração API - Cursos")]
        public async Task ObterTodosCursos_ComoAdmin_DeveRetornarOK()
        {
            // Arrange
            await AutenticarComoAdmin();

            // Act
            var response = await _testsFixture.Client.GetAsync("/api/v1/catalogo_curso/obter_todos");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact(DisplayName = "Obter curso por ID deve retornar OK")]
        [Trait("Cursos", "Integração API - Cursos")]
        public async Task ObterCursoPorId_Existente_DeveRetornarOK()
        {
            // Arrange
            await AutenticarComoAdmin();
            var idCurso = await _testsFixture.ObterIdPrimeiroCursoAsync();

            // Act
            var response = await _testsFixture.Client.GetAsync($"/api/v1/catalogo_curso/obter/{idCurso}");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact(DisplayName = "Obter curso por ID inexistente deve retornar NotFound")]
        [Trait("Cursos", "Integração API - Cursos")]
        public async Task ObterCursoPorId_Inexistente_DeveRetornarNotFound()
        {
            // Arrange
            await AutenticarComoAdmin();
            var idInexistente = Guid.NewGuid();

            // Act
            var response = await _testsFixture.Client.GetAsync($"/api/v1/catalogo_curso/obter/{idInexistente}");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact(DisplayName = "Obter curso por ID sem autenticação deve retornar Unauthorized")]
        [Trait("Cursos", "Integração API - Cursos")]
        public async Task ObterCursoPorId_SemToken_DeveRetornarUnauthorized()
        {
            // Arrange
            await AutenticarComoAdmin();
            var idCurso = await _testsFixture.ObterIdPrimeiroCursoAsync();
            LimparToken();

            // Act
            var response = await _testsFixture.Client.GetAsync($"/api/v1/catalogo_curso/obter/{idCurso}");

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        #endregion

        #region Helpers

        private async Task AutenticarComoAdmin()
        {
            await _testsFixture.RealizarLoginComoAdmim();
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

        #endregion
    }
}

