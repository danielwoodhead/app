using MyHealth.Mobile.Core.Services.Navigation;
using MyHealth.Mobile.Core.Services.Settings;
using MyHealth.Mobile.Core.ViewModels.Base;
using Xamarin.Forms;

namespace MyHealth.Mobile.Core
{
    public partial class App : Application
    {
        private readonly ISettingsService _settingsService;

        public App()
        {
            InitializeComponent();

            _settingsService = ViewModelLocator.Resolve<ISettingsService>();
            ViewModelLocator.UpdateDependencies(_settingsService.UseMocks);
        }

        protected override async void OnStart()
        {
            base.OnStart();

            var navigationService = ViewModelLocator.Resolve<INavigationService>();
            await navigationService.InitializeAsync();

            base.OnResume();
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
