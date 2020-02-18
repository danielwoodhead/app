using System;
using System.Collections.Generic;
using System.Net;
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
    // TODO: add auto mapper

    public class FhirObservationsRepository : IObservationsRepository
    {
        private readonly IFhirClient _fhirClient;

        public FhirObservationsRepository(IOptions<FhirServerSettings> settings)
        {
            if (settings == null)
                throw new ArgumentNullException(nameof(settings));

            // TODO: auth
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

            // it should be possible to create the observation and the patient (if the patient doesn't
            // already exist) in a single transaction bundle, however this doesn't seem to work correctly
            //
            // var patient = new FHIR.Patient();
            // patient.Identifier.Add(new FHIR.Identifier(IdentifierSystem.Patient, request.UserId));
            //
            // var condition = new SearchParams();
            // condition.Add("identifier", $"{IdentifierSystem.Patient}|{request.UserId}");
            //
            // builder.Create(patient, condition);
            //
            // var observation = new FHIR.Observation
            // {
            //    Subject = new FHIR.ResourceReference($"Patient?identifier={IdentifierSystem.Patient}|{request.UserId}")
            //    ...
            // };
            // builder.Create(observation);

            FHIR.Patient patient = await _fhirClient.EnsurePatientAsync(request.UserId);

            var newObservation = new FHIR.Observation
            {
                Code = new FHIR.CodeableConcept("system", "code"), // TODO
                Status = FHIR.ObservationStatus.Final,
                Subject = new FHIR.ResourceReference($"Patient/{patient.Id}"),
                Performer = new List<FHIR.ResourceReference>
                {
                    new FHIR.ResourceReference($"Patient/{patient.Id}")
                },
                Value = new FHIR.FhirString(request.Content)
            };

            FHIR.Observation observation = await _fhirClient.CreateAsync(newObservation);

            return new Observation
            {
                Id = observation.Id,
                Content = ((FHIR.FhirString)observation.Value).Value
            };
        }

        public async Task DeleteObservationAsync(string id)
        {
            await _fhirClient.DeleteAsync($"Observation/{id}");
        }

        public async Task<Observation> GetObservationAsync(string id)
        {
            FHIR.Observation observation;

            try
            {
                observation = await _fhirClient.ReadAsync<FHIR.Observation>($"Observation/{id}");
            }
            catch (FhirOperationException ex) when (ex.Status == HttpStatusCode.NotFound || ex.Status == HttpStatusCode.Gone)
            {
                return null;
            }

            return new Observation
            {
                Id = observation.Id,
                Content = ((FHIR.FhirString)observation.Value).Value
            };
        }

        public async Task<Observation> UpdateObservationAsync(string id, UpdateObservationRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            FHIR.Observation observation = await _fhirClient.ReadAsync<FHIR.Observation>($"Observation/{id}");
            observation.Value = new FHIR.FhirString(request.Content);

            FHIR.Observation updatedObservation = await _fhirClient.UpdateAsync(observation);

            return new Observation
            {
                Id = updatedObservation.Id,
                Content = ((FHIR.FhirString)observation.Value).Value
            };
        }
    }
}
