
using EducaMBAXpert.Api.Configuration;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace EducaMBAXpert.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder
                .AddApiConfig()
                .AddCorsConfig()
                .AddSwaggerConfig()
                .AddDbContextConfig()
                .AddIdentityConfig()
                .AddAutoMapperConfig()
                .AddDependencyInjectionConfig()
                .AddMediatRConfig();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
                app.UseSwaggerConfig(apiVersionDescriptionProvider);
                app.UseCors("Development");
            }
            else
            {
                app.UseCors("Production");
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.UseDbMigrationHelper();

            app.Run();
        }
    }
}
