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
        private readonly IIntegrationsService _integrationsService;

        public IntegrationsController(IIntegrationsService integrationsService)
        {
            _integrationsService = integrationsService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<SearchIntegrationsResponse>> Search()
        {
            return Ok(await _integrationsService.SearchIntegrationsAsync());
        }

        [HttpGet("{id}", Name = "Get")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Integration>> Get(string id)
        {
            Integration integration = await _integrationsService.GetIntegrationAsync(id);

            if (integration == null)
                return NotFound();

            return Ok(integration);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> Delete(string id)
        {
            await _integrationsService.DeleteIntegrationAsync(id);

            return NoContent();
        }
    }
}
