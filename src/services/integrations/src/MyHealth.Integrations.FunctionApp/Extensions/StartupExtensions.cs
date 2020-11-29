using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyHealth.Extensions.Fhir;

namespace MyHealth.Integrations.FunctionApp.Extensions
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddFhir(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddFhirClient(settings =>
            {
                settings.BaseUrl = configuration.GetSection("Fhir").GetValue<string>("BaseUrl");
                settings.Timeout = configuration.GetSection("Fhir").GetValue<TimeSpan>("Timeout");
                settings.AuthenticationMode = AuthenticationMode.ClientCredentials;
                settings.AuthenticationTokenEndpoint = configuration.GetSection("IntegrationsApi").GetValue<string>("AuthenticationTokenEndpoint");
                settings.AuthenticationClientId = configuration.GetSection("IntegrationsApi").GetValue<string>("AuthenticationClientId");
                settings.AuthenticationClientSecret = configuration.GetSection("IntegrationsApi").GetValue<string>("AuthenticationClientSecret");
                settings.AuthenticationScope = configuration.GetSection("IntegrationsApi").GetValue<string>("AuthenticationScope");
            });

            return services;
        }
    }
}
