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
        private readonly IFhirClientFactory _fhirClientFactory;

        public FhirObservationsRepository(IFhirClientFactory fhirClientFactory)
        {
            _fhirClientFactory = fhirClientFactory ?? throw new ArgumentNullException(nameof(fhirClientFactory));
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

            IFhirClient client = await _fhirClientFactory.InstanceAsync();
            FHIR.Patient patient = await client.EnsurePatientAsync(userId);

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

            FHIR.Observation observation = await client.CreateAsync(newObservation);

            return new Observation
            {
                Id = observation.Id,
                Value = ((FHIR.FhirString)observation.Value).Value
            };
        }

        public async Task DeleteObservationAsync(string id)
        {
            IFhirClient client = await _fhirClientFactory.InstanceAsync();
            await client.DeleteAsync($"Observation/{id}");
        }

        public async Task<Observation> GetObservationAsync(string id)
        {
            IFhirClient client = await _fhirClientFactory.InstanceAsync();
            FHIR.Observation observation;

            try
            {
                observation = await client.ReadAsync<FHIR.Observation>($"Observation/{id}");
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
            IFhirClient client = await _fhirClientFactory.InstanceAsync();

            FHIR.Bundle result = await client.SearchAsync<FHIR.Observation>(new SearchParams().Add("subject:Patient.identifier", userId));

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

            if (observation.Value is FHIR.FhirString stringValue)
            {
                result.Value = stringValue.Value;
            }
            else if (observation.Value is FHIR.Quantity quantityValue)
            {
                result.Value = quantityValue.Value.ToString();
                result.Unit = quantityValue.Unit;
            }
            else
            {
                throw new ArgumentException($"Unsupported FHIR value type {observation.Value.GetType().Name}.");
            }

            return result;
        }

        public async Task<Observation> UpdateObservationAsync(string id, UpdateObservationRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            IFhirClient client = await _fhirClientFactory.InstanceAsync();
            FHIR.Observation observation = await client.ReadAsync<FHIR.Observation>($"Observation/{id}");
            observation.Value = new FHIR.FhirString(request.Content);

            FHIR.Observation updatedObservation = await client.UpdateAsync(observation);

            return new Observation
            {
                Id = updatedObservation.Id,
                Value = ((FHIR.FhirString)observation.Value).Value
            };
        }
    }
}
