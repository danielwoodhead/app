using System;
using System.Linq;
using System.Threading.Tasks;
using Hl7.Fhir.Rest;
using Microsoft.Extensions.Options;
using MyHealth.Observations.Core.Repository;
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
            _fhirClient = new FhirClient(settings.Value.BaseUrl);
            _fhirClient.PreferredFormat = ResourceFormat.Json;
            _fhirClient.Timeout = 30000; // ms
        }

        public async Task<Observation> CreateObservationAsync(CreateObservationRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            // TODO: auth
            // TODO: error handling - FhirOperationException

            var patient = new FHIR.Patient();
            patient.Identifier.Add(new FHIR.Identifier("def.myhealth.io/Patient", request.UserId));

            var createPatientConditions = new SearchParams();
            createPatientConditions.Add("identifier", $"def.myhealth.io/Patient|{request.UserId}");

            var observation = new FHIR.Observation();
            observation.Status = FHIR.ObservationStatus.Final;
            observation.Subject = new FHIR.ResourceReference($"{nameof(FHIR.Patient)}/{request.UserId}");

            var builder = new TransactionBuilder(_fhirClient.Endpoint.AbsoluteUri, FHIR.Bundle.BundleType.Transaction)
                .Create(patient, createPatientConditions)
                .Create(observation);

            var bundle = await _fhirClient.TransactionAsync(builder.ToBundle());
            var observationResource = bundle.Entry.SingleOrDefault(e => e.Resource.ResourceType == FHIR.ResourceType.Observation);

            if (observationResource == null)
                throw new Exception();

            return new Observation
            {
                Id = observationResource.ElementId
            };
        }
    }
}
