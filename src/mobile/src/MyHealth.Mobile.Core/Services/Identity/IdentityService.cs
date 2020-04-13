using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using IdentityModel;
using IdentityModel.Client;
using MyHealth.Extensions.Cryptography;
using MyHealth.Mobile.Core.Extensions;
using MyHealth.Mobile.Core.Models.Identity;
using MyHealth.Mobile.Core.Services.Http;

namespace MyHealth.Mobile.Core.Services.Identity
{
    public class IdentityService : IIdentityService
    {
        private readonly HttpClient _httpClient;
        private string _codeVerifier;

        public IdentityService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.Create();
        }

        public string CreateAuthorizationRequest()
        {
            var requestParams = new Dictionary<string, string>
            {
                { "client_id", GlobalSettings.AuthClientId },
                { "client_secret", GlobalSettings.AuthClientSecret },
                { "response_type", "code" },
                { "scope", "openid profile observations-api fhir-api offline_access" },
                { "redirect_uri", GlobalSettings.AuthRedirectUri },
                { "nonce", Guid.NewGuid().ToString("N") },
                { "code_challenge", CreateCodeChallenge() },
                { "code_challenge_method", "S256" },
                { "state", Guid.NewGuid().ToString("N") }
            };

            var queryString = string.Join("&", requestParams
                .Select(kvp => string.Format("{0}={1}", WebUtility.UrlEncode(kvp.Key), WebUtility.UrlEncode(kvp.Value)))
                .ToArray());

            return string.Format("{0}?{1}", GlobalSettings.AuthAuthorizeEndpoint, queryString);
        }

        public string CreateLogoutRequest(string token)
        {
            return string.Format("{0}?id_token_hint={1}&post_logout_redirect_uri={2}",
                GlobalSettings.AuthLogoutEndpoint,
                token,
                GlobalSettings.AuthLogoutCallback);
        }

        public async Task<UserToken> GetTokenAsync(string code)
        {
            var tokenResponse = await _httpClient.RequestAuthorizationCodeTokenAsync(
                new AuthorizationCodeTokenRequest
                {
                    Address = GlobalSettings.AuthTokenEndpoint,
                    ClientId = GlobalSettings.AuthClientId,
                    ClientSecret = GlobalSettings.AuthClientSecret,
                    Code = code,
                    RedirectUri = GlobalSettings.AuthRedirectUri,
                    CodeVerifier = _codeVerifier,
                });

            if (tokenResponse.IsError)
                throw new Exception($"Failed to retrieve token: {tokenResponse.ErrorDescription}");

            return tokenResponse.ToUserToken();
        }

        private string CreateCodeChallenge()
        {
            _codeVerifier = GenerateRandomNumber();
            var codeVerifierBytes = Encoding.ASCII.GetBytes(_codeVerifier);
            var hashedBytes = codeVerifierBytes.Sha256();
            return Base64Url.Encode(hashedBytes);
        }

        private static string GenerateRandomNumber()
        {
            byte[] random = new byte[64];
            var rng = new RNGCryptoServiceProvider();
            rng.GetBytes(random);
            return ByteArrayToString(random);
        }

        private static string ByteArrayToString(byte[] array)
        {
            var hex = new StringBuilder(array.Length * 2);
            foreach (byte b in array)
            {
                hex.AppendFormat("{0:x2}", b);
            }
            return hex.ToString();
        }
    }
}
