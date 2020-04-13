using MyHealth.Mobile.Core.Services.Navigation;
using MyHealth.Mobile.Core.ViewModels.Base;
using Xamarin.Forms;

namespace MyHealth.Mobile.Core
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
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
