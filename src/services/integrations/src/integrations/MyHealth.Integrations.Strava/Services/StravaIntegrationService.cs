using System;
using System.Threading.Tasks;
using MyHealth.Integrations.Core.Services;
using MyHealth.Integrations.Models;
using MyHealth.Integrations.Strava.Clients;
using MyHealth.Integrations.Strava.Clients.Models;
using MyHealth.Integrations.Strava.Models;

namespace MyHealth.Integrations.Strava.Services
{
    public class StravaIntegrationService : IIntegrationSystemService
    {
        private readonly IStravaClient _stravaClient;

        public StravaIntegrationService(IStravaClient stravaClient)
        {
            _stravaClient = stravaClient;
        }

        public Provider Provider => Provider.Strava;

        public async Task<ProviderResult> CreateIntegrationAsync(ProviderRequest request)
        {
            var requestData = (CreateStravaIntegrationRequest)request.Data;
            TokenResponse tokenResponse = await _stravaClient.AuthenticateAsync(requestData.Code);

            return new ProviderResult
            {
                Provider = Provider,
                ProviderUserId = tokenResponse.Athlete.Id.ToString(),
                Data = new StravaIntegrationData
                {
                    AccessToken = tokenResponse.AccessToken,
                    AccessTokenExpiresUtc = DateTimeOffset.FromUnixTimeSeconds(tokenResponse.ExpiresAt).UtcDateTime,
                    RefreshToken = tokenResponse.RefreshToken
                }
            };
        }

        public Task DeleteIntegrationAsync(string userId)
        {
            // nothing to do - the Strava subscription is for the application, not the user
            return Task.CompletedTask;
        }
    }
}
