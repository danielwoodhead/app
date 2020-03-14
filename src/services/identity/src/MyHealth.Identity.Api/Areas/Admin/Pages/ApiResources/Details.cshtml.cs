using System;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyHealth.Identity.Api.Areas.Admin.Services;

namespace MyHealth.Identity.Api.Areas.Admin.Pages.ApiResources
{
    public class DetailsModel : PageModel
    {
        private readonly IApiResourceRepository _repository;

        public DetailsModel(IApiResourceRepository repository)
        {
            _repository = repository;
        }

        public ApiResource ApiResource { get; set; }

        public string Scopes => string.Join(", ", ApiResource.Scopes.Select(x => x.Name));

        public async Task<IActionResult> OnGetAsync(string id)
        {
            ApiResource = await _repository.GetApiResourceAsync(id);

            if (ApiResource == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
