using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MyHealth.App.Api.Core.Http
{
    public static class HttpClientExtensions
    {
        public static async Task<HttpResponseMessage> SendAsync(this HttpClient client, HttpMethod method, string url, object body = null)
        {
            var request = new HttpRequestMessage(method, url);

            if (body != null)
                request.Content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, MediaTypeNames.Application.Json);

            return await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
        }
    }
}
