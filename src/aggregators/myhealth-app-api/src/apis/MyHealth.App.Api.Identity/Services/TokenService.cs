using System;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using MyHealth.App.Api.Core.Authentication;
using MyHealth.App.Api.Identity.Clients;

namespace MyHealth.App.Api.Identity.Services
{
    public class TokenService : ITokenService
    {
        private readonly IDistributedCache _cache;
        private readonly ITokenClient _identityClient;
        private readonly IUserContext _userContext;
        private readonly ILogger<TokenService> _logger;

        public TokenService(IDistributedCache cache, ITokenClient identityClient, IUserContext userContext, ILogger<TokenService> logger)
        {
            _cache = cache;
            _identityClient = identityClient;
            _userContext = userContext;
            _logger = logger;
        }

        public async Task<string> GetDelegationTokenAsync()
        {
            string userId = _userContext.UserId;
            string cacheKey = CacheKey(userId);
            string cached = await _cache.GetStringAsync(cacheKey);

            if (cached != null)
                return cached;

            string userToken = await _userContext.GetAccessTokenAsync();
            TokenResponse response = await _identityClient.GetDelegationTokenAsync(userToken);

            if (response.IsError)
            {
                _logger.LogError($"Token retrieval failed for user {userId}: {response.Error} - {response.ErrorDescription}");
                return null;
            }

            await _cache.SetStringAsync(
                cacheKey,
                response.AccessToken,
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(response.ExpiresIn) - TimeSpan.FromMinutes(1)
                });

            return response.AccessToken;
        }

        private static string CacheKey(string userId) => $"DelegationToken|{userId}";
    }
}
