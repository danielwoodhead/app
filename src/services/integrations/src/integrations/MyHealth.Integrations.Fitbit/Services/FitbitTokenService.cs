using System;
using System.Threading.Tasks;
using IdentityModel.Client;
using MyHealth.Integrations.Core.Data;
using MyHealth.Integrations.Core.Utility;
using MyHealth.Integrations.Fitbit.Clients;
using MyHealth.Integrations.Fitbit.Models;
using MyHealth.Integrations.Models;

namespace MyHealth.Integrations.Fitbit.Services
{
    public class FitbitTokenService : IFitbitTokenService
    {
        private readonly IIntegrationRepository _integrationsRepository;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IFitbitAuthenticationClient _fitbitAuthenticationClient;

        public FitbitTokenService(
            IIntegrationRepository integrationsRepository,
            IDateTimeProvider dateTimeProvider,
            IFitbitAuthenticationClient fitbitAuthenticationClient)
        {
            _integrationsRepository = integrationsRepository;
            _dateTimeProvider = dateTimeProvider;
            _fitbitAuthenticationClient = fitbitAuthenticationClient;
        }

        public async Task<string> GetAccessTokenAsync(string userId)
        {
            Integration fitbitIntegration = await _integrationsRepository.GetIntegrationAsync(userId, Provider.Fitbit);

            if (fitbitIntegration == null)
                throw new ArgumentException($"Fitbit integration not found for userId '{userId}'");

            var fitbitIntegrationData = (FitbitIntegrationData)fitbitIntegration.Data;

            if (fitbitIntegrationData.AccessTokenExpiresUtc > _dateTimeProvider.UtcNow.AddMinutes(1))
                return fitbitIntegrationData.AccessToken;

            TokenResponse tokenResponse = await _fitbitAuthenticationClient.RefreshTokenAsync(fitbitIntegrationData.RefreshToken);

            if (tokenResponse.IsError)
                throw new Exception($"Failed to refresh Fitbit token: {tokenResponse.Error} - {tokenResponse.ErrorDescription}");

            fitbitIntegrationData.AccessToken = tokenResponse.AccessToken;
            fitbitIntegrationData.AccessTokenExpiresUtc = _dateTimeProvider.UtcNow.AddSeconds(tokenResponse.ExpiresIn);
            fitbitIntegrationData.RefreshToken = tokenResponse.RefreshToken;
            await _integrationsRepository.UpdateIntegrationAsync(userId, Provider.Fitbit, fitbitIntegrationData);

            return tokenResponse.AccessToken;
        }
    }
}
