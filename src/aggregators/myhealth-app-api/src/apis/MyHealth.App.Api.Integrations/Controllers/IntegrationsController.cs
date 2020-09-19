using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyHealth.App.Api.Core.Http;
using MyHealth.App.Api.Integrations.Clients;
using MyHealth.App.Api.Integrations.Models;

namespace MyHealth.App.Api.Integrations.Controllers
{
    [Route("v{version:apiVersion}/integrations")]
    [ApiController]
    [Authorize]
    public class IntegrationsController : ControllerBase
    {
        private readonly IIntegrationsClient _client;

        public IntegrationsController(IIntegrationsClient client)
        {
            _client = client;
        }

        [HttpGet]
        public async Task<ActionResult<GetIntegrationsResponse>> GetIntegrations()
        {
            using HttpResponseMessage response = await _client.GetIntegrationsAsync();

            return await response.ToResultAsync<GetIntegrationsResponse>();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Integration>> GetIntegration(string id)
        {
            using HttpResponseMessage response = await _client.GetIntegrationAsync(id);

            return await response.ToResultAsync<Integration>();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteIntegration(string id)
        {
            using HttpResponseMessage response = await _client.DeleteIntegrationAsync(id);

            return response.ToResult();
        }

        [HttpPost("fitbit")]
        public async Task<ActionResult> CreateFitbitIntegration([FromBody] CreateFitbitIntegrationRequest request)
        {
            using HttpResponseMessage response = await _client.CreateFitbitIntegrationAsync(request);

            return response.ToResult();
        }

        [HttpGet("fitbit/authenticationUri")]
        public async Task<ActionResult> GetFitbitAuthenticationUri(string redirectUri)
        {
            using HttpResponseMessage response = await _client.GetFitbitAuthenticationUri(redirectUri);

            return await response.ToStringResultAsync();
        }
    }
}
