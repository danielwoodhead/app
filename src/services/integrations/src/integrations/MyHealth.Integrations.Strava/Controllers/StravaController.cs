using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyHealth.Extensions.AspNetCore.Context;
using MyHealth.Extensions.AspNetCore.Versioning;
using MyHealth.Integrations.Core.Services;
using MyHealth.Integrations.Models;
using MyHealth.Integrations.Strava.Models;
using MyHealth.Integrations.Strava.Services;

namespace MyHealth.Integrations.Strava.Controllers
{
    [ApiController]
    [ApiExplorerSettings(GroupName = "Integrations")]
    [ApiVersion("1.0")]
    [Authorize]
    [Route("v{version:apiVersion}/integrations/strava")]
    public class StravaController : ControllerBase
    {
        private readonly IIntegrationService _integrationService;
        private readonly IStravaService _stravaService;
        private readonly IUserOperationContext _operationContext;
        private readonly ILogger<StravaController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public StravaController(
            IIntegrationService integrationService,
            IStravaService stravaService,
            IUserOperationContext operationContext,
            ILogger<StravaController> logger,
            IHttpContextAccessor httpContextAccessor)
        {
            _integrationService = integrationService;
            _stravaService = stravaService;
            _operationContext = operationContext;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult> CreateStravaIntegration([FromBody] CreateStravaIntegrationRequest request, ApiVersion apiVersion)
        {
            Integration integration = await _integrationService.CreateIntegrationAsync(
                new ProviderRequest
                {
                    Provider = Provider.Strava,
                    Data = request,
                    UserId = _operationContext.UserId
                });

            return CreatedAtRoute(
                "GetIntegration",
                new { id = integration.Id, version = apiVersion.ToUrlString() },
                integration);
        }

        [HttpGet("authenticationUri")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult GetStravaAuthenticationUri([FromQuery] string redirectUri)
        {
            string authenticationUri = _stravaService.GetAuthenticationUri(redirectUri);

            return Ok(authenticationUri);
        }

        [HttpPost("subscription")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult> CreateStravaSubscription(ApiVersion apiVersion)
        {
            string hostAddress = _httpContextAccessor.HttpContext.Request.Host.Value;

            StravaSubscription subscription = await _stravaService.CreateSubscriptionAsync(
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
            await _stravaService.DeleteSubscriptionAsync();

            return NoContent();
        }

        [HttpGet("subscription", Name = "GetStravaSubscription")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<StravaSubscription>> GetStravaSubscription()
        {
            var subscription = await _stravaService.GetSubscriptionAsync();

            if (subscription is null)
                return NotFound();

            return subscription;
        }

        [HttpGet("update")]
        [AllowAnonymous]
        [ApiExplorerSettings(IgnoreApi = true)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<ValidateStravaSubscriptionResponse> ValidateStravaSubscription(
            [FromQuery(Name = "hub.mode")] string mode,
            [FromQuery(Name = "hub.challenge")] string challenge,
            [FromQuery(Name = "hub.verify_token")] string verifyToken)
        {
            bool isValid = _stravaService.ValidateSubscription(verifyToken);

            if (isValid)
            {
                return Ok(new ValidateStravaSubscriptionResponse
                {
                    Challenge = challenge
                });
            }

            _logger.LogInformation($"Strava subscription validation failed mode={mode}, challenge={challenge}, verify_token={verifyToken}");

            return Unauthorized();
        }

        [HttpPost("update")]
        [AllowAnonymous]
        [ApiExplorerSettings(IgnoreApi = true)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ValidateStravaSubscriptionResponse>> StravaSubscriptionUpdate()
        {
            var reader = new StreamReader(Request.Body);
            string request = await reader.ReadToEndAsync();

            _logger.LogInformation(request);

            await _stravaService.ProcessUpdateNotification(
                JsonSerializer.Deserialize<StravaUpdateNotification>(request));

            return Ok();
        }
    }
}
