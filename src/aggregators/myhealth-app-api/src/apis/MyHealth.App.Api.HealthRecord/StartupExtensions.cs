using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyHealth.App.Api.Core.DependencyInjection;
using MyHealth.App.Api.HealthRecord.Clients;
using MyHealth.App.Api.HealthRecord.Settings;

namespace MyHealth.App.Api.HealthRecord
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddHealthRecordApi(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<HealthRecordApiSettings>(configuration.GetSection("HealthRecordApi"));
            services.AddApiClient<IHealthRecordClient, HealthRecordClient>(configuration["HealthRecordApi:BaseAddress"]);

            return services;
        }
    }
}
