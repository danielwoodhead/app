using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        public FitbitController(
            IFitbitService fitbitService,
            IUserOperationContext userOperationContext)
        {
            _fitbitService = fitbitService ?? throw new ArgumentNullException(nameof(fitbitService));
            _userOperationContext = userOperationContext ?? throw new ArgumentNullException(nameof(userOperationContext));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult> CreateFitbitIntegration([FromBody] AuthorizationCodeRequest request)
        {
            await _fitbitService.CreateIntegrationAsync(_userOperationContext.UserId, request);

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
