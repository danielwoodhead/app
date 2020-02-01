using MyApp.Services.Navigation;
using MyApp.ViewModels.Base;
using MyApp.Views;
using Xamarin.Forms;

namespace MyApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new MainView();
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
