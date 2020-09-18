using Microsoft.Extensions.DependencyInjection;

namespace MyHealth.Extensions.AspNetCore.Context
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddContext(this IServiceCollection services)
        {
            services.AddScoped<IOperationContext, OperationContext>();
            services.AddScoped<IUserOperationContext, UserOperationContext>();

            return services;
        }
    }
}
