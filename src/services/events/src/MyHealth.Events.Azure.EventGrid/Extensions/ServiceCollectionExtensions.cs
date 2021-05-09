using Microsoft.Extensions.DependencyInjection;
using MyHealth.Events.Azure.EventGrid.Topics;
using MyHealth.Events.EventIngestion.Topics;

namespace MyHealth.Events.Azure.EventGrid.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAzureEventGrid(this IServiceCollection services)
        {
            services.AddTransient<ITopicFactory, EventGridTopicFactory>();

            return services;
        }
    }
}
