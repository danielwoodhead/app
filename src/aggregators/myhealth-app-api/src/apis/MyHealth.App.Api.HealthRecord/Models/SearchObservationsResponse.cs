using System.Collections.Generic;

namespace MyHealth.App.Api.HealthRecord.Models
{
    public class SearchObservationsResponse
    {
        public IEnumerable<Observation> Observations { get; set; }
    }
}
