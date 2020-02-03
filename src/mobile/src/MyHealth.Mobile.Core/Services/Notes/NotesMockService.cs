using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyHealth.Mobile.Core.Services.Notes
{
    public class NotesMockService : INotesService
    {
        private readonly IList<string> _notes = new List<string> { "hello", "world" };

        public Task AddNoteAsync(string note)
        {
            _notes.Add(note);

            return Task.CompletedTask;
        }

        public Task<IEnumerable<string>> GetNotesAsync()
        {
            return Task.FromResult((IEnumerable<string>)_notes);
        }
    }
}
