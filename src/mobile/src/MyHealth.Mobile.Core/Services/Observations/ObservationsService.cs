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

        public async Task AddObservationAsync(Observation observation)
        {
            await _apiClient.PostAsync<CreateObservationRequest, Observation>(
                ApiUrlBase,
                new CreateObservationRequest
                {
                    Content = observation.Content
                });
        }

        public async Task DeleteObservationAsync(string id)
        {
            await _apiClient.DeleteAsync($"{ApiUrlBase}/{id}");
        }

        public async Task<Observation> GetObservationAsync(string id)
        {
            return await _apiClient.GetAsync<Observation>($"{ApiUrlBase}/{id}");
        }

        public async Task<IEnumerable<Observation>> GetObservationsAsync()
        {
            var response = await _apiClient.GetAsync<SearchObservationsResponse>(ApiUrlBase);

            return response.Observations;
        }

        public async Task UpdateObservationAsync(Observation observation)
        {
            await _apiClient.PutAsync($"{ApiUrlBase}/{observation.Id}", observation);
        }
    }
}
