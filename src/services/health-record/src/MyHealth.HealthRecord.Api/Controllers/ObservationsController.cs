using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyHealth.HealthRecord.Core;
using MyHealth.HealthRecord.Models.Responses;

namespace MyHealth.HealthRecord.Api.Controllers
{
    [Route("v{version:apiVersion}/observations")]
    [ApiVersion("1.0")]
    [ApiController]
    [Authorize]
    public class ObservationsController : ControllerBase
    {
        private readonly IObservationsService _observationsService;

        public ObservationsController(IObservationsService observationsService)
        {
            _observationsService = observationsService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<SearchObservationsResponse>> Search()
        {
            return Ok(await _observationsService.SearchObservationsAsync());
        }
    }
}
