using System.Threading.Tasks;
using MyHealth.App.Api.Core.Authentication;
using MyHealth.Extensions.Fhir.Authentication;

namespace MyHealth.App.Api.HealthRecord.Services
{
    public class FhirAuthenticationService : IAuthenticationService
    {
        private readonly ITokenService _tokenService;

        public FhirAuthenticationService(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        public async Task<string> GetAccessTokenAsync()
        {
            return await _tokenService.GetDelegationTokenAsync();
        }
    }
}
