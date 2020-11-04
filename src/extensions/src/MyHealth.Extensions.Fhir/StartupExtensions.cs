using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyHealth.Extensions.Fhir.Authentication;
using MyHealth.Extensions.Fhir.Client;

namespace MyHealth.Extensions.Fhir
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddFhirClient(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMemoryCache();
            services.Configure<FhirClientSettings>(settings =>
            {
                settings.AuthenticationClientId = configuration.GetSection("IntegrationsApi").GetValue<string>("AuthenticationClientId");
                settings.AuthenticationClientSecret = configuration.GetSection("IntegrationsApi").GetValue<string>("AuthenticationClientSecret");
                settings.AuthenticationScope = configuration.GetSection("IntegrationsApi").GetValue<string>("AuthenticationScope");
                settings.AuthenticationTokenEndpoint = configuration.GetSection("IntegrationsApi").GetValue<string>("AuthenticationTokenEndpoint");
                settings.BaseUrl = configuration.GetSection("Fhir").GetValue<string>("BaseUrl");
            });
            services.AddHttpClient<IAuthenticationService, AuthenticationService>();
            services.AddTransient<IFhirClientFactory, FhirClientFactory>();
            services.AddTransient(s =>
            {
                return s.GetRequiredService<IFhirClientFactory>().Create();
            });

            return services;
        }
    }
}
