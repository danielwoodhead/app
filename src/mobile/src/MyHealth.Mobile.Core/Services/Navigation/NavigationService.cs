using System;
using System.Globalization;
using System.Reflection;
using System.Threading.Tasks;
using MyHealth.Mobile.Core.Services.Settings;
using MyHealth.Mobile.Core.Services.SystemTime;
using MyHealth.Mobile.Core.ViewModels;
using MyHealth.Mobile.Core.ViewModels.Base;
using MyHealth.Mobile.Core.Views;
using Xamarin.Forms;

namespace MyHealth.Mobile.Core.Services.Navigation
{
    public class NavigationService : INavigationService
    {
        private readonly ISettingsService _settingsService;
        private readonly ISystemTimeService _systemTimeService;

        public NavigationService(ISettingsService settingsService, ISystemTimeService systemTimeService)
        {
            _settingsService = settingsService;
            _systemTimeService = systemTimeService;
        }

        public Task InitializeAsync()
        {
            if (string.IsNullOrEmpty(_settingsService.AuthAccessToken) || _settingsService.AuthAccessTokenExpiresUtc < _systemTimeService.UtcNow)
                return NavigateToAsync<LoginViewModel>();
            else
                return NavigateToAsync<MainViewModel>();
        }

        public Task NavigateToAsync<TViewModel>() where TViewModel : ViewModelBase
        {
            return InternalNavigateToAsync(typeof(TViewModel), null);
        }

        public Task NavigateToAsync<TViewModel>(object parameter) where TViewModel : ViewModelBase
        {
            return InternalNavigateToAsync(typeof(TViewModel), parameter);
        }

        private async Task InternalNavigateToAsync(Type viewModelType, object parameter)
        {
            Page page = CreatePage(viewModelType, parameter);

            if (page is LoginView)
            {
                Application.Current.MainPage = new CustomNavigationView(page);
            }
            else
            {
                var navigationPage = Application.Current.MainPage as CustomNavigationView;
                if (navigationPage != null)
                {
                    await navigationPage.PushAsync(page);
                }
                else
                {
                    Application.Current.MainPage = new CustomNavigationView(page);
                }
            }

            await (page.BindingContext as ViewModelBase).InitializeAsync(parameter);
        }

        private Type GetPageTypeForViewModel(Type viewModelType)
        {
            var viewName = viewModelType.FullName.Replace("Model", string.Empty);
            var viewModelAssemblyName = viewModelType.GetTypeInfo().Assembly.FullName;
            var viewAssemblyName = string.Format(CultureInfo.InvariantCulture, "{0}, {1}", viewName, viewModelAssemblyName);
            var viewType = Type.GetType(viewAssemblyName);
            return viewType;
        }

        private Page CreatePage(Type viewModelType, object parameter)
        {
            Type pageType = GetPageTypeForViewModel(viewModelType);
            if (pageType == null)
            {
                throw new Exception($"Cannot locate page type for {viewModelType}");
            }

            Page page = Activator.CreateInstance(pageType) as Page;
            return page;
        }
    }
}
