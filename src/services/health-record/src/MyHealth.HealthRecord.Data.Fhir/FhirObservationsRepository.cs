using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Hl7.Fhir.Rest;
using MyHealth.HealthRecord.Core.Data;
using MyHealth.HealthRecord.Data.Fhir.Base;
using MyHealth.HealthRecord.Models;
using MyHealth.HealthRecord.Models.Requests;
using FHIR = Hl7.Fhir.Model;

namespace MyHealth.HealthRecord.Data.Fhir
{
    // TODO: add auto mapper

    public class FhirObservationsRepository : IObservationsRepository
    {
        private readonly IFhirClient _client;

        public FhirObservationsRepository(IFhirClientFactory fhirClientFactory)
        {
            if (fhirClientFactory == null)
                throw new ArgumentNullException(nameof(fhirClientFactory));

            _client = fhirClientFactory.Create();
        }

        public async Task<Observation> CreateObservationAsync(CreateObservationRequest request, string userId)
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

            FHIR.Patient patient = await _client.EnsurePatientAsync(userId);

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

            FHIR.Observation observation = await _client.CreateAsync(newObservation);

            return new Observation
            {
                Id = observation.Id,
                Value = ((FHIR.FhirString)observation.Value).Value
            };
        }

        public async Task DeleteObservationAsync(string id)
        {
            await _client.DeleteAsync($"Observation/{id}");
        }

        public async Task<Observation> GetObservationAsync(string id)
        {
            FHIR.Observation observation;

            try
            {
                observation = await _client.ReadAsync<FHIR.Observation>($"Observation/{id}");
            }
            catch (FhirOperationException ex) when (ex.Status == HttpStatusCode.NotFound || ex.Status == HttpStatusCode.Gone)
            {
                return null;
            }

            return new Observation
            {
                Id = observation.Id,
                Value = ((FHIR.FhirString)observation.Value).Value
            };
        }

        public async Task<IEnumerable<Observation>> GetObservationsAsync(string userId)
        {
            FHIR.Bundle result = await _client.SearchAsync<FHIR.Observation>(new SearchParams().Add("subject:Patient.identifier", userId));

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

        public async Task<Observation> UpdateObservationAsync(string id, UpdateObservationRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            FHIR.Observation observation = await _client.ReadAsync<FHIR.Observation>($"Observation/{id}");
            observation.Value = new FHIR.FhirString(request.Content);

            FHIR.Observation updatedObservation = await _client.UpdateAsync(observation);

            return new Observation
            {
                Id = updatedObservation.Id,
                Value = ((FHIR.FhirString)observation.Value).Value
            };
        }
    }
}
