using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MyHealth.Observations.Api.Swagger
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddVersionAwareSwagger(this IServiceCollection services)
        {
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerGenOptions>();
            services.AddSwaggerGen();

            return services;
        }
    }
}
