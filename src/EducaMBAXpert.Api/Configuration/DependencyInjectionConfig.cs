using EducaMBAXpert.Api.Authentication;
using EducaMBAXpert.Api.Interfaces;
using EducaMBAXpert.Api.Notificacoes;
using EducaMBAXpert.CatalagoCursos.Application.Interfaces;
using EducaMBAXpert.CatalagoCursos.Application.Services;
using EducaMBAXpert.CatalagoCursos.Data.Repository;
using EducaMBAXpert.CatalagoCursos.Domain.Interfaces;
using EducaMBAXpert.CatalagoCursos.Domain.Services;
using EducaMBAXpert.Certificados.Services;
using EducaMBAXpert.Contracts.Certificados;
using EducaMBAXpert.Contracts.Cursos;
using EducaMBAXpert.Core.Bus;
using EducaMBAXpert.Core.Messages.CommonMessages.Notifications;
using EducaMBAXpert.Pagamentos.AntiCorruption.Interfaces;
using EducaMBAXpert.Pagamentos.AntiCorruption.Services;
using EducaMBAXpert.Pagamentos.Application.Interfaces;
using EducaMBAXpert.Pagamentos.Application.Services;
using EducaMBAXpert.Pagamentos.Business.Interfaces;
using EducaMBAXpert.Pagamentos.Business.Services;
using EducaMBAXpert.Pagamentos.Data.Repository;
using EducaMBAXpert.Usuarios.Application.Interfaces;
using EducaMBAXpert.Usuarios.Application.Services;
using EducaMBAXpert.Usuarios.Data.Repository;
using EducaMBAXpert.Usuarios.Domain.Interfaces;
using EducaMBAXpert.Usuarios.Domain.Services;
using MediatR;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;


namespace EducaMBAXpert.Api.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static WebApplicationBuilder AddDependencyInjectionConfig(this WebApplicationBuilder builder)
        {
            builder.Services.ResolveDependencies();
            return builder;
        }

        public static IServiceCollection ResolveDependencies(this IServiceCollection services)
        {
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

            services.AddScoped<IMediatrHandler, MediatorHandler>();

            services.AddScoped<DomainNotificationHandler>();
            services.AddScoped<NotificationContext>();
            services.AddScoped<INotificationHandler<DomainNotification>>(provider => provider.GetService<DomainNotificationHandler>());

            services.AddScoped<IAppIdentityUser, AppIdentityUser>();
            services.AddScoped<INotificador, Notificador>();

            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            services.AddScoped<IUsuarioService, UsuarioService>();
            services.AddScoped<IUsuarioAppService, UsuarioAppService>();

            services.AddScoped<ICursoRepository, CursoRepository>();
            services.AddScoped<ICursoService, CursoService>();
            services.AddScoped<ICursoConsultaAppService, CursoAppService>();
            services.AddScoped<ICursoComandoAppService, CursoAppService>();

            services.AddScoped<ICursoConsultaService, CursoConsultaService>();

            services.AddScoped<IMatriculaRepository, MatriculaRepository>();
            services.AddScoped<IMatriculaAppService, MatriculaAppService>();

            services.AddScoped<ICertificadoAppService, CertificadoAppService>();
            services.AddScoped<ICertificadoService, CertificadoService>();

            services.AddScoped<IPagamentoAppService, PagamentoAppService>();
            services.AddScoped<IPagamentoRepository, PagamentoRepository>();
            services.AddScoped<IPagamentoService, PagamentoService>();
            services.AddScoped<IPagamentoCartaoCreditoFacade, PagamentoCartaoCreditoFacade>();
            services.AddScoped<IPayPalGateway, PayPalGateway>();
            services.AddScoped<Pagamentos.AntiCorruption.Interfaces.IConfigurationManager, Pagamentos.AntiCorruption.Services.ConfigurationManager>();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());



            return services;
        }
    }
}
