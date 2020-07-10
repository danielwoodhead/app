using System;
using Microsoft.Azure.EventGrid.Models;
using MyHealth.Extensions.Events;
using MyHealth.Extensions.Events.Azure.EventGrid;
using MyHealth.Integrations.Models.Events;

namespace MyHealth.Integrations.Functions.Extensions
{
    public static class EventGridEventExtensions
    {
        public static IEvent ToEvent(this EventGridEvent @event)
        {
            switch (@event.EventType)
            {
                case EventTypes.IntegrationCreated:
                    return @event.ReadAs<IntegrationCreatedEvent, IntegrationEventData>();
                case EventTypes.IntegrationDeleted:
                    return @event.ReadAs<IntegrationDeletedEvent, IntegrationEventData>();
                case EventTypes.IntegrationUpdated:
                    return @event.ReadAs<IntegrationUpdatedEvent, IntegrationEventData>();
                default:
                    throw new NotSupportedException($"Unsupported event type '{@event.EventType}'");
            }
        }
    }
}
