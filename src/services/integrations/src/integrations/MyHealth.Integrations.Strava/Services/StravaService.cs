using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using MyHealth.Integrations.Core.Services;
using MyHealth.Integrations.Models;
using MyHealth.Integrations.Strava.Clients;
using MyHealth.Integrations.Strava.Models;

namespace MyHealth.Integrations.Strava.Services
{
    public class StravaService : IStravaService, IIntegrationSystemService
    {
        private readonly StravaSettings _stravaSettings;
        private readonly IStravaAuthenticationClient _stravaAuthenticationClient;

        public StravaService(
            IOptions<StravaSettings> stravaSettings,
            IStravaAuthenticationClient stravaAuthenticationClient)
        {
            _stravaSettings = stravaSettings.Value;
            _stravaAuthenticationClient = stravaAuthenticationClient;
        }

        public Provider Provider => Provider.Strava;

        public async Task<ProviderResult> CreateIntegrationAsync(ProviderRequest request)
        {
            var requestData = (CreateStravaIntegrationRequest)request.Data;
            TokenResponse tokenResponse = await _stravaAuthenticationClient.AuthenticateAsync(requestData.Code);

            return new ProviderResult
            {
                Provider = Provider,
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
                    { "scope", "read" }
                });
        }
    }
}
