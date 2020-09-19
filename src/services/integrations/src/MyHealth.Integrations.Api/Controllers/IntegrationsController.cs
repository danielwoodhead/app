using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyHealth.Integrations.Core.Services;
using MyHealth.Integrations.Models;
using MyHealth.Integrations.Models.Response;

namespace MyHealth.Integrations.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize]
    [Route("v{version:apiVersion}/integrations")]
    public class IntegrationsController : ControllerBase
    {
        private readonly IIntegrationService _integrationService;

        public IntegrationsController(IIntegrationService integrationService)
        {
            _integrationService = integrationService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<GetIntegrationsResponse>> GetIntegrations()
        {
            return Ok(await _integrationService.GetIntegrationsAsync());
        }

        [HttpGet("{id}", Name = "GetIntegration")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Integration>> GetIntegration(string id)
        {
            Integration integration = await _integrationService.GetIntegrationAsync(id);

            if (integration == null)
                return NotFound();

            return Ok(integration);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> DeleteIntegration(string id)
        {
            await _integrationService.DeleteIntegrationAsync(id);

            return NoContent();
        }
    }
}
