using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using MyHealth.App.Api.Identity.Models;

namespace MyHealth.App.Api.Identity.Clients
{
    public class IdentityClient : IIdentityClient
    {
        private readonly HttpClient _httpClient;

        public IdentityClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task DeletePersistedGrantAsync(string id)
        {
            await _httpClient.DeleteAsync($"PersistedGrants/{id}");
        }

        public async Task<IEnumerable<PersistedGrant>> GetAllPersistedGrantsAsync(string subjectId)
        {
            var persistedGrants = new List<PersistedGrant>();

            PersistedGrantsResponse response = await GetPersistedGrantsAsync(
                subjectId,
                page: 1,
                pageSize: 10);

            persistedGrants.AddRange(response.PersistedGrants);

            int totalCount = response.TotalCount;
            int pageSize = response.PageSize;
            int callCount = (totalCount + pageSize - 1) / pageSize;

            for (int i = 1; i < callCount; i++)
            {
                response = await GetPersistedGrantsAsync(
                    subjectId,
                    page: i + 1,
                    pageSize: 10);

                persistedGrants.AddRange(response.PersistedGrants);
            }

            return persistedGrants;
        }

        private async Task<PersistedGrantsResponse> GetPersistedGrantsAsync(string subjectId, int page, int pageSize)
        {
            return await _httpClient.GetFromJsonAsync<PersistedGrantsResponse>(
                QueryHelpers.AddQueryString(
                    $"PersistedGrants/Subjects/{subjectId}",
                    new Dictionary<string, string>
                    {
                        { "page", page.ToString() },
                        { "pageSize", pageSize.ToString() }
                    }));
        }
    }
}
