using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using MyHealth.Extensions.AspNetCore.Context;
using MyHealth.Integrations.Fitbit.Services;

namespace MyHealth.Integrations.Fitbit.Clients
{
    public class FitbitBearerAuthenticationHandler : DelegatingHandler
    {
        private readonly IFitbitTokenService _fitbitTokenService;
        private readonly IUserOperationContext _userContext;

        public FitbitBearerAuthenticationHandler(
            IFitbitTokenService fitbitTokenService,
            IUserOperationContext userContext)
        {
            _fitbitTokenService = fitbitTokenService;
            _userContext = userContext;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request.Headers.Authorization == null)
            {
                string accessToken = await _fitbitTokenService.GetAccessTokenAsync(_userContext.UserId);

                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
