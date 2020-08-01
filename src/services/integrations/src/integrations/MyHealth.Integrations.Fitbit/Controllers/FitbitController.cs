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
using MyHealth.Extensions.AspNetCore.Versioning;
using MyHealth.Integrations.Fitbit.Models;
using MyHealth.Integrations.Fitbit.Services;
using MyHealth.Integrations.Models.Requests;
using MyHealth.Integrations.Utility;

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
        private readonly IUserOperationContext _userOperationContext;
        private readonly ILogger<FitbitController> _logger;

        public FitbitController(
            IFitbitService fitbitService,
            IUserOperationContext userOperationContext,
            ILogger<FitbitController> logger)
        {
            _fitbitService = fitbitService ?? throw new ArgumentNullException(nameof(fitbitService));
            _userOperationContext = userOperationContext ?? throw new ArgumentNullException(nameof(userOperationContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult> CreateFitbitIntegration([FromBody] AuthorizationCodeRequest request, ApiVersion apiVersion)
        {
            var integration = await _fitbitService.CreateIntegrationAsync(_userOperationContext.UserId, request);

            return CreatedAtRoute(
                "GetIntegration",
                new { id = integration.Id, version = apiVersion.ToUrlString() },
                integration);
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
