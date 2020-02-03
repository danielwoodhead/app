using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyHealth.Mobile.Core.Services.Notes
{
    public interface INotesService
    {
        Task AddNoteAsync(string note);
        Task<IEnumerable<string>> GetNotesAsync();
    }
}
