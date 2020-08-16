using Microsoft.AspNetCore.Mvc;

namespace MyHealth.App.Api.HealthRecord.Controllers
{
    [Route("v{version:apiVersion}/observations")]
    [ApiController]
    public class ObservationsController : ControllerBase
    {
    }
}
