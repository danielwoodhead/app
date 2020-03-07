using System;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyHealth.Identity.Api.Areas.Admin.Services;

namespace MyHealth.Identity.Api.Areas.Admin.Pages.Clients
{
    public class DetailsModel : PageModel
    {
        private readonly IClientRepository _repository;

        public DetailsModel(IClientRepository repository)
        {
            _repository = repository;
        }

        public Client Client { get; set; }
        public string AllowedGrantTypes => string.Join(", ", Client.AllowedGrantTypes);
        public string AllowedScopes => string.Join(", ", Client.AllowedScopes);

        public async Task<IActionResult> OnGetAsync(string id)
        {
            Client = await _repository.GetClientAsync(id);

            if (Client == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
