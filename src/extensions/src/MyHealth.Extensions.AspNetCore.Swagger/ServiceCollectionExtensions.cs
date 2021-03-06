﻿using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MyHealth.Extensions.AspNetCore.Swagger
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMyHealthSwagger(this IServiceCollection services, Action<SwaggerOptions> configure)
        {
            services.Configure(configure);
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerGenOptions>();
            services.AddSwaggerGen();

            return services;
        }
    }
}
