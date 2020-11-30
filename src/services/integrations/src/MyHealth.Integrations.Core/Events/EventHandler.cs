using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MyHealth.Extensions.Events;
using MyHealth.Integrations.Core.Events.Handlers;
using MyHealth.Integrations.Models.Events;

namespace MyHealth.Integrations.Core.Events
{
    public class EventHandler : IEventHandler
    {
        private readonly IEnumerable<IIntegrationEventHandler> _eventHandlers;
        private readonly ILogger<EventHandler> _logger;

        public EventHandler(
            IEnumerable<IIntegrationEventHandler> eventHandlers,
            ILogger<EventHandler> logger)
        {
            _eventHandlers = eventHandlers;
            _logger = logger;
        }

        public async Task ProcessAsync(IEvent @event)
        {
            if (@event is null)
            {
                throw new ArgumentNullException(nameof(@event));
            }

            var data = (IntegrationEventData)@event.Data;
            IEnumerable<IIntegrationEventHandler> eventHandlers = _eventHandlers.Where(x => x.EventType == @event.EventType && x.Provider == data.Provider);

            if (!eventHandlers.Any())
            {
                _logger.LogInformation($"Event type {@event.EventType} received for subject {@event.Subject} - no event handler found for provider {data.Provider}.");
                return;
            }

            foreach (IIntegrationEventHandler eventHandler in eventHandlers)
            {
                await eventHandler.ProcessAsync(@event);
            }
        }
    }
}
