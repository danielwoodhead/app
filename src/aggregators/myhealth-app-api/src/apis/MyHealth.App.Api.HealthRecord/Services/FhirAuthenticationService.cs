using System;
using System.Threading.Tasks;
using IdentityModel.Client;
using MyHealth.App.Api.Identity.Clients;
using MyHealth.Extensions.AspNetCore.Context;
using MyHealth.Extensions.Fhir.Authentication;

namespace MyHealth.App.Api.HealthRecord.Services
{
    public class FhirAuthenticationService : IAuthenticationService
    {
        private readonly ITokenClient _tokenClient;
        private readonly IUserOperationContext _userContext;

        public FhirAuthenticationService(ITokenClient tokenClient, IUserOperationContext userContext)
        {
            _tokenClient = tokenClient;
            _userContext = userContext;
        }

        public async Task<string> GetAccessTokenAsync()
        {
            string token = await _userContext.GetAccessTokenAsync();

            TokenResponse response = await _tokenClient.GetDelegationTokenAsync(token);

            if (response.IsError)
            {
                throw new Exception($"Failed to retrieve access token: {response.Error}");
            }

            return response.AccessToken;
        }
    }
}
