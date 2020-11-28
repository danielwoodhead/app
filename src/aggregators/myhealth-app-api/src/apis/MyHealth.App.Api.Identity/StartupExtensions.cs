using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyHealth.App.Api.Core.Authentication;
using MyHealth.App.Api.Core.DependencyInjection;
using MyHealth.App.Api.Identity.Clients;
using MyHealth.App.Api.Identity.Services;
using MyHealth.App.Api.Identity.Settings;

namespace MyHealth.App.Api.Identity
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddIdentityApi(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<IdentityApiSettings>(configuration.GetSection("IdentityApi"));
            services.AddTransient<ITokenService, TokenService>();
            services.AddTransient<IUserContext, UserContext>();
            services.AddHttpClient<ITokenClient, TokenClient>();
            services.AddApiClient<IIdentityClient, IdentityClient>(configuration["IdentityApi:BaseAddress"]);

            return services;
        }
    }
}
