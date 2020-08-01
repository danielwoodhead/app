using System;
using System.Threading.Tasks;
using IdentityModel.Client;

namespace MyHealth.Integrations.Fitbit.Clients
{
    public interface IFitbitAuthenticationClient
    {
        Task<TokenResponse> AuthenticateAsync(string code, Uri redirectUri);
    }
}
