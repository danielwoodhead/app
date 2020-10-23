using System.Threading.Tasks;

namespace MyHealth.Integrations.Strava.Services
{
    public interface IStravaAuthenticationService
    {
        Task<string> GetAccessTokenAsync(long ownerId);
        string GetAuthenticationUri(string redirectUri);
    }
}
