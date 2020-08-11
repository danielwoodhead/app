using Microsoft.Extensions.DependencyInjection;
using MyHealth.Integrations.Core.Utility;

namespace MyHealth.Integrations.Utility
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddUtility(this IServiceCollection services)
        {
            services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
            services.AddSingleton<IOperationContext, OperationContext>();
            services.AddScoped<IUserOperationContext, UserOperationContext>();

            return services;
        }
    }
}
