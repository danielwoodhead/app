// Default URL for triggering event grid function in the local environment.
// http://localhost:7071/runtime/webhooks/EventGrid?functionName={functionname}
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using MyHealth.Integrations.Core.Events;
using System.Threading.Tasks;
using MyHealth.Integrations.Functions.Extensions;

namespace MyHealth.Integrations.Functions
{
    public class IntegrationProviderUpdate
    {
        private readonly IEventHandler _eventHandler;

        public IntegrationProviderUpdate(IEventHandler eventHandler)
        {
            _eventHandler = eventHandler;
        }

        [FunctionName(nameof(IntegrationProviderUpdate))]
        public async Task Run([EventGridTrigger]EventGridEvent eventGridEvent)
        {
            await _eventHandler.ProcessAsync(eventGridEvent.ToEvent());
        }
    }
}
