using System.Collections.Generic;
using System.Threading.Tasks;
using MyHealth.Mobile.Core.Services.Api;

namespace MyHealth.Mobile.Core.Services.Notes
{
    public class NotesService : INotesService
    {
        private readonly IApiClient _apiClient;

        public NotesService(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public Task AddNoteAsync(string note)
        {
            // TODO
            return Task.CompletedTask;
        }

        public Task<IEnumerable<string>> GetNotesAsync()
        {
            // TODO
            return Task.FromResult((IEnumerable<string>)new[] { "TODO" });
        }
    }
}
