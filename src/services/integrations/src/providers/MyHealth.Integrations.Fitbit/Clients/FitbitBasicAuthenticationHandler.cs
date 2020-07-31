using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace MyHealth.Integrations.Fitbit.Clients
{
    public class FitbitBasicAuthenticationHandler : DelegatingHandler
    {
        private readonly FitbitSettings _settings;

        public FitbitBasicAuthenticationHandler(IOptions<FitbitSettings> settings)
        {
            _settings = settings.Value;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.Authorization = new AuthenticationHeaderValue(
                "Basic",
                Convert.ToBase64String(
                    Encoding.UTF8.GetBytes(
                        $"{_settings.ClientId}:{_settings.ClientSecret}")));

            return base.SendAsync(request, cancellationToken);
        }
    }
}
