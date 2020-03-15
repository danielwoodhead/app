using System.Threading.Tasks;
using MyHealth.Mobile.Core.ViewModels.Base;

namespace MyHealth.Mobile.Core.Services.Navigation
{
    public interface INavigationService
    {
        Task InitializeAsync();
        Task NavigateToAsync<TViewModel>() where TViewModel : ViewModelBase;
        Task NavigateToAsync<TViewModel>(object parameter) where TViewModel : ViewModelBase;
    }
}
