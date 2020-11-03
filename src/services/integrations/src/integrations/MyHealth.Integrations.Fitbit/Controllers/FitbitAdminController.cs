using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyHealth.Integrations.Core.Events;
using MyHealth.Integrations.Fitbit.Models;
using MyHealth.Integrations.Models;
using MyHealth.Integrations.Models.Events;
using Newtonsoft.Json.Linq;

namespace MyHealth.Integrations.Fitbit.Controllers
{
    [ApiController]
    [ApiExplorerSettings(GroupName = "Admin")]
    [ApiVersion("1.0")]
    [AllowAnonymous]
    [Route("v{version:apiVersion}/admin/fitbit")]
    public class FitbitAdminController : ControllerBase
    {
        private readonly IEventHandler _eventHandler;

        public FitbitAdminController(IEventHandler eventHandler)
        {
            _eventHandler = eventHandler;
        }

        [HttpPost("update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> TestFitbitUpdate([FromBody] FitbitUpdateNotification update, [FromQuery] string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return BadRequest();

            await _eventHandler.ProcessAsync(
                new IntegrationProviderUpdateEvent(
                    id: Guid.NewGuid().ToString(),
                    subject: update.SubscriptionId,
                    eventTime: DateTime.UtcNow,
                    dataVersion: EventConstants.EventDataVersion,
                    data: new IntegrationProviderEventData
                    {
                        OperationId = "test",
                        SourceSystem = EventConstants.IntegrationsApi,
                        SubjectSystem = EventConstants.MyHealth,
                        Provider = Provider.Fitbit,
                        ProviderData = JObject.FromObject(update),
                        UserId = userId
                    }));

            return Ok();
        }
    }
}
