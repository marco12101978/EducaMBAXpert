
using EducaMBAXpert.Api.Context;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Net.Http;

namespace EducaMBAXpert.Api.Tests.Integration
{
    public class TesteIntegracaoBase : IDisposable
    {
        protected readonly HttpClient TestClient;
        private readonly WebApplicationFactory<Program> _factory;

        public TesteIntegracaoBase()
        {
            _factory = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.UseEnvironment("Test"); // <- ESSENCIAL
                    //builder.ConfigureServices(services =>
                    //{
                    //    var descriptor = services.SingleOrDefault(
                    //        d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
                    //    if (descriptor != null)
                    //    {
                    //        services.Remove(descriptor);
                    //    }

                    //    services.AddDbContext<ApplicationDbContext>(options =>
                    //    {
                    //        options.UseInMemoryDatabase("BancoDeTeste");
                    //    });
                    //});
                });

            TestClient = _factory.CreateClient();
        }

        public void Dispose()
        {
            TestClient.Dispose();
            _factory.Dispose();
        }
    }
}
