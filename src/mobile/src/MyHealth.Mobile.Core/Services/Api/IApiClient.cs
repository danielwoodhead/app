using System.Threading.Tasks;

namespace MyHealth.Mobile.Core.Services.Api
{
    public interface IApiClient
    {
        Task<TResult> GetAsync<TResult>(string uri, string token = "");
        Task<TResult> PostAsync<TRequest, TResult>(string uri, TRequest data, string token = "", string header = "");
        Task<TResult> PutAsync<TResult>(string uri, TResult data, string token = "", string header = "");
        Task DeleteAsync(string uri, string token = "");
    }
}
