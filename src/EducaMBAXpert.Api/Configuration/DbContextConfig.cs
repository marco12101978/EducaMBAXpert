using EducaMBAXpert.Api.Context;
using EducaMBAXpert.CatalagoCursos.Data.Context;
using EducaMBAXpert.Pagamentos.Data.Context;
using EducaMBAXpert.Usuarios.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace EducaMBAXpert.Api.Configuration
{
    public static class DbContextConfig
    {
        public static WebApplicationBuilder AddDbContextConfig(this WebApplicationBuilder builder)
        {
            if (builder.Environment.IsDevelopment())
            {
                var connectionString = builder.Configuration.GetConnectionString("DefaultConnectionSqLite") ?? throw new InvalidOperationException("Connection string 'DefaultConnectionSqLite' not found.");

                builder.Services.AddDbContext<CursoContext>(options =>
                    options.UseSqlite(connectionString));

                builder.Services.AddDbContext<UsuarioContext>(options =>
                    options.UseSqlite(connectionString));

                builder.Services.AddDbContext<PagamentoContext>(options =>
                    options.UseSqlite(connectionString));

                builder.Services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlite(connectionString));

                return builder;
            }
            else
            {

                builder.Services.AddDbContext<CursoContext>(options =>
                {
                    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
                });

                builder.Services.AddDbContext<UsuarioContext>(options =>
                {
                    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
                });

                builder.Services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
                });

                return builder;
            }

        }
    }
}
