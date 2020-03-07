using System;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyHealth.Identity.Api.Areas.Admin.Services;

namespace MyHealth.Identity.Api.Areas.Admin.Pages.Clients
{
    public class CreateModel : PageModel
    {
        private readonly IClientRepository _repository;

        public CreateModel(IClientRepository repository)
        {
            _repository = repository;
        }

        [BindProperty]
        public Client Client { get; set; }

        [BindProperty]
        public string AllowedGrantTypes { get; set; }

        [BindProperty]
        public string AllowedScopes { get; set; }

        [BindProperty]
        public string ClientSecrets { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Client.AllowedGrantTypes = AllowedGrantTypes.Split(Environment.NewLine);
            Client.AllowedScopes = AllowedScopes.Split(Environment.NewLine);
            Client.ClientSecrets = ClientSecrets.Split(Environment.NewLine).Select(x => new Secret(x)).ToList();
            await _repository.CreateClientAsync(Client);

            return RedirectToPage("./Index");
        }
    }
}
