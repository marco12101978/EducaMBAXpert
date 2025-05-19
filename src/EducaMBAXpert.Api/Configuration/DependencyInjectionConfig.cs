using EducaMBAXpert.Api.Authentication;
using EducaMBAXpert.Api.Interfaces;
using EducaMBAXpert.Api.Notificacoes;
using EducaMBAXpert.CatalagoCursos.Application.Interfaces;
using EducaMBAXpert.CatalagoCursos.Application.Services;
using EducaMBAXpert.CatalagoCursos.Data.Repository;
using EducaMBAXpert.CatalagoCursos.Domain.Interfaces;
using EducaMBAXpert.CatalagoCursos.Domain.Services;
using EducaMBAXpert.Core.Bus;
using EducaMBAXpert.Core.Messages.CommonMessages.Notifications;
using EducaMBAXpert.Pagamentos.AntiCorruption.Interfaces;
using EducaMBAXpert.Pagamentos.AntiCorruption.Services;
using EducaMBAXpert.Pagamentos.Application.Interfaces;
using EducaMBAXpert.Pagamentos.Application.Services;
using EducaMBAXpert.Pagamentos.Business.Interfaces;
using EducaMBAXpert.Pagamentos.Business.Services;
using EducaMBAXpert.Pagamentos.Data.Repository;
using EducaMBAXpert.Alunos.Application.Interfaces;
using EducaMBAXpert.Alunos.Application.Services;
using EducaMBAXpert.Alunos.Data.Repository;
using EducaMBAXpert.Alunos.Domain.Interfaces;
using EducaMBAXpert.Alunos.Domain.Services;
using MediatR;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using EducaMBAXpert.Core.Data;


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

            services.AddScoped<IAlunoRepository, AlunoRepository>();
            services.AddScoped<IAlunoService, AlunoService>();
            services.AddScoped<IAlunoComandoAppService, AlunoAppService>();
            services.AddScoped<IAlunoConsultaAppService, AlunoAppService>();

            services.AddScoped<ICursoRepository, CursoRepository>();
            services.AddScoped<ICursoService, CursoService>();
            services.AddScoped<ICursoConsultaAppService, CursoAppService>();
            services.AddScoped<ICursoComandoAppService, CursoAppService>();

            services.AddScoped<ICursoConsultaService, CursoConsultaService>();

            services.AddScoped<IMatriculaRepository, MatriculaRepository>();
            services.AddScoped<IMatriculaConsultaAppService, MatriculaAppService>();
            services.AddScoped<IMatriculaComandoAppService, MatriculaAppService>();

            services.AddScoped<ICertificadoAppService, CertificadoAppService>();

            services.AddScoped<IPagamentoConsultaAppService, PagamentoAppService>();
            services.AddScoped<IPagamentoComandoAppService, PagamentoAppService>();
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
