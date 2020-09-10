using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyHealth.App.Api.Core.Http;
using MyHealth.App.Api.HealthRecord.Clients;
using MyHealth.App.Api.HealthRecord.Models;

namespace MyHealth.App.Api.HealthRecord.Controllers
{
    [Route("v{version:apiVersion}/observations")]
    [ApiController]
    [Authorize]
    public class ObservationsController : ControllerBase
    {
        private readonly IHealthRecordClient _client;

        public ObservationsController(IHealthRecordClient client)
        {
            _client = client;
        }

        [HttpGet]
        public async Task<ActionResult<SearchObservationsResponse>> GetObservations()
        {
            using HttpResponseMessage response = await _client.GetObservationsAsync();

            return await response.ToResultAsync<SearchObservationsResponse>();
        }
    }
}
