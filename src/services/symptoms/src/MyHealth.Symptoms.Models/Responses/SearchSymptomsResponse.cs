using System.Collections.Generic;

namespace MyHealth.Symptoms.Models.Responses
{
    public class SearchSymptomsResponse
    {
        public IEnumerable<Symptom> Symptoms { get; set; }
    }
}
