using System.Text.Json;
using System.Threading.Tasks;
using MyHealth.Integrations.Core.Services;
using MyHealth.Integrations.Core.Utility;
using MyHealth.Integrations.Fitbit.Models;
using MyHealth.Integrations.Models;

namespace MyHealth.Integrations.Fitbit.Services
{
    public class FitbitTokenService : IFitbitTokenService
    {
        private readonly IIntegrationsService _integrationsService;
        private readonly IDateTimeProvider _dateTimeProvider;

        public FitbitTokenService(
            IIntegrationsService integrationsService,
            IDateTimeProvider dateTimeProvider)
        {
            _integrationsService = integrationsService;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<string> GetAccessTokenAsync(string userId)
        {
            var fitbitIntegration = await _integrationsService.GetIntegrationAsync(Provider.Fitbit, userId);

            var fitbitIntegrationData = JsonSerializer.Deserialize<FitbitIntegrationData>(fitbitIntegration.Data);

            if (fitbitIntegrationData.AccessTokenExpiresUtc < _dateTimeProvider.UtcNow)
            {
                // TODO: get (and store) new access token
            }

            return fitbitIntegrationData.AccessToken;
        }
    }
}
