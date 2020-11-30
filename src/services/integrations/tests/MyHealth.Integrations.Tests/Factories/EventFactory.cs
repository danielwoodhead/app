using Microsoft.Azure.EventGrid.Models;
using MyHealth.Integrations.Fitbit.Models;
using MyHealth.Integrations.Models;
using MyHealth.Integrations.Models.Events;
using Newtonsoft.Json.Linq;

namespace MyHealth.Integrations.Tests.Factories
{
    internal static class EventFactory
    {
        public static EventGridEvent FitbitProviderUpdateEvent => new EventGridEvent
        {
            EventType = EventTypes.IntegrationProviderUpdate,
            Data = JObject.FromObject(new IntegrationProviderEventData
            {
                Provider = Provider.Fitbit,
                ProviderData = JObject.FromObject(new FitbitUpdateNotification())
            })
        };
    }
}
