using System.Threading.Tasks;
using MyHealth.Integrations.Strava.Models;

namespace MyHealth.Integrations.Strava.Clients
{
    public interface IStravaAuthenticationClient
    {
        Task<TokenResponse> AuthenticateAsync(string code);
        Task<TokenResponse> RefreshTokenAsync(string refreshToken);
    }
}
