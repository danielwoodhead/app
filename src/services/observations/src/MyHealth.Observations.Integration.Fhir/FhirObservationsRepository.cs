using System.Threading.Tasks;
using MyHealth.Observations.Core.Repository;
using MyHealth.Observations.Models;
using MyHealth.Observations.Models.Requests;

namespace MyHealth.Observations.Integration.Fhir
{
    public class FhirObservationsRepository : IObservationsRepository
    {
        public Task<Observation> CreateObservationAsync(CreateObservationRequest request)
        {
            throw new System.NotImplementedException();
        }
    }
}
