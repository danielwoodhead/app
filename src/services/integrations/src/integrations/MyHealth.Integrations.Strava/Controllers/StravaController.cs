using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        public StravaController(
            IIntegrationService integrationService,
            IStravaService stravaService,
            IUserOperationContext operationContext)
        {
            _integrationService = integrationService;
            _stravaService = stravaService;
            _operationContext = operationContext;
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
    }
}
