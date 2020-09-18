using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyHealth.HealthRecord.Core.Data;
using MyHealth.HealthRecord.Data.Fhir.Base;

namespace MyHealth.HealthRecord.Data.Fhir
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddFhir(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<FhirServerSettings>(configuration.GetSection("FhirServer"));
            services.AddTransient<IFhirClientFactory, FhirClientFactory>();
            services.AddTransient<IObservationsRepository, FhirObservationsRepository>();

            return services;
        }
    }
}
