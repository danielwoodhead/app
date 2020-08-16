﻿using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyHealth.App.Api.Core.Http;
using MyHealth.App.Api.Integrations.Clients;
using MyHealth.App.Api.Integrations.Models;

namespace MyHealth.App.Api.Integrations.Controllers
{
    [Route("v{version:apiVersion}/integrations")]
    [ApiController]
    public class IntegrationsController : ControllerBase
    {
        private readonly IIntegrationsClient _client;

        public IntegrationsController(IIntegrationsClient client)
        {
            _client = client;
        }

        [HttpGet]
        public async Task<ActionResult<SearchIntegrationsResponse>> Search()
        {
            using HttpResponseMessage response = await _client.SearchIntegrationsAsync();

            return await response.ToResultAsync<SearchIntegrationsResponse>();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Integration>> Get(string id)
        {
            using HttpResponseMessage response = await _client.GetIntegrationAsync(id);

            return await response.ToResultAsync<Integration>();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            using HttpResponseMessage response = await _client.DeleteIntegrationAsync(id);

            return response.ToResult();
        }

        [HttpPost]
        public async Task<ActionResult> CreateFitbitIntegration([FromBody] CreateFitbitIntegrationRequest request)
        {
            using HttpResponseMessage response = await _client.CreateFitbitIntegrationAsync(request);

            return response.ToResult();
        }
    }
}
