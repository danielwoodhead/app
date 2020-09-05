using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MyHealth.App.Api.Core.Http
{
    public static class HttpResponseMessageExtensions
    {
        public static async Task<ActionResult> ToResultAsync<T>(this HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                return new ObjectResult(await response.Content.ReadFromJsonAsync<T>())
                {
                    StatusCode = (int)response.StatusCode
                };
            }
            else
            {
                return new ObjectResult(await response.Content.ReadAsStringAsync())
                {
                    StatusCode = (int)response.StatusCode
                };
            }
        }

        public static async Task<ActionResult> ToStringResultAsync(this HttpResponseMessage response)
        {
            return new ObjectResult(await response.Content.ReadAsStringAsync())
            {
                StatusCode = (int)response.StatusCode
            };
        }

        public static ActionResult ToResult(this HttpResponseMessage response)
        {
            return new StatusCodeResult((int)response.StatusCode);
        }
    }
}
