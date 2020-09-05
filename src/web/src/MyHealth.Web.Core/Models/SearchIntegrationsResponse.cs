using System.Collections.Generic;
using System.Linq;

namespace MyHealth.Web.Core.Models
{
    public class SearchIntegrationsResponse
    {
        public IEnumerable<Integration> Integrations { get; set; } = Enumerable.Empty<Integration>();
    }
}
