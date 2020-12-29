using System.Collections.Generic;

namespace MyHealth.Web.App.Areas.HealthRecord.Models
{
    public class SearchObservationsResponse
    {
        public IEnumerable<Observation> Observations { get; set; }
    }
}
