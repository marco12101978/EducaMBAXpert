using Bogus;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System;
using Xunit;
using EducaMBAXpert.Api.ViewModels.User;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using EducaMBAXpert.CatalagoCursos.Application.ViewModels;
using System.Linq;
using EducaMBAXpert.Alunos.Application.ViewModels;
using EducaMBAXpert.Pagamentos.Application.ViewModels;

namespace EducaMBAXpert.Api.Tests.Integration.Config
{
    [CollectionDefinition(nameof(IntegrationWebTestsFixtureCollection))]
    public class IntegrationWebTestsFixtureCollection : ICollectionFixture<IntegrationTestsFixture<Program>> { }

    [CollectionDefinition(nameof(IntegrationApiTestsFixtureCollection))]
    public class IntegrationApiTestsFixtureCollection : ICollectionFixture<IntegrationTestsFixture<Program>> { }

    public class IntegrationTestsFixture<TProgram> : IDisposable where TProgram : class
    {
        public string Token;
        public Guid IdAluno;

        public readonly LojaAppFactory<TProgram> Factory;
        public HttpClient Client;

        public IntegrationTestsFixture()
        {
            var clientOptions = new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = true,
                BaseAddress = new Uri("http://localhost"),
                HandleCookies = true,
                MaxAutomaticRedirections = 7
            };

            Factory = new LojaAppFactory<TProgram>();
            Client = Factory.CreateClient(clientOptions);
        }

        public async Task RealizarLoginComoUsuario()
        {
            var userData = new LoginUserViewModel
            {
                Email = "teste@teste.com",
                Password = "Teste@123"
            };

            Client = Factory.CreateClient();

            var response = await Client.PostAsJsonAsync("/api/v1/alunos/login", userData);

            response.EnsureSuccessStatusCode();

            var contentString = await response.Content.ReadAsStringAsync();
            var usuario = JsonConvert.DeserializeObject<Usuario>(contentString);

            Token = usuario?.token;
            IdAluno = Guid.Parse(usuario?.userId);
        }

        public async Task RealizarLoginComoAdmim()
        {
            var userData = new LoginUserViewModel
            {
                Email = "marco@imperiumsolucoes.com.br",
                Password = "Imp@S2291755"
            };

            Client = Factory.CreateClient();

            var response = await Client.PostAsJsonAsync("/api/v1/alunos/login", userData);

            response.EnsureSuccessStatusCode();

            var contentString = await response.Content.ReadAsStringAsync();
            var usuario = JsonConvert.DeserializeObject<Usuario>(contentString);

            Token = usuario?.token;
            IdAluno = Guid.Parse(usuario?.userId);
        }



        public async Task<Guid> ObterIdPrimeiroCursoAsync()
        {
            var response = await Client.GetAsync("/api/v1/catalogo_curso/obter_todos");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var cursos = JsonConvert.DeserializeObject<List<CursoViewModel>>(json);

            if (cursos == null || !cursos.Any())
                throw new Exception("Nenhum curso encontrado na resposta da API.");

            return cursos.First().Id;
        }

        public async Task<MatriculaViewModel> ObterPrimeiraMatriculaAsync(Guid idAluno)
        {
            var response = await Client.GetAsync($"/api/v1/matriculas/aluno/{idAluno}/matriculas-ativas");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var matricula = JsonConvert.DeserializeObject<List<MatriculaViewModel>>(json);

            if (matricula == null || !matricula.Any())
                throw new Exception("Nenhuma matricula encontrado na resposta da API.");

            return matricula.First();
        }

        public async Task<PagamentoViewModel> ObterPrimeiraPagamentoAsync()
        {
            var response = await Client.GetAsync($"/api/v1/pagamentos/obter_todos");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var pagamento = JsonConvert.DeserializeObject<List<PagamentoViewModel>>(json);

            if (pagamento == null || !pagamento.Any())
                throw new Exception("Nenhuma matricula encontrado na resposta da API.");

            return pagamento.First();
        }



        public async Task<CursoViewModel> ObterAulasCursoAsync(Guid idCurso)
        {
            var response = await Client.GetAsync($"/api/v1/catalogo_curso/obter/{idCurso}");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var curso = JsonConvert.DeserializeObject<CursoViewModel>(json);

            if (curso == null)
                throw new Exception("Nenhum curso encontrado na resposta da API.");

            return curso;
        }

        public void Dispose()
        {
            Client.Dispose();
            Factory.Dispose();
        }


        private class Usuario
        {
            [JsonPropertyName("userId")]
            public string userId { get; set; }

            [JsonPropertyName("token")]
            public string token { get; set; }
        }



    }
}
