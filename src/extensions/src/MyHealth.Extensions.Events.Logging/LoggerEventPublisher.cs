using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MyHealth.Extensions.Logging;

namespace MyHealth.Extensions.Events.Logging
{
    public class LoggerEventPublisher : IEventPublisher
    {
        private readonly ILogger<LoggerEventPublisher> _logger;

        public LoggerEventPublisher(ILogger<LoggerEventPublisher> logger)
        {
            _logger = logger;
        }

        public Task PublishAsync(IEvent @event)
        {
            _logger.Information(@event.EventType, @event.Properties);

            return Task.CompletedTask;
        }

        public async Task PublishAsync(IEnumerable<IEvent> events)
        {
            foreach (IEvent @event in events)
            {
                await PublishAsync(@event).ConfigureAwait(false);
            }
        }
    }
}
