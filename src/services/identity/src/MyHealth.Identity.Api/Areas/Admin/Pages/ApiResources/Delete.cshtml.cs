using System.Threading.Tasks;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyHealth.Identity.Api.Areas.Admin.Services;

namespace MyHealth.Identity.Api.Areas.Admin.Pages.ApiResources
{
    public class DeleteModel : PageModel
    {
        private readonly IApiResourceRepository _repository;

        public DeleteModel(IApiResourceRepository repository)
        {
            _repository = repository;
        }

        [BindProperty]
        public ApiResource ApiResource { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ApiResource = await _repository.GetApiResourceAsync(id);

            if (ApiResource == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            await _repository.DeleteApiResourceAsync(id);

            return RedirectToPage("./Index");
        }
    }
}
