using System;
using System.Net.Http;
using TinyIoC;

namespace MyHealth.Mobile.Core.Services.Http
{
    public class HttpClientFactory : IHttpClientFactory
    {
        private readonly Lazy<HttpClient> _httpClient = new Lazy<HttpClient>(() => new HttpClient(TinyIoCContainer.Current.Resolve<UserAccessTokenHandler>()));

        public HttpClient Create()
        {
            return _httpClient.Value;
        }
    }
}
