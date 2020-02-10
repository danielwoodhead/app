using System.Collections.Generic;
using System.Threading.Tasks;
using MyHealth.Symptoms.Models;
using MyHealth.Symptoms.Models.Requests;
using MyHealth.Symptoms.Models.Responses;

namespace MyHealth.Symptoms.Core
{
    public interface ISymptomsService
    {
        Task<Symptom> CreateSymptomAsync(CreateSymptomRequest request);
        Task DeleteSymptomAsync(string id);
        Task<Symptom> GetSymptomAsync(string id);
        Task<SearchSymptomsResponse> SearchSymptomsAsync();
        Task<Symptom> UpdateSymptomAsync(string id, UpdateSymptomRequest request);
    }
}
