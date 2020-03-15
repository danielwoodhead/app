using System.Threading.Tasks;
using System.Windows.Input;
using MyHealth.Mobile.Core.Models.Observations;
using MyHealth.Mobile.Core.Services.Observations;
using MyHealth.Mobile.Core.Validation;
using MyHealth.Mobile.Core.ViewModels.Base;
using Xamarin.Forms;

namespace MyHealth.Mobile.Core.ViewModels
{
    public class ObservationDetailsViewModel : ViewModelBase
    {
        private readonly IObservationsService _observationsService;

        private ValidatableObject<Observation> _observation;
        private bool _canDeleteObservation;

        public ValidatableObject<Observation> Observation
        {
            get
            {
                return _observation;
            }
            set
            {
                _observation = value;
                RaisePropertyChanged(() => Observation);
            }
        }

        public bool CanDeleteObservation
        {
            get
            {
                return _canDeleteObservation;
            }
            set
            {
                _canDeleteObservation = value;
                RaisePropertyChanged(() => CanDeleteObservation);
            }
        }

        public ICommand DeleteObservationCommand => new Command(async () => await DeleteObservationAsync());
        public ICommand SaveObservationCommand => new Command(async () => await SaveObservationAsync());

        public ObservationDetailsViewModel(IObservationsService observationsService)
        {
            _observationsService = observationsService;
            _observation = new ValidatableObject<Observation>();
        }

        public override async Task InitializeAsync(object navigationData)
        {
            if (navigationData != null)
            {
                IsBusy = true;
                string observationId = (string)navigationData;
                Observation observation = await _observationsService.GetObservationAsync(observationId);
                Observation.Value = observation;
                CanDeleteObservation = true;
                IsBusy = false;
            }
            else
            {
                Observation.Value = new Observation();
            }
        }

        private async Task DeleteObservationAsync()
        {
            await _observationsService.DeleteObservationAsync(Observation.Value.Id);
            await NavigationService.NavigateToAsync<ObservationsViewModel>();
        }

        private async Task SaveObservationAsync()
        {
            if (Observation.Value.Id == null)
            {
                await _observationsService.AddObservationAsync(Observation.Value);
            }
            else
            {
                await _observationsService.UpdateObservationAsync(Observation.Value);
            }

            await NavigationService.NavigateToAsync<ObservationsViewModel>();
        }
    }
}
