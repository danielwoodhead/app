namespace MyHealth.Integrations.Strava.Services
{
    public interface IStravaService
    {
        string GetAuthenticationUri(string redirectUri);
    }
}
