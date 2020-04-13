using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using MyHealth.Mobile.Core.Models.Observations;
using MyHealth.Mobile.Core.Services.Http;

namespace MyHealth.Mobile.Core.Services.Observations
{
    public class ObservationsService : IObservationsService
    {
        private const string ApiUrlBase = GlobalSettings.ObservationsApiUrlBase;

        private readonly HttpClient _httpClient;

        public ObservationsService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.Create();
        }

        public async Task AddObservationAsync(Observation observation)
        {
            await _httpClient.PostAsync<CreateObservationRequest, Observation>(
                ApiUrlBase,
                new CreateObservationRequest
                {
                    Content = observation.Content
                });
        }

        public async Task DeleteObservationAsync(string id)
        {
            await _httpClient.DeleteAsync($"{ApiUrlBase}/{id}");
        }

        public async Task<Observation> GetObservationAsync(string id)
        {
            return await _httpClient.GetAsync<Observation>($"{ApiUrlBase}/{id}");
        }

        public async Task<IEnumerable<Observation>> GetObservationsAsync()
        {
            var response = await _httpClient.GetAsync<SearchObservationsResponse>(ApiUrlBase);

            return response.Observations;
        }

        public async Task UpdateObservationAsync(Observation observation)
        {
            var request = new UpdateObservationRequest
            {
                Content = observation.Content
            };

            await _httpClient.PutAsync<UpdateObservationRequest, Observation>($"{ApiUrlBase}/{observation.Id}", request);
        }
    }
}
