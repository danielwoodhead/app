using System.Collections.Generic;

namespace MyHealth.Mobile.Core.Models.Observations
{
    public class SearchObservationsResponse
    {
        public IEnumerable<Observation> Observations { get; set; }
    }
}
