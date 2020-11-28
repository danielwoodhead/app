using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyHealth.App.Api.Core.Authentication;
using MyHealth.App.Api.Identity.Clients;
using MyHealth.App.Api.Identity.Models;

namespace MyHealth.App.Api.Identity.Controllers
{
    [Route("v{version:apiVersion}/datasharing")]
    [ApiController]
    [Authorize]
    public class DataSharingController : ControllerBase
    {
        private readonly IIdentityClient _identityClient;
        private readonly IUserContext _userContext;

        public DataSharingController(IIdentityClient identityClient, IUserContext userContext)
        {
            _identityClient = identityClient;
            _userContext = userContext;
        }

        [HttpGet("agreements")]
        public async Task<ActionResult<IEnumerable<DataSharingAgreement>>> GetDataSharingAgreements()
        {
            IEnumerable<PersistedGrant> persistedGrants = await _identityClient.GetAllPersistedGrantsAsync(_userContext.UserId);

            return Ok(persistedGrants.Where(x => x.Type == IdentityConstants.PersistedGrant.RefreshToken)
                .Select(x => x.ClientId)
                .Distinct()
                .Select(x => new DataSharingAgreement { Name = x }));
        }

        [HttpDelete("agreements/{name}")]
        public async Task<IActionResult> DeleteDataSharingAgreement(string name)
        {
            IEnumerable<PersistedGrant> persistedGrants = await _identityClient.GetAllPersistedGrantsAsync(_userContext.UserId);

            foreach (var persistedGrant in persistedGrants.Where(x => x.ClientId == name))
            {
                await _identityClient.DeletePersistedGrantAsync(persistedGrant.Key);
            }

            return NoContent();
        }
    }
}
