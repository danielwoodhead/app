using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using MyHealth.Extensions.AspNetCore.Context;
using MyHealth.Extensions.AspNetCore.Versioning;
using MyHealth.Integrations.Core.Services;
using MyHealth.Integrations.Fitbit.Models;
using MyHealth.Integrations.Fitbit.Services;
using MyHealth.Integrations.Models;

namespace MyHealth.Integrations.Fitbit.Controllers
{
    [ApiController]
    [ApiExplorerSettings(GroupName = "Integrations")]
    [ApiVersion("1.0")]
    [Authorize]
    [Route("v{version:apiVersion}/integrations/fitbit")]
    public class FitbitController : ControllerBase
    {
        private readonly IFitbitService _fitbitService;
        private readonly IIntegrationService _integrationService;
        private readonly IUserOperationContext _userOperationContext;
        private readonly ILogger<FitbitController> _logger;

        public FitbitController(
            IFitbitService fitbitService,
            IIntegrationService integrationService,
            IUserOperationContext userOperationContext,
            ILogger<FitbitController> logger)
        {
            _fitbitService = fitbitService ?? throw new ArgumentNullException(nameof(fitbitService));
            _integrationService = integrationService ?? throw new ArgumentNullException(nameof(integrationService));
            _userOperationContext = userOperationContext ?? throw new ArgumentNullException(nameof(userOperationContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult> CreateFitbitIntegration([FromBody] CreateFitbitIntegrationRequest request, ApiVersion apiVersion)
        {
            Integration integration = await _integrationService.CreateIntegrationAsync(
                new ProviderRequest
                {
                    Provider = Provider.Fitbit,
                    Data = request,
                    UserId = _userOperationContext.UserId
                });

            return CreatedAtRoute(
                "GetIntegration",
                new { id = integration.Id, version = apiVersion.ToUrlString() },
                integration);
        }

        [HttpGet("authenticationUri")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult GetFitbitAuthenticationUri([FromQuery] string redirectUri)
        {
            string authenticationUri = _fitbitService.GetAuthenticationUri(redirectUri);

            return Ok(authenticationUri);
        }

        [HttpGet("update")]
        [AllowAnonymous]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult VerifySubscriptionEndpoint([FromQuery] string verify)
        {
            if (!_fitbitService.VerifySubscriptionEndpoint(verify))
                return NotFound();

            return NoContent();
        }

        [HttpPost("update")]
        [AllowAnonymous]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> UpdateNotification()
        {
            // `[FromBody] string body` doesn't work with System.Text.Json...
            var reader = new StreamReader(Request.Body);
            string request = await reader.ReadToEndAsync();

            _logger.LogInformation(request);

            if (!Request.Headers.TryGetValue(FitbitConstants.RequestSignatureHeader, out StringValues fitbitSignatureHeader)
                || !_fitbitService.VerifyUpdateNotification(request, fitbitSignatureHeader[0]))
            {
                _logger.LogWarning($"Received invalid fitbit update notification: content={request}, signature={fitbitSignatureHeader.FirstOrDefault()}, IP address={HttpContext.Connection.RemoteIpAddress}.");
                return NotFound();
            }

            await _fitbitService.ProcessUpdateNotificationAsync(
                JsonSerializer.Deserialize<IEnumerable<FitbitUpdateNotification>>(request));

            return NoContent();
        }
    }
}
