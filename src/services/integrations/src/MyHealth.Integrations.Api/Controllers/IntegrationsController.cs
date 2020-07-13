using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyHealth.Extensions.AspNetCore.Versioning;
using MyHealth.Integrations.Core.Services;
using MyHealth.Integrations.Models;
using MyHealth.Integrations.Models.Requests;
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

        // GET: api/integrations
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<SearchIntegrationsResponse>> Search()
        {
            return Ok(await _integrationsService.SearchIntegrationsAsync());
        }

        // GET: api/integrations/5
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

        // POST: api/integrations
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<Integration>> Post([FromBody] CreateIntegrationRequest request, ApiVersion apiVersion)
        {
            Integration integration = await _integrationsService.CreateIntegrationAsync(request);

            return CreatedAtRoute("Get", new { id = integration.Id, version = apiVersion.ToUrlString() }, integration);
        }

        // PUT: api/integrations/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Integration>> Put(string id, [FromBody] UpdateIntegrationRequest request)
        {
            return Ok(await _integrationsService.UpdateIntegrationAsync(id, request));
        }

        // DELETE: api/integrations/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> Delete(string id)
        {
            await _integrationsService.DeleteIntegrationAsync(id);

            return NoContent();
        }
    }
}
