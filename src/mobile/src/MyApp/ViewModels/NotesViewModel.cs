using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using MyApp.Services.Notes;
using MyApp.Validation;
using MyApp.ViewModels.Base;
using Xamarin.Forms;

namespace MyApp.ViewModels
{
    public class NotesViewModel : ViewModelBase
    {
        private readonly INotesService _notesService;

        private ValidatableObject<string> _note;
        private ObservableCollection<string> _notes;

        public ValidatableObject<string> Note
        {
            get
            {
                return _note;
            }
            set
            {
                _note = value;
                RaisePropertyChanged(() => Note);
            }
        }

        public ObservableCollection<string> Notes
        {
            get { return _notes; }
            set
            {
                _notes = value;
                RaisePropertyChanged(() => Notes);
            }
        }

        public ICommand AddNoteCommand => new Command(async () => await AddNoteAsync());

        public NotesViewModel(INotesService notesService)
        {
            _notesService = notesService;
            _note = new ValidatableObject<string>();
        }

        public override async Task InitializeAsync(object navigationData)
        {
            if (Notes == null)
                Notes = new ObservableCollection<string>();

            var notes = await _notesService.GetNotesAsync();

            Notes.Clear();
            foreach (var note in notes)
            {
                Notes.Add(note);
            }

            await base.InitializeAsync(navigationData);
        }

        private async Task AddNoteAsync()
        {
            IsBusy = true;

            Notes.Add(Note.Value);
            RaisePropertyChanged(() => Notes);

            await _notesService.AddNoteAsync(Note.Value);

            IsBusy = false;
        }
    }
}
