using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityServer4.Models;

namespace MyHealth.Identity.Api.Areas.Admin.Services
{
    public interface IApiResourceRepository
    {
        Task CreateApiResourceAsync(ApiResource apiResource);
        Task DeleteApiResourceAsync(string name);
        Task<IEnumerable<ApiResource>> GetAllApiResourcesAsync();
        Task<ApiResource> GetApiResourceAsync(string name);
        Task<ApiResource> UpdateApiResourceAsync(ApiResource apiResource);
    }
}
