using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyHealth.Extensions.AspNetCore.Versioning;
using MyHealth.Integrations.Core.Events;
using MyHealth.Integrations.Models;
using MyHealth.Integrations.Models.Events;
using MyHealth.Integrations.Strava.Clients.Models;
using MyHealth.Integrations.Strava.Models;
using MyHealth.Integrations.Strava.Services;
using Newtonsoft.Json.Linq;

namespace MyHealth.Integrations.Strava.Controllers
{
    [ApiController]
    [ApiExplorerSettings(GroupName = "Admin")]
    [ApiVersion("1.0")]
    [AllowAnonymous]
    [Route("v{version:apiVersion}/admin/strava")]
    public class StravaAdminController : ControllerBase
    {
        private readonly IStravaSubscriptionService _stravaSubscriptionService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEventHandler _eventHandler;
        private readonly ILogger<StravaAdminController> _logger;

        public StravaAdminController(
            IStravaSubscriptionService stravaSubscriptionService,
            IHttpContextAccessor httpContextAccessor,
            IEventHandler eventHandler,
            ILogger<StravaAdminController> logger)
        {
            _stravaSubscriptionService = stravaSubscriptionService;
            _httpContextAccessor = httpContextAccessor;
            _eventHandler = eventHandler;
            _logger = logger;
        }

        [HttpPost("subscription")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult> CreateStravaSubscription(ApiVersion apiVersion)
        {
            string hostAddress = _httpContextAccessor.HttpContext.Request.Host.Value;

            StravaSubscription subscription = await _stravaSubscriptionService.CreateSubscriptionAsync(
                $"https://{hostAddress}/{apiVersion.ToUrlString()}/integrations/strava/update");

            return CreatedAtRoute(
                "GetStravaSubscription",
                new { version = apiVersion.ToUrlString() },
                subscription);
        }

        [HttpDelete("subscription")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> DeleteStravaSubscription()
        {
            await _stravaSubscriptionService.DeleteSubscriptionAsync();

            return NoContent();
        }

        [HttpGet("subscription", Name = "GetStravaSubscription")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<StravaSubscription>> GetStravaSubscription()
        {
            StravaSubscription subscription = await _stravaSubscriptionService.GetSubscriptionAsync();

            if (subscription is null)
                return NotFound();

            return subscription;
        }

        [HttpPost("update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> TestStravaUpdate([FromBody]StravaUpdateNotification update, [FromQuery]string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return BadRequest();

            _logger.LogInformation($"TestStravaUpdate: {JsonSerializer.Serialize(update)}");

            await _eventHandler.ProcessAsync(
                new IntegrationProviderUpdateEvent(
                    id: Guid.NewGuid().ToString(),
                    subject: update.OwnerId.ToString(),
                    eventTime: DateTime.UtcNow,
                    dataVersion: EventConstants.EventDataVersion,
                    data: new IntegrationProviderEventData
                    {
                        OperationId = "test",
                        SourceSystem = EventConstants.IntegrationsApi,
                        SubjectSystem = Provider.Strava.ToString(),
                        Provider = Provider.Strava,
                        ProviderData = JObject.FromObject(update),
                        UserId = userId
                    }));

            return Ok();
        }
    }
}
