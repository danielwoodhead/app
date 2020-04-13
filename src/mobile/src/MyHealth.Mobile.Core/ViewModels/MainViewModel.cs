using System.Threading.Tasks;
using System.Windows.Input;
using MyHealth.Mobile.Core.Models.Identity;
using MyHealth.Mobile.Core.Models.Navigation;
using MyHealth.Mobile.Core.ViewModels.Base;
using Xamarin.Forms;

namespace MyHealth.Mobile.Core.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public ICommand LogoutCommand => new Command(Logout);

        private void Logout(object obj)
        {
            NavigationService.NavigateToAsync<LoginViewModel>(new LogoutParameter { Logout = true });
        }

        public override Task InitializeAsync(object navigationData)
        {
            IsBusy = true;

            if (navigationData is TabParameter)
            {
                var tabIndex = ((TabParameter)navigationData).TabIndex;
                MessagingCenter.Send(this, MessageKeys.ChangeTab, tabIndex);
            }

            return base.InitializeAsync(navigationData);
        }

    }
}
