using System.Net.Http;

namespace MyHealth.Mobile.Core.Services.Http
{
    public interface IHttpClientFactory
    {
        HttpClient Create();
    }
}
