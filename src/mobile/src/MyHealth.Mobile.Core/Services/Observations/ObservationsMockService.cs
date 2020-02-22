using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyHealth.Mobile.Core.Services.Observations
{
    public class ObservationsMockService : IObservationsService
    {
        private readonly IList<string> _observations = new List<string> { "hello", "world" };

        public Task AddObservationAsync(string observation)
        {
            _observations.Add(observation);

            return Task.CompletedTask;
        }

        public Task<IEnumerable<string>> GetObservationsAsync()
        {
            return Task.FromResult((IEnumerable<string>)_observations);
        }
    }
}
