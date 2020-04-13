using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using MyHealth.Mobile.Core.Services.Settings;

namespace MyHealth.Mobile.Core.Services.Http
{
    public class UserAccessTokenHandler : DelegatingHandler
    {
        private readonly ISettingsService _settingsService;

        public UserAccessTokenHandler(ISettingsService settingsService)
        {
            _settingsService = settingsService;
            InnerHandler = new HttpClientHandler();
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrEmpty(_settingsService.AuthAccessToken))
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _settingsService.AuthAccessToken);

            return base.SendAsync(request, cancellationToken);
        }
    }
}
