using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MyHealth.Extensions.Events;
using MyHealth.Integrations.Fitbit.Models;
using MyHealth.Integrations.Models.Events;
using MyHealth.Integrations.Utility;

namespace MyHealth.Integrations.Fitbit.Controllers
{
    [Route("api/v{version:apiVersion}/fitbit")]
    [ApiVersion("1.0")]
    [ApiController]
    [Authorize]
    public class FitbitController : ControllerBase
    {
        private const string EventDataVersion = "1.0";

        private readonly IEventPublisher _eventPublisher;
        private readonly IOperationContext _operationContext;
        private readonly FitbitSettings _fitBitSettings;

        public FitbitController(
            IEventPublisher eventPublisher,
            IOperationContext operationContext,
            IOptions<FitbitSettings> fitBitSettings)
        {
            _eventPublisher = eventPublisher ?? throw new ArgumentNullException(nameof(eventPublisher));
            _operationContext = operationContext ?? throw new ArgumentNullException(nameof(operationContext));
            _fitBitSettings = fitBitSettings.Value ?? throw new ArgumentNullException(nameof(fitBitSettings));
        }

        [HttpGet("update")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateNotification([FromQuery] string verify)
        {
            if (string.IsNullOrEmpty(verify) || verify != _fitBitSettings.VerificationCode)
                return NotFound();

            return NoContent();
        }

        [HttpPost("update")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdateNotification([FromBody] IEnumerable<FitbitUpdateNotification> request)
        {
            // TODO: signature verification

            var events = new List<IEvent>();

            foreach (FitbitUpdateNotification update in request)
            {
                events.Add(new IntegrationProviderUpdateEvent(
                    id: Guid.NewGuid().ToString(),
                    subject: update.SubscriptionId,
                    eventTime: DateTime.UtcNow,
                    dataVersion: EventDataVersion,
                    data: CreateEventData(update)));
            }

            await _eventPublisher.PublishAsync(events);

            return NoContent();
        }

        private IntegrationEventData CreateEventData(FitbitUpdateNotification update) =>
            new IntegrationEventData
            {
                OperationId = _operationContext.OperationId,
                SourceSystem = FitbitConstants.Provider,
                SubjectSystem = FitbitConstants.SubscriberSystem,
                Provider = FitbitConstants.Provider,
                UserId = update.SubscriptionId
            };
    }
}
