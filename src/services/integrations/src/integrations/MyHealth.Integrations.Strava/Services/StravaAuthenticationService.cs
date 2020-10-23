using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using MyHealth.Integrations.Core.Data;
using MyHealth.Integrations.Core.Utility;
using MyHealth.Integrations.Models;
using MyHealth.Integrations.Strava.Clients;
using MyHealth.Integrations.Strava.Clients.Models;
using MyHealth.Integrations.Strava.Models;

namespace MyHealth.Integrations.Strava.Services
{
    public class StravaAuthenticationService : IStravaAuthenticationService
    {
        private readonly IIntegrationRepository _integrationRepository;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IStravaClient _stravaClient;
        private readonly StravaSettings _stravaSettings;

        public StravaAuthenticationService(
            IIntegrationRepository integrationRepository,
            IDateTimeProvider dateTimeProvider,
            IStravaClient stravaClient,
            IOptions<StravaSettings> stravaSettings)
        {
            _integrationRepository = integrationRepository;
            _dateTimeProvider = dateTimeProvider;
            _stravaClient = stravaClient;
            _stravaSettings = stravaSettings.Value;
        }

        public async Task<string> GetAccessTokenAsync(long ownerId)
        {
            Integration integration = await _integrationRepository.GetIntegrationAsync(Provider.Strava, ownerId.ToString());

            if (integration is null)
            {
                return null;
            }

            var stravaIntegrationData = (StravaIntegrationData)integration.Data;

            if (stravaIntegrationData.AccessTokenExpiresUtc > _dateTimeProvider.UtcNow.AddMinutes(1))
            {
                return stravaIntegrationData.AccessToken;
            }

            TokenResponse tokenResponse = await _stravaClient.RefreshTokenAsync(stravaIntegrationData.RefreshToken);

            stravaIntegrationData.AccessToken = tokenResponse.AccessToken;
            stravaIntegrationData.AccessTokenExpiresUtc = _dateTimeProvider.UtcNow.AddSeconds(tokenResponse.ExpiresIn);
            stravaIntegrationData.RefreshToken = tokenResponse.RefreshToken;

            await _integrationRepository.UpdateIntegrationAsync(
                integration.UserId,
                Provider.Strava,
                stravaIntegrationData);

            return tokenResponse.AccessToken;
        }

        public string GetAuthenticationUri(string redirectUri)
        {
            return QueryHelpers.AddQueryString(
                _stravaSettings.AuthenticationUrl,
                new Dictionary<string, string>
                {
                    { "client_id", _stravaSettings.ClientId },
                    { "response_type", "code" },
                    { "redirect_uri", redirectUri },
                    { "approval_prompt", "force" },
                    { "scope", "activity:read" }
                });
        }
    }
}
