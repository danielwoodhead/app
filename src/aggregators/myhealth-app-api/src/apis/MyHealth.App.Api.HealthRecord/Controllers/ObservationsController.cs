using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyHealth.App.Api.HealthRecord.Models;
using MyHealth.App.Api.HealthRecord.Services;

namespace MyHealth.App.Api.HealthRecord.Controllers
{
    [Route("v{version:apiVersion}/observations")]
    [ApiController]
    [Authorize]
    public class ObservationsController : ControllerBase
    {
        private readonly IHealthRecordService _healthRecordService;

        public ObservationsController(IHealthRecordService healthRecordService)
        {
            _healthRecordService = healthRecordService;
        }

        [HttpGet]
        public async Task<ActionResult<SearchObservationsResponse>> GetObservations()
        {
            SearchObservationsResponse observations = await _healthRecordService.GetObservationsAsync();

            return Ok(observations);
        }
    }
}
