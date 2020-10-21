using System;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyHealth.Integrations.Core.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MyHealth.Integrations.Data.Cosmos
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddCosmos(this IServiceCollection services, IConfiguration configuration)
        {
            var jsonSerializerSettings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            };
            jsonSerializerSettings.Converters.Add(new StringEnumConverter());

            services.AddSingleton(sp => new CosmosClient(
                configuration.GetConnectionString("Cosmos"),
                new CosmosClientOptions
                {
                    // would be configured differently for more frequent usage
                    IdleTcpConnectionTimeout = TimeSpan.FromMinutes(20),

                    Serializer = new CosmosJsonDotNetSerializer(jsonSerializerSettings)
                }));
            services.AddTransient<IIntegrationRepository, CosmosIntegrationRepository>();

            return services;
        }
    }
}
