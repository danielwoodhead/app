using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using MyHealth.Mobile.Core.Services.Observations;
using MyHealth.Mobile.Core.Validation;
using MyHealth.Mobile.Core.ViewModels.Base;
using Xamarin.Forms;

namespace MyHealth.Mobile.Core.ViewModels
{
    public class ObservationsViewModel : ViewModelBase
    {
        private readonly IObservationsService _observationsService;

        private ValidatableObject<string> _observation;
        private ObservableCollection<string> _observations;

        public ValidatableObject<string> Observation
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

        public ObservableCollection<string> Observations
        {
            get { return _observations; }
            set
            {
                _observations = value;
                RaisePropertyChanged(() => Observations);
            }
        }

        public ICommand AddObservationCommand => new Command(async () => await AddObservationAsync());

        public ObservationsViewModel(IObservationsService observationsService)
        {
            _observationsService = observationsService;
            _observation = new ValidatableObject<string>();
        }

        public override async Task InitializeAsync(object navigationData)
        {
            if (Observations == null)
                Observations = new ObservableCollection<string>();

            IEnumerable<string> observations = await _observationsService.GetObservationsAsync();

            Observations.Clear();
            foreach (string observation in observations)
            {
                Observations.Add(observation);
            }

            await base.InitializeAsync(navigationData);
        }

        private async Task AddObservationAsync()
        {
            IsBusy = true;

            Observations.Add(Observation.Value);
            RaisePropertyChanged(() => Observations);

            await _observationsService.AddObservationAsync(Observation.Value);

            IsBusy = false;
        }
    }
}
