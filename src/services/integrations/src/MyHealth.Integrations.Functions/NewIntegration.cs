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
    public class NewIntegration
    {
        private readonly IEventHandler _eventHandler;

        public NewIntegration(IEventHandler eventHandler)
        {
            _eventHandler = eventHandler;
        }

        [FunctionName(nameof(NewIntegration))]
        public async Task Run([EventGridTrigger]EventGridEvent eventGridEvent)
        {
            await _eventHandler.RunAsync(eventGridEvent.ToEvent());
        }
    }
}
