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

namespace EducaMBAXpert.Api.Tests.Integration.Config
{
    [CollectionDefinition(nameof(IntegrationWebTestsFixtureCollection))]
    public class IntegrationWebTestsFixtureCollection : ICollectionFixture<IntegrationTestsFixture<Program>> { }

    [CollectionDefinition(nameof(IntegrationApiTestsFixtureCollection))]
    public class IntegrationApiTestsFixtureCollection : ICollectionFixture<IntegrationTestsFixture<Program>> { }

    public class IntegrationTestsFixture<TProgram> : IDisposable where TProgram : class
    {

        public string TokenAluno;
        public string IdAluno;

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

        public async Task RealizarLoginApi()
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

            TokenAluno = usuario?.token;
            IdAluno = usuario?.userId;
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
