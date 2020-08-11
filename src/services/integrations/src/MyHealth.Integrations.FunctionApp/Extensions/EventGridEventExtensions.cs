using System;
using Microsoft.Azure.EventGrid.Models;
using MyHealth.Extensions.Events;
using MyHealth.Extensions.Events.Azure.EventGrid;
using MyHealth.Integrations.Models.Events;

namespace MyHealth.Integrations.FunctionApp.Extensions
{
    public static class EventGridEventExtensions
    {
        public static IEvent ToEvent(this EventGridEvent @event)
        {
            if (@event == null)
                throw new ArgumentNullException(nameof(@event));

            return @event.EventType switch
            {
                EventTypes.IntegrationCreated => @event.ReadAs<IntegrationCreatedEvent, IntegrationEventData>(),
                EventTypes.IntegrationDeleted => @event.ReadAs<IntegrationDeletedEvent, IntegrationEventData>(),
                EventTypes.IntegrationUpdated => @event.ReadAs<IntegrationUpdatedEvent, IntegrationEventData>(),
                EventTypes.IntegrationProviderUpdate => @event.ReadAs<IntegrationProviderUpdateEvent, IntegrationProviderEventData>(),
                _ => throw new NotSupportedException($"Unsupported event type '{@event.EventType}'"),
            };
        }
    }
}
