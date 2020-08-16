using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.Extensions.Options;
using MyHealth.App.Api.Core.Settings;
using MyHealth.App.Api.Identity.Settings;

namespace MyHealth.App.Api.Identity.Clients
{
    public class TokenClient : ITokenClient
    {
        private readonly HttpClient _httpClient;
        private readonly IdentityApiSettings _identitySettings;
        private readonly MyHealthAppApiSettings _myHealthSettings;

        public TokenClient(
            HttpClient httpClient,
            IOptions<IdentityApiSettings> identitySettings,
            IOptions<MyHealthAppApiSettings> myHealthSettings)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _identitySettings = identitySettings?.Value ?? throw new ArgumentNullException(nameof(identitySettings));
            _myHealthSettings = myHealthSettings?.Value ?? throw new ArgumentNullException(nameof(myHealthSettings));
        }

        public async Task<TokenResponse> GetDelegationTokenAsync(string userToken)
        {
            using var request = new TokenRequest
            {
                Address = _identitySettings.TokenEndpoint,
                ClientId = _myHealthSettings.ClientId,
                ClientSecret = _myHealthSettings.ClientSecret,
                GrantType = IdentityConstants.DelegationGrant.Type,
                Parameters = new Dictionary<string, string>
                {
                    { IdentityConstants.DelegationGrant.Scopes, _myHealthSettings.ClientScopes },
                    { IdentityConstants.DelegationGrant.Token, userToken }
                }
            };
            return await _httpClient.RequestTokenAsync(request);
        }
    }
}
