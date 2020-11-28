using System.Collections.Generic;
using System.Threading.Tasks;
using MyHealth.App.Api.Identity.Models;

namespace MyHealth.App.Api.Identity.Clients
{
    public interface IIdentityClient
    {
        Task<IEnumerable<PersistedGrant>> GetAllPersistedGrantsAsync(string subjectId);
        Task DeletePersistedGrantAsync(string id);
    }
}
