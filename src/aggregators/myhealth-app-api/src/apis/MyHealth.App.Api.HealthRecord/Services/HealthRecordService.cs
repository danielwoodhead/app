using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hl7.Fhir.Rest;
using MyHealth.App.Api.HealthRecord.Extensions;
using MyHealth.App.Api.HealthRecord.Models;
using MyHealth.Extensions.AspNetCore.Context;
using FHIR = Hl7.Fhir.Model;

namespace MyHealth.App.Api.HealthRecord.Services
{
    public class HealthRecordService : IHealthRecordService
    {
        private readonly IFhirClient _fhirClient;
        private readonly IUserOperationContext _userContext;

        public HealthRecordService(IFhirClient fhirClient, IUserOperationContext userContext)
        {
            _fhirClient = fhirClient;
            _userContext = userContext;
        }

        public async Task<SearchObservationsResponse> GetObservationsAsync()
        {
            string userId = _userContext.UserId;
            FHIR.Bundle result = await _fhirClient.SearchAsync<FHIR.Observation>(new SearchParams().Add("subject:Patient.identifier", userId));

            IEnumerable<Observation> observations = result.Entry
                .Select(e => (FHIR.Observation)e.Resource)
                .Select(ConvertObservation);

            return new SearchObservationsResponse
            {
                Observations =  observations
            };
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
