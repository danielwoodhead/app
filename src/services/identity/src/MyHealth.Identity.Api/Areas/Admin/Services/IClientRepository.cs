using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityServer4.Models;

namespace MyHealth.Identity.Api.Areas.Admin.Services
{
    public interface IClientRepository
    {
        Task CreateClientAsync(Client client);
        Task DeleteClientAsync(string name);
        Task<IEnumerable<Client>> GetAllClientsAsync();
        Task<Client> GetClientAsync(string name);
        Task<Client> UpdateClientAsync(Client client);
    }
}
