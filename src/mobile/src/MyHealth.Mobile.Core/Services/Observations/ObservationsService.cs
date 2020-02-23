using System.Collections.Generic;
using System.Threading.Tasks;
using MyHealth.Mobile.Core.Models.Observations;
using MyHealth.Mobile.Core.Services.Api;

namespace MyHealth.Mobile.Core.Services.Observations
{
    public class ObservationsService : IObservationsService
    {
        private const string ApiUrlBase = "https://myhealth-observations-api.azurewebsites.net/api/v1/observations";

        private readonly IApiClient _apiClient;

        public ObservationsService(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task AddObservationAsync(string observation)
        {
            await _apiClient.PostAsync<CreateObservationRequest, Observation>(
                ApiUrlBase,
                new CreateObservationRequest
                {
                    UserId = "xamarin1", // TODO: remove when auth in place
                    Content = observation
                });
        }

        public Task<IEnumerable<string>> GetObservationsAsync()
        {
            // TODO
            return Task.FromResult((IEnumerable<string>)new[] { "TODO" });
        }
    }
}
