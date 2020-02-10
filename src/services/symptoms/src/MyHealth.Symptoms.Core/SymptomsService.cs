using System.Threading.Tasks;
using MyHealth.Symptoms.Models;
using MyHealth.Symptoms.Models.Requests;
using MyHealth.Symptoms.Models.Responses;

namespace MyHealth.Symptoms.Core
{
    public class SymptomsService : ISymptomsService
    {
        public Task<Symptom> CreateSymptomAsync(CreateSymptomRequest request)
        {
            throw new System.NotImplementedException();
        }

        public Task DeleteSymptomAsync(string id)
        {
            throw new System.NotImplementedException();
        }

        public Task<Symptom> GetSymptomAsync(string id)
        {
            throw new System.NotImplementedException();
        }

        public Task<SearchSymptomsResponse> SearchSymptomsAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<Symptom> UpdateSymptomAsync(string id, UpdateSymptomRequest request)
        {
            throw new System.NotImplementedException();
        }
    }
}
