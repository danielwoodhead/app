using System.Collections.Generic;

namespace MyHealth.Observations.Models.Responses
{
    public class SearchObservationsResponse
    {
        public IEnumerable<Observation> Observations { get; set; }
    }
}
