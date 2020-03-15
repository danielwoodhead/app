using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyHealth.Mobile.Core.Models.Observations;

namespace MyHealth.Mobile.Core.Services.Observations
{
    public class ObservationsMockService : IObservationsService
    {
        private readonly IList<Observation> _observations = new List<Observation>
        {
            new Observation { Content = "hello" },
            new Observation { Content = "world" }
        };

        public Task AddObservationAsync(Observation observation)
        {
            _observations.Add(observation);

            return Task.CompletedTask;
        }

        public Task DeleteObservationAsync(string id)
        {
            Observation observation = _observations.FirstOrDefault(o => o.Id == id);

            if (observation != null)
                _observations.Remove(observation);

            return Task.CompletedTask;
        }

        public Task<Observation> GetObservationAsync(string id)
        {
            return Task.FromResult(new Observation { Id = id });
        }

        public Task<IEnumerable<Observation>> GetObservationsAsync()
        {
            return Task.FromResult((IEnumerable<Observation>)_observations);
        }

        public Task UpdateObservationAsync(Observation observation)
        {
            Observation toUpdate = _observations.FirstOrDefault(o => o.Id == observation.Id);

            if (toUpdate != null)
            {
                toUpdate.Content = observation.Content;
            }

            return Task.CompletedTask;
        }
    }
}
