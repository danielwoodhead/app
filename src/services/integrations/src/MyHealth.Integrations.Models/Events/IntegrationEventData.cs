using System.Collections.Generic;
using System.Linq;
using MyHealth.Extensions.Events;

namespace MyHealth.Integrations.Models.Events
{
    public class IntegrationEventData : EventData
    {
        public Provider Provider { get; set; }
        public string UserId { get; set; }

        public override IDictionary<string, string> Properties =>
            base.Properties.Union(new Dictionary<string, string>
            {
                { nameof(Provider), Provider.ToString() },
                { nameof(UserId), UserId },
            }).ToDictionary(x => x.Key, x => x.Value);
    }
}
