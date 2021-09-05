using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyHealth.Events.Azure.BlobStorage.Configuration;
using MyHealth.Events.Azure.BlobStorage.Repositories;
using MyHealth.Events.Azure.BlobStorage.Utility;
using MyHealth.Events.EventIngestion.Repositories;

namespace MyHealth.Events.Azure.BlobStorage.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAzureBlobStorage(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<BlobStorageSettings>(configuration.GetSection("BlobStorage"));
            services.AddTransient<IEventRepository, BlobStorageEventRepository>();
            services.AddSingleton<ISystemClock, SystemClock>();

            return services;
        }
    }
}
