using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using MyHealth.Web.App.Areas.DataSharing.Models;

namespace MyHealth.Web.App.Areas.DataSharing.Client
{
    public class DataSharingClient : IDataSharingClient
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public DataSharingClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            _jsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        }

        public async Task DeleteDataSharingAgreementAsync(string name)
        {
            await _httpClient.DeleteAsync($"datasharing/agreements/{name}");
        }

        public async Task<IEnumerable<DataSharingAgreement>> GetDataSharingAgreements()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<DataSharingAgreement>>("datasharing/agreements");
        }
    }
}
