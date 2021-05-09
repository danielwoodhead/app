using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyHealth.Events.Azure.TableStorage.Configuration;
using MyHealth.Events.Azure.TableStorage.Repositories;
using MyHealth.Events.EventIngestion.Repositories;

namespace MyHealth.Events.Azure.TableStorage.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAzureTableStorage(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<TableStorageSettings>(configuration.GetSection("TableStorage"));
            services.AddTransient<IEventRepository, TableStorageEventRepository>();

            return services;
        }
    }
}
