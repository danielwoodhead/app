using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyHealth.Integrations.Fitbit.Models;
using MyHealth.Integrations.Fitbit.Services;
using MyHealth.Integrations.Models.Requests;

namespace MyHealth.Integrations.Fitbit.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize]
    [Route("v{version:apiVersion}/integrations/fitbit")]
    public class FitbitController : ControllerBase
    {
        private readonly IFitbitService _fitbitService;

        public FitbitController(IFitbitService fitbitService)
        {
            _fitbitService = fitbitService ?? throw new ArgumentNullException(nameof(fitbitService));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult> CreateFitbitIntegration([FromBody] AuthorizationCodeRequest request)
        {
            await _fitbitService.CreateIntegrationAsync(request.Code);

            return Created("", ""); // TODO
        }

        [HttpGet("update")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult VerifySubscriptionEndpoint([FromQuery] string verificationCode)
        {
            if (_fitbitService.Verify(verificationCode))
                return NoContent();
            else
                return NotFound();
        }

        [HttpPost("update")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> UpdateNotification([FromBody] IEnumerable<FitbitUpdateNotification> request)
        {
            await _fitbitService.ProcessUpdateNotificationAsync(request);

            return NoContent();
        }
    }
}
