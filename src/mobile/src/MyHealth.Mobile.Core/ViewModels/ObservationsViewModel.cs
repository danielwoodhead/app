using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using MyHealth.Mobile.Core.Models.Observations;
using MyHealth.Mobile.Core.Services.Observations;
using MyHealth.Mobile.Core.ViewModels.Base;
using Xamarin.Forms;

namespace MyHealth.Mobile.Core.ViewModels
{
    public class ObservationsViewModel : ViewModelBase
    {
        private readonly IObservationsService _observationsService;

        private ObservableCollection<Observation> _observations;

        public ObservableCollection<Observation> Observations
        {
            get { return _observations; }
            set
            {
                _observations = value;
                RaisePropertyChanged(() => Observations);
            }
        }

        public ICommand NewObservationCommand => new Command(async () => await NewObservationAsync());
        public ICommand OpenObservationCommand => new Command<Observation>(async (item) => await OpenObservationAsync(item));

        public ObservationsViewModel(IObservationsService observationsService)
        {
            _observationsService = observationsService;
        }

        public override async Task InitializeAsync(object navigationData)
        {
            if (Observations == null)
                Observations = new ObservableCollection<Observation>();

            IEnumerable<Observation> observations = await _observationsService.GetObservationsAsync();

            Observations.Clear();
            foreach (Observation observation in observations)
            {
                Observations.Add(observation);
            }

            await base.InitializeAsync(navigationData);
        }

        private async Task NewObservationAsync()
        {
            await NavigationService.NavigateToAsync<ObservationDetailsViewModel>();
        }

        private async Task OpenObservationAsync(Observation item)
        {
            await NavigationService.NavigateToAsync<ObservationDetailsViewModel>(item.Id);
        }
    }
}
