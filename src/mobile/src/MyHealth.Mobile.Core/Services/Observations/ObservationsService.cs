using System.Collections.Generic;
using System.Threading.Tasks;
using MyHealth.Mobile.Core.Services.Api;

namespace MyHealth.Mobile.Core.Services.Observations
{
    public class ObservationsService : IObservationsService
    {
        private readonly IApiClient _apiClient;

        public ObservationsService(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public Task AddObservationAsync(string observation)
        {
            // TODO
            return Task.CompletedTask;
        }

        public Task<IEnumerable<string>> GetObservationsAsync()
        {
            // TODO
            return Task.FromResult((IEnumerable<string>)new[] { "TODO" });
        }
    }
}
