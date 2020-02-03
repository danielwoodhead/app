using System.Threading.Tasks;
using MyApp.Models.Navigation;
using MyApp.ViewModels.Base;
using Xamarin.Forms;

namespace MyApp.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
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
