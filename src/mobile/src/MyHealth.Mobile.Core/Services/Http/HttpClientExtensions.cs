using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MyHealth.Mobile.Core.Services.Http
{
    public static class HttpClientExtensions
    {
        public static async Task<TResponse> GetAsync<TResponse>(this HttpClient client, string url)
        {
            HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            return JsonConvert.DeserializeObject<TResponse>(await response.Content.ReadAsStringAsync());
        }

        public static async Task<TResponse> PostAsync<TRequest, TResponse>(this HttpClient client, string url, TRequest request)
        {
            HttpResponseMessage response = await client.PostAsync(url, new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();

            return JsonConvert.DeserializeObject<TResponse>(await response.Content.ReadAsStringAsync());
        }

        public static async Task<TResponse> PutAsync<TRequest, TResponse>(this HttpClient client, string url, TRequest request)
        {
            HttpResponseMessage response = await client.PutAsync(url, new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();

            return JsonConvert.DeserializeObject<TResponse>(await response.Content.ReadAsStringAsync());
        }
    }
}
