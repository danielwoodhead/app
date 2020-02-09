using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MyHealth.Symptoms.Api.Swagger
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddVersionAwareSwagger(this IServiceCollection services)
        {
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerGenOptions>();
            services.AddSwaggerGen();

            //services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc("v1", new OpenApiInfo { Title = "MyHealth Symptoms API", Version = "v1" });
            //});

            return services;
        }
    }
}
