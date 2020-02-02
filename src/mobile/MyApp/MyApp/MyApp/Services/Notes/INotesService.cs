using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyApp.Services.Notes
{
    public interface INotesService
    {
        Task AddNoteAsync(string note);
        Task<IEnumerable<string>> GetNotesAsync();
    }
}
