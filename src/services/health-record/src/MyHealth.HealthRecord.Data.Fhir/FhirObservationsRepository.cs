using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hl7.Fhir.Rest;
using MyHealth.HealthRecord.Core.Data;
using MyHealth.HealthRecord.Models;
using FHIR = Hl7.Fhir.Model;

namespace MyHealth.HealthRecord.Data.Fhir
{
    public class FhirObservationsRepository : IObservationsRepository
    {
        private readonly IFhirClient _fhirClient;

        public FhirObservationsRepository(IFhirClient fhirClient)
        {
            _fhirClient = fhirClient;
        }

        public async Task<IEnumerable<Observation>> GetObservationsAsync(string userId)
        {
            FHIR.Bundle result = await _fhirClient.SearchAsync<FHIR.Observation>(new SearchParams().Add("subject:Patient.identifier", userId));

            return result.Entry
                .Select(e => (FHIR.Observation)e.Resource)
                .Select(ConvertObservation);
        }

        private Observation ConvertObservation(FHIR.Observation observation)
        {
            var result = new Observation
            {
                Id = observation.Id,
                Text = observation.Code.Text
            };

            if (observation.Effective is FHIR.Period periodValue)
            {
                result.DateTime = periodValue.StartElement.ToDateTimeOffset();
            }
            else
            {
                throw new ArgumentException($"Unsupported FHIR date type {observation.Effective.GetType().Name}.");
            }

            result.Value = observation.ToValue();

            return result;
        }
    }
}
