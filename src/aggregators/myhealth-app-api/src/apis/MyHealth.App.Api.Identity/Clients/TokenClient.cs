using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.Extensions.Caching.Memory;
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
        private readonly IMemoryCache _cache;

        public TokenClient(
            HttpClient httpClient,
            IOptions<IdentityApiSettings> identitySettings,
            IOptions<MyHealthAppApiSettings> myHealthSettings,
            IMemoryCache cache)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _identitySettings = identitySettings?.Value ?? throw new ArgumentNullException(nameof(identitySettings));
            _myHealthSettings = myHealthSettings?.Value ?? throw new ArgumentNullException(nameof(myHealthSettings));
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }

        public async Task<string> GetDelegationTokenAsync(string userId, string userToken)
        {
            return await _cache.GetOrCreateAsync(
                $"DelegationToken_{userId}",
                async (cacheEntry) =>
                {
                    TokenResponse response = await _httpClient.RequestTokenAsync(
                        new TokenRequest
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
                        });

                    if (response.IsError)
                    {
                        throw new Exception($"Failed to retrieve access token: {response.Error}");
                    }

                    cacheEntry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(response.ExpiresIn - 30);

                    return response.AccessToken;
                });
        }
    }
}
