using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyHealth.App.Api.Core.DependencyInjection;
using MyHealth.App.Api.Integrations.Clients;
using MyHealth.App.Api.Integrations.Settings;

namespace MyHealth.App.Api.Integrations
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddIntegrationsApi(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<IntegrationApiSettings>(configuration.GetSection("IntegrationsApi"));
            services.AddApiClient<IIntegrationsClient, IntegrationsClient>(configuration["IntegrationsApi:BaseAddress"]);

            return services;
        }
    }
}
