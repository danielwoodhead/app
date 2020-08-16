using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace MyHealth.App.Api.Core.Authentication
{
    public class DelegationAuthenticationHandler : DelegatingHandler
    {
        private readonly ITokenService _tokenService;

        public DelegationAuthenticationHandler(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            string delegationToken = await _tokenService.GetDelegationTokenAsync();

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", delegationToken);

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
