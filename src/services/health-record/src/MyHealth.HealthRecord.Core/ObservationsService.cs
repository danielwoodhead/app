using System.Collections.Generic;
using System.Threading.Tasks;
using MyHealth.Extensions.AspNetCore.Context;
using MyHealth.Extensions.Events;
using MyHealth.HealthRecord.Core.Data;
using MyHealth.HealthRecord.Models;
using MyHealth.HealthRecord.Models.Responses;

namespace MyHealth.HealthRecord.Core
{
    public class ObservationsService : IObservationsService
    {
        private readonly IObservationsRepository _repository;
        private readonly IUserOperationContext _operationContext;

        public ObservationsService(
            IObservationsRepository repository,
            IUserOperationContext operationContext)
        {
            _repository = repository;
            _operationContext = operationContext;
        }

        public async Task<SearchObservationsResponse> SearchObservationsAsync()
        {
            IEnumerable<Observation> observations = await _repository.GetObservationsAsync(_operationContext.UserId);

            return new SearchObservationsResponse
            {
                Observations = observations
            };
        }
    }
}
