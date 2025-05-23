﻿using AutoMapper;

namespace EducaMBAXpert.Api.Configuration
{

    public static class AutoMapperConfig
    {
        public static WebApplicationBuilder AddAutoMapperConfig(this WebApplicationBuilder builder)
        {
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            return builder;
        }
    }

    public class AutomapperConfiguration : Profile
    {
        public AutomapperConfiguration()
        {
        }
    }

}
