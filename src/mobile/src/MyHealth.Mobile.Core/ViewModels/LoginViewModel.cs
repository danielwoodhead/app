using System.Net;
using System.Threading.Tasks;
using System.Windows.Input;
using IdentityModel.Client;
using MyHealth.Mobile.Core.Models.Identity;
using MyHealth.Mobile.Core.Services.Identity;
using MyHealth.Mobile.Core.Services.Settings;
using MyHealth.Mobile.Core.Services.SystemTime;
using MyHealth.Mobile.Core.ViewModels.Base;
using Xamarin.Forms;

namespace MyHealth.Mobile.Core.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly IIdentityService _identityService;
        private readonly ISettingsService _settingsService;
        private readonly ISystemTimeService _systemTimeService;

        public LoginViewModel(
            IIdentityService identityService,
            ISettingsService settingsService,
            ISystemTimeService systemTimeService)
        {
            _identityService = identityService;
            _settingsService = settingsService;
            _systemTimeService = systemTimeService;
        }

        private string _loginUrl;

        public string LoginUrl
        {
            get
            {
                return _loginUrl;
            }
            set
            {
                _loginUrl = value;
                RaisePropertyChanged(() => LoginUrl);
            }
        }

        public ICommand NavigateCommand => new Command<string>(async (url) => await NavigateAsync(url));

        public override Task InitializeAsync(object navigationData)
        {
            if (navigationData is LogoutParameter)
            {
                Logout();
            }
            else
            {
                LoginUrl = _identityService.CreateAuthorizationRequest();
            }

            return base.InitializeAsync(navigationData);
        }

        private async Task NavigateAsync(string url)
        {
            var decodedUrl = WebUtility.UrlDecode(url);

            if (decodedUrl.StartsWith(GlobalSettings.AuthLogoutCallback))
            {
                _settingsService.AuthAccessToken = string.Empty;
                _settingsService.AuthIdentityToken = string.Empty;
                LoginUrl = _identityService.CreateAuthorizationRequest();
            }
            else if (decodedUrl.StartsWith(GlobalSettings.AuthRedirectUri))
            {
                var authResponse = new AuthorizeResponse(url);
                var userToken = await _identityService.GetTokenAsync(authResponse.Code);

                _settingsService.AuthAccessToken = userToken.AccessToken;
                _settingsService.AuthAccessTokenExpiresUtc = _systemTimeService.UtcNow.AddSeconds(userToken.ExpiresIn - 30);
                _settingsService.AuthIdentityToken = userToken.IdentityToken;
                await NavigationService.NavigateToAsync<MainViewModel>();
            }
        }

        private void Logout()
        {
            LoginUrl = _identityService.CreateLogoutRequest(_settingsService.AuthIdentityToken);
        }
    }
}
