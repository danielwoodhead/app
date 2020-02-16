using System;
using System.Linq;
using System.Threading.Tasks;
using Hl7.Fhir.Rest;
using Microsoft.Extensions.Options;
using MyHealth.Observations.Core.Repository;
using MyHealth.Observations.Integration.Fhir.Base;
using MyHealth.Observations.Models;
using MyHealth.Observations.Models.Requests;
using FHIR = Hl7.Fhir.Model;

namespace MyHealth.Observations.Integration.Fhir
{
    public class FhirObservationsRepository : IObservationsRepository
    {
        private readonly IFhirClient _fhirClient;

        public FhirObservationsRepository(IOptions<FhirServerSettings> settings)
        {
            if (settings == null)
                throw new ArgumentNullException(nameof(settings));

            _fhirClient = new FhirClient(settings.Value.BaseUrl)
            {
                PreferredFormat = ResourceFormat.Json,
                Timeout = (int)settings.Value.Timeout.TotalMilliseconds
            };
        }

        public async Task<Observation> CreateObservationAsync(CreateObservationRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            // TODO: auth
            // TODO: error handling - FhirOperationException

            var observation = new FHIR.Observation
            {
                Code = new FHIR.CodeableConcept("system", "code"), // TODO
                Status = FHIR.ObservationStatus.Final,
                Subject = new FHIR.ResourceReference().ForPatient(request.UserId)
            };

            FHIR.Bundle transaction = _fhirClient.BuildTransaction()
                .Create(observation)
                .ForPatient(request.UserId)
                .ToBundle();

            FHIR.Bundle response = await _fhirClient.TransactionAsync(transaction);
            FHIR.Bundle.EntryComponent observationResource = response.Entry.SingleOrDefault(e => e.Resource?.ResourceType == FHIR.ResourceType.Observation);

            if (observationResource == null)
                throw new Exception(); // TODO

            return new Observation
            {
                Id = observationResource.Resource.Id
            };
        }
    }
}
