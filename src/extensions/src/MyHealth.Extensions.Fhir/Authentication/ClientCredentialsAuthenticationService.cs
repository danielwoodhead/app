using System;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace MyHealth.Extensions.Fhir.Authentication
{
    internal class ClientCredentialsAuthenticationService : IAuthenticationService
    {
        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _cache;
        private readonly FhirClientSettings _fhirClientSettings;

        public ClientCredentialsAuthenticationService(
            HttpClient httpClient,
            IMemoryCache cache,
            IOptions<FhirClientSettings> fhirClientSettings)
        {
            _httpClient = httpClient;
            _cache = cache;
            _fhirClientSettings = fhirClientSettings.Value;
        }

        public async Task<string> GetAccessTokenAsync()
        {
            return await _cache.GetOrCreateAsync(
                $"{GetType().FullName}.AccessToken",
                GetAccessTokenFromAuthServerAsync);
        }

        private async Task<string> GetAccessTokenFromAuthServerAsync(ICacheEntry cacheEntry)
        {
            TokenResponse tokenResponse = await _httpClient.RequestClientCredentialsTokenAsync(
                new ClientCredentialsTokenRequest
                {
                    Address = _fhirClientSettings.AuthenticationTokenEndpoint,
                    ClientId = _fhirClientSettings.AuthenticationClientId,
                    ClientSecret = _fhirClientSettings.AuthenticationClientSecret,
                    Scope = _fhirClientSettings.AuthenticationScope
                });

            if (tokenResponse.IsError)
            {
                throw new Exception($"Failed to retrieve access token: {tokenResponse.Error}");
            }

            cacheEntry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(tokenResponse.ExpiresIn - 30);

            return tokenResponse.AccessToken;
        }
    }
}
