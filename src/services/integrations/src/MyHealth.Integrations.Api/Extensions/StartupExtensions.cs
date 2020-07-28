using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyHealth.Extensions.AspNetCore.Swagger;
using MyHealth.Extensions.Events;
using MyHealth.Extensions.Events.Azure.EventGrid;
using MyHealth.Integrations.Core.Data;
using MyHealth.Integrations.Core.Services;
using MyHealth.Integrations.Core.Utility;
using MyHealth.Integrations.Repository.TableStorage;
using MyHealth.Integrations.Utility;

namespace MyHealth.Integrations.Api.Extensions
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddVersionAwareSwagger(options =>
            {
                options.ApiName = "MyHealth Integrations API";
            });

            return services;
        }

        public static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration)
        {
            // prevent mapping of 'sub' claim
            JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = configuration["Authentication:Authority"];
                    options.Audience = configuration["Authentication:Audience"];
                });

            return services;
        }

        public static IServiceCollection AddIntegrationsCore(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<TableStorageSettings>(configuration.GetSection("TableStorage"));
            services.AddTransient<IIntegrationsService, IntegrationsService>();
            services.AddSingleton<IIntegrationsRepository, TableStorageIntegrationsRepository>();
            services.AddSingleton<IOperationContext, OperationContext>();
            services.AddScoped<IUserOperationContext, UserOperationContext>();

            services.Configure<EventGridSettings>(configuration.GetSection("EventGrid"));
            services.AddSingleton<IEventPublisher, EventGridEventPublisher>();

            services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

            return services;
        }
    }
}
