using System;
using System.Threading.Tasks;
using IdentityModel.Client;
using MyHealth.Integrations.Fitbit.Clients;

namespace MyHealth.Integrations.Tests.Mocks
{
    public class MockFitbitAuthenticationClient : IFitbitAuthenticationClient
    {
        public Task<TokenResponse> AuthenticateAsync(string code, Uri redirectUri) => throw new NotImplementedException();
        public Task<TokenResponse> RefreshTokenAsync(string refreshToken) => throw new NotImplementedException();
    }
}
