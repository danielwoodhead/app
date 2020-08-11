using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyHealth.Integrations.Core.Data;

namespace MyHealth.Integrations.Data.TableStorage
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddTableStorage(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            services.Configure<TableStorageSettings>(configuration.GetSection("TableStorage"));
            services.AddSingleton<IIntegrationRepository, TableStorageIntegrationsRepository>();

            return services;
        }
    }
}
