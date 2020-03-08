using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyHealth.Identity.Api.Areas.Admin.Services;

namespace MyHealth.Identity.Api.Areas.Admin.Pages.Clients
{
    public class IndexModel : PageModel
    {
        private readonly IClientRepository _repository;

        public IndexModel(IClientRepository repository)
        {
            _repository = repository;
        }

        public IList<Client> Clients { get; private set; }

        public async Task OnGetAsync()
        {
            Clients = (await _repository.GetAllClientsAsync()).ToList();
        }
    }
}
