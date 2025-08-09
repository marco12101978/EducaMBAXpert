using EducaMBAXpert.Pagamentos.Data.Context;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;
using System;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace EducaMBAXpert.Api.Tests.Integration.Config
{
    public class LojaAppFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
    {

        protected override IHost CreateHost(IHostBuilder builder)
        {
            ExcluirBaseDadosTest();

            builder.UseEnvironment("Test");
            return base.CreateHost(builder);
        }


        private void ExcluirBaseDadosTest()
        {
            var configuration = new ConfigurationBuilder().SetBasePath(AppContext.BaseDirectory)
                                                          .AddJsonFile("appsettings.Test.json", optional: false)
                                                          .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnectionSqLite");

            var dbPath = connectionString.Replace("Data Source=", "").Trim();

            if (File.Exists(Path.Combine(AppContext.BaseDirectory, dbPath)))
            {
                File.Delete(Path.Combine(AppContext.BaseDirectory, dbPath));
            }
        }

    }
}
