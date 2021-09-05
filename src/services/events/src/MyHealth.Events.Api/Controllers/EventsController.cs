using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CloudNative.CloudEvents;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyHealth.Events.EventIngestion.Services;

namespace MyHealth.Events.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize]
    [Route("v{version:apiVersion}/events")]
    public class EventsController : ControllerBase
    {
        private readonly IEventService _eventService;

        public EventsController(IEventService eventService)
        {
            _eventService = eventService;
        }

        [HttpPost]
        [Consumes("application/cloudevents+json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostEvent([FromBody] CloudEvent @event)
        {
            string[] validationErrors = ValidateCloudEvent(@event);

            if (validationErrors.Any())
            {
                return ValidationProblem(validationErrors);
            }

            await _eventService.HandleEventAsync(@event);

            return NoContent();
        }

        private static string[] ValidateCloudEvent(CloudEvent @event)
        {
            if (@event is null)
            {
                return new[] { "Invalid event" };
            }

            try
            {
                @event.Validate();
            }
            catch (InvalidOperationException ex)
            {
                return new[] { ex.Message };
            }

            return Array.Empty<string>();
        }

        private ActionResult ValidationProblem(string[] validationErrors)
        {
            return ValidationProblem(
                new ValidationProblemDetails(
                    new Dictionary<string, string[]>
                    {
                        { "validation", validationErrors }
                    }));
        }
    }
}
