// Default URL for triggering event grid function in the local environment.
// http://localhost:7071/runtime/webhooks/EventGrid?functionName={functionname}
using System.Threading.Tasks;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using MyHealth.Integrations.Core.Events;
using MyHealth.Integrations.FunctionApp.Extensions;

namespace MyHealth.Integrations.FunctionApp.Functions
{
    public class IntegrationEventProcessor
    {
        private readonly IEventHandler _eventHandler;

        public IntegrationEventProcessor(IEventHandler eventHandler)
        {
            _eventHandler = eventHandler;
        }

        [FunctionName(nameof(IntegrationEventProcessor))]
        public async Task Run([EventGridTrigger]EventGridEvent eventGridEvent)
        {
            await _eventHandler.ProcessAsync(eventGridEvent.ToEvent());
        }
    }
}
