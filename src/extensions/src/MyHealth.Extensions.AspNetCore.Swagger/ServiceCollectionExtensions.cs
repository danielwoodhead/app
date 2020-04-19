using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MyHealth.Extensions.AspNetCore.Swagger
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddVersionAwareSwagger(this IServiceCollection services, Action<SwaggerOptions> configure)
        {
            //var options = new SwaggerOptions();
            //configure(options);
            services.Configure(configure);
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerGenOptions>();
            services.AddSwaggerGen();

            return services;
        }
    }
}
