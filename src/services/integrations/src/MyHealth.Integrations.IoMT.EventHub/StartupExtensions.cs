using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyHealth.Integrations.Core.IoMT;

namespace MyHealth.Integrations.IoMT.EventHub
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddIoMTEventHub(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<EventHubSettings>(configuration.GetSection("IoMT").GetSection("EventHub"));

            services.AddSingleton<IIoMTDataPublisher, EventHubIoMTDataPublisher>();

            return services;
        }
    }
}
