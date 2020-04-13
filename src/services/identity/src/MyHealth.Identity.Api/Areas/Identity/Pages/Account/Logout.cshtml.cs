using System.Threading.Tasks;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace MyHealth.Identity.Api.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<LogoutModel> _logger;
        private readonly IIdentityServerInteractionService _identityServer;

        public LogoutModel(SignInManager<IdentityUser> signInManager, ILogger<LogoutModel> logger, IIdentityServerInteractionService identityServer)
        {
            _signInManager = signInManager;
            _logger = logger;
            _identityServer = identityServer;
        }

        public async Task<IActionResult> OnGet(string logoutId)
        {
            string returnUrl = null;

            if (!string.IsNullOrEmpty(logoutId))
            {
                var logoutContext = await _identityServer.GetLogoutContextAsync(logoutId);

                returnUrl = logoutContext.PostLogoutRedirectUri;
            }

            if (returnUrl != null)
            {
                return await OnPost(returnUrl);
            }
            else
            {
                return Page();
            }
        }

        public async Task<IActionResult> OnPost(string returnUrl = null)
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            if (returnUrl != null)
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToPage();
            }
        }
    }
}
