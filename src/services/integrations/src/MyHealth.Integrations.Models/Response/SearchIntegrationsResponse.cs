using System.Collections.Generic;

namespace MyHealth.Integrations.Models.Response
{
    public class SearchIntegrationsResponse
    {
        public IEnumerable<Integration> Integrations { get; set; }
    }
}
