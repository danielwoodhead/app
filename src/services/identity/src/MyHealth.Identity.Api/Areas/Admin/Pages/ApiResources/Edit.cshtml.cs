using System;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyHealth.Identity.Api.Areas.Admin.Services;

namespace MyHealth.Identity.Api.Areas.Admin.Pages.ApiResources
{
    public class EditModel : PageModel
    {
        private readonly IApiResourceRepository _repository;

        public EditModel(IApiResourceRepository repository)
        {
            _repository = repository;
        }

        [BindProperty]
        public ApiResource ApiResource { get; set; }

        [BindProperty]
        public string Scopes { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            ApiResource = await _repository.GetApiResourceAsync(id);

            if (ApiResource == null)
            {
                return RedirectToPage("./Index");
            }

            Scopes = string.Join(Environment.NewLine, ApiResource.Scopes.Select(x => x.Name));

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            ApiResource.Scopes = Scopes.Split(Environment.NewLine).Select(x => new Scope(x)).ToList();
            await _repository.UpdateApiResourceAsync(ApiResource);

            return RedirectToPage("./Index");
        }
    }
}
