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
        private readonly IStravaAuthenticationService _stravaAuthenticationService;
        private readonly IStravaSubscriptionService _stravaSubscriptionService;
        private readonly IStravaUpdateService _stravaUpdateService;
        private readonly IUserOperationContext _operationContext;
        private readonly ILogger<StravaController> _logger;

        public StravaController(
            IIntegrationService integrationService,
            IStravaAuthenticationService stravaAuthenticationService,
            IStravaSubscriptionService stravaSubscriptionService,
            IStravaUpdateService stravaUpdateService,
            IUserOperationContext operationContext,
            ILogger<StravaController> logger)
        {
            _integrationService = integrationService;
            _stravaAuthenticationService = stravaAuthenticationService;
            _stravaSubscriptionService = stravaSubscriptionService;
            _stravaUpdateService = stravaUpdateService;
            _operationContext = operationContext;
            _logger = logger;
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
            string authenticationUri = _stravaAuthenticationService.GetAuthenticationUri(redirectUri);

            return Ok(authenticationUri);
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
            bool isValid = _stravaSubscriptionService.ValidateSubscription(verifyToken);

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

            await _stravaUpdateService.ProcessUpdateNotification(
                JsonSerializer.Deserialize<StravaUpdateNotification>(request));

            return Ok();
        }
    }
}
