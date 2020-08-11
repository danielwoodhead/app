using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MyHealth.Extensions.Events;
using MyHealth.Integrations.Core.Events.Handlers;
using MyHealth.Integrations.Models.Events;

namespace MyHealth.Integrations.Core.Events
{
    public class EventHandler : IEventHandler
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly Dictionary<string, Type> _eventTypeMapping;
        private readonly ILogger<EventHandler> _logger;

        public EventHandler(IServiceProvider serviceProvider, ILogger<EventHandler> logger)
        {
            _eventTypeMapping = new Dictionary<string, Type>()
            {
                { EventTypes.IntegrationProviderUpdate, typeof(IIntegrationProviderUpdateEventHandler) }
            };

            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public async Task ProcessAsync(IEvent @event)
        {
            if (@event is null)
            {
                throw new ArgumentNullException(nameof(@event));
            }

            if (!_eventTypeMapping.TryGetValue(@event.EventType, out Type eventHandlerType))
            {
                _logger.LogInformation($"Event type {@event.EventType} received for subject {@event.Subject} - no event handler interface defined.");
                return;
            }

            var data = (IntegrationEventData)@event.Data;

            var handlers = ((IEnumerable<IIntegrationEventHandler>)_serviceProvider.GetServices(eventHandlerType)).Where(x => x.Provider == data.Provider);

            if (handlers is null || !handlers.Any())
            {
                _logger.LogInformation($"Event type {@event.EventType} received for subject {@event.Subject} - no event handler found for provider {data.Provider}.");
                return;
            }

            foreach (IIntegrationEventHandler handler in handlers)
            {
                await handler.RunAsync(@event);
            }
        }
    }
}
