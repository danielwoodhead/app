using System.Collections.Generic;

namespace MyHealth.Web.Core.Models
{
    public class SearchObservationsResponse
    {
        public IEnumerable<Observation> Observations { get; set; }
    }
}
