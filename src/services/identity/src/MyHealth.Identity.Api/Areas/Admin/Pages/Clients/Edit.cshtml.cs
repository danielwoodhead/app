using System;
using System.Threading.Tasks;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyHealth.Identity.Api.Areas.Admin.Services;

namespace MyHealth.Identity.Api.Areas.Admin.Pages.Clients
{
    public class EditModel : PageModel
    {
        private readonly IClientRepository _repository;

        public EditModel(IClientRepository repository)
        {
            _repository = repository;
        }

        [BindProperty]
        public Client Client { get; set; }

        [BindProperty]
        public string AllowedGrantTypes { get; set; }

        [BindProperty]
        public string AllowedScopes { get; set; }


        public async Task<IActionResult> OnGetAsync(string id)
        {
            Client = await _repository.GetClientAsync(id);

            if (Client == null)
            {
                return RedirectToPage("./Index");
            }

            AllowedGrantTypes = string.Join(Environment.NewLine, Client.AllowedGrantTypes);
            AllowedScopes = string.Join(Environment.NewLine, Client.AllowedScopes);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Client.AllowedGrantTypes = AllowedGrantTypes.Split(Environment.NewLine);
            Client.AllowedScopes = AllowedScopes.Split(Environment.NewLine);
            await _repository.UpdateClientAsync(Client);

            return RedirectToPage("./Index");
        }
    }
}
