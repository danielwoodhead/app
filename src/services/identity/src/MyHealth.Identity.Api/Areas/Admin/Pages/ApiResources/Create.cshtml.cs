using System;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyHealth.Identity.Api.Areas.Admin.Services;

namespace MyHealth.Identity.Api.Areas.Admin.Pages.ApiResources
{
    public class CreateModel : PageModel
    {
        private readonly IApiResourceRepository _repository;

        public CreateModel(IApiResourceRepository repository)
        {
            _repository = repository;
        }

        [BindProperty]
        public ApiResource ApiResource { get; set; }

        [BindProperty]
        public string Scopes { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            ApiResource.Scopes = Scopes.Split(Environment.NewLine).Select(s => new Scope(s)).ToList();
            await _repository.CreateApiResourceAsync(ApiResource);

            return RedirectToPage("./Index");
        }
    }
}
