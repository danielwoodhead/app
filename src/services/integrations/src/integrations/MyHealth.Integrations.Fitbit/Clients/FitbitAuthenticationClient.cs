using System;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.Extensions.Options;

namespace MyHealth.Integrations.Fitbit.Clients
{
    public class FitbitAuthenticationClient : IFitbitAuthenticationClient
    {
        private readonly HttpClient _httpClient;
        private readonly FitbitSettings _fitbitSettings;

        public FitbitAuthenticationClient(HttpClient httpClient, IOptions<FitbitSettings> fitbitSettings)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _fitbitSettings = fitbitSettings?.Value ?? throw new ArgumentNullException(nameof(fitbitSettings));
        }

        public async Task<TokenResponse> AuthenticateAsync(string code, Uri redirectUri)
        {
            return await _httpClient.RequestAuthorizationCodeTokenAsync(
                new AuthorizationCodeTokenRequest
                {
                    Address = GetTokenEndpointAddress(),
                    ClientId = _fitbitSettings.ClientId,
                    RedirectUri = redirectUri.AbsoluteUri,
                    Code = code
                });
        }

        public async Task<TokenResponse> RefreshTokenAsync(string refreshToken)
        {
            return await _httpClient.RequestRefreshTokenAsync(
                new RefreshTokenRequest
                {
                    Address = GetTokenEndpointAddress(),
                    RefreshToken = refreshToken
                });
        }

        private string GetTokenEndpointAddress() => _httpClient.BaseAddress.AbsoluteUri + "oauth2/token";
    }
}
