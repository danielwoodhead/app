using System.Collections.Generic;
using System.Linq;

namespace MyHealth.Web.App.Areas.Integrations.Models
{
    public class GetIntegrationsResponse
    {
        public IEnumerable<Integration> Integrations { get; set; } = Enumerable.Empty<Integration>();
    }
}
