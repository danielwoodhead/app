using System.Collections.Generic;

namespace MyHealth.Integrations.Models.Response
{
    public class GetIntegrationsResponse
    {
        public IEnumerable<Integration> Integrations { get; set; }
    }
}
