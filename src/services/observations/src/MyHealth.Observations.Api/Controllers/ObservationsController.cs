﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyHealth.Observations.Core;
using MyHealth.Observations.Models;
using MyHealth.Observations.Models.Requests;
using MyHealth.Observations.Models.Responses;

namespace MyHealth.Observations.Api.Controllers
{
    [Route("api/observations")]
    [ApiController]
    public class ObservationsController : ControllerBase
    {
        private readonly IObservationsService _observationsService;

        public ObservationsController(IObservationsService observationsService)
        {
            _observationsService = observationsService;
        }

        // GET: api/observations
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<SearchObservationsResponse>> Search()
        {
            return Ok(await _observationsService.SearchObservationsAsync());
        }

        // GET: api/observations/5
        [HttpGet("{id}", Name = "Get")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Observation>> Get(string id)
        {
            Observation observation = await _observationsService.GetObservationAsync(id);

            if (observation == null)
                return NotFound();

            return Ok(observation);
        }

        // POST: api/observations
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<Observation>> Post([FromBody] CreateObservationRequest request)
        {
            Observation observation = await _observationsService.CreateObservationAsync(request);

            return CreatedAtRoute("Get", observation.Id, observation);
        }

        // PUT: api/observations/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Observation>> Put(string id, [FromBody] UpdateObservationRequest request)
        {
            return Ok(await _observationsService.UpdateObservationAsync(id, request));
        }

        // DELETE: api/observations/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> Delete(string id)
        {
            await _observationsService.DeleteObservationAsync(id);

            return NoContent();
        }
    }
}