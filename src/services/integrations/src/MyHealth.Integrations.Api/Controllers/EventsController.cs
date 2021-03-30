using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.EventGrid;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Extensions.Options;
using MyHealth.Integrations.Api.Configuration;
using MyHealth.Integrations.Api.Extensions;
using MyHealth.Integrations.Core.Events;

namespace MyHealth.Integrations.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [AllowAnonymous]
    [Route("v{version:apiVersion}/events")]
    public class EventsController : ControllerBase
    {
        private readonly IEventHandler _eventHandler;
        private readonly IntegrationsApiSettings _settings;

        public EventsController(IEventHandler eventHandler, IOptions<IntegrationsApiSettings> settings)
        {
            _eventHandler = eventHandler;
            _settings = settings.Value;
        }

        [HttpPost]
        public async Task<IActionResult> HandleEvent([FromQuery]string apiKey)
        {
            if (apiKey != _settings.EventsApiKey)
            {
                return Unauthorized();
            }

            EventGridEvent[] eventGridEvents = await GetEventGridEventsAsync();

            foreach (EventGridEvent eventGridEvent in eventGridEvents)
            {
                if (eventGridEvent.Data is SubscriptionValidationEventData validationEventData)
                {
                    return HandleValidation(validationEventData);
                }
                else
                {
                    await _eventHandler.ProcessAsync(eventGridEvent.ToEvent());
                }
            }

            return new OkResult();
        }

        private async Task<EventGridEvent[]> GetEventGridEventsAsync()
        {
            using var reader = new StreamReader(Request.Body, Encoding.UTF8);
            string body = await reader.ReadToEndAsync();
            var eventGridSubscriber = new EventGridSubscriber();

            return eventGridSubscriber.DeserializeEventGridEvents(body);
        }

        private IActionResult HandleValidation(SubscriptionValidationEventData validationEventData)
        {
            return new OkObjectResult(
                new SubscriptionValidationResponse
                {
                    ValidationResponse = validationEventData.ValidationCode
                });
        }
    }
}
