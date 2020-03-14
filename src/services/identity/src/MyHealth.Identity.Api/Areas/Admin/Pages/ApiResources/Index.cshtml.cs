using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyHealth.Identity.Api.Areas.Admin.Services;

namespace MyHealth.Identity.Api.Areas.Admin.Pages.ApiResources
{
    public class IndexModel : PageModel
    {
        private readonly IApiResourceRepository _repository;

        public IndexModel(IApiResourceRepository repository)
        {
            _repository = repository;
        }

        public IList<ApiResource> ApiResources { get; private set; }

        public async Task OnGetAsync()
        {
            ApiResources = (await _repository.GetAllApiResourcesAsync()).ToList();
        }
    }
}
