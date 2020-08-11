using System.Collections.Generic;

namespace MyHealth.HealthRecord.Models.Responses
{
    public class SearchObservationsResponse
    {
        public IEnumerable<Observation> Observations { get; set; }
    }
}
