using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyHealth.Symptoms.Core;
using MyHealth.Symptoms.Models;
using MyHealth.Symptoms.Models.Requests;
using MyHealth.Symptoms.Models.Responses;

namespace MyHealth.Symptoms.Api.Controllers
{
    [Route("api/symptoms")]
    [ApiController]
    public class SymptomsController : ControllerBase
    {
        private readonly ISymptomsService _symptomsService;

        public SymptomsController(ISymptomsService symptomsService)
        {
            _symptomsService = symptomsService;
        }

        // GET: api/Symptoms
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<SearchSymptomsResponse>> Search()
        {
            return Ok(await _symptomsService.SearchSymptomsAsync());
        }

        // GET: api/Symptoms/5
        [HttpGet("{id}", Name = "Get")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Symptom>> Get(string id)
        {
            Symptom symptom = await _symptomsService.GetSymptomAsync(id);

            if (symptom == null)
                return NotFound();

            return Ok(symptom);
        }

        // POST: api/Symptoms
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<Symptom>> Post([FromBody] CreateSymptomRequest request)
        {
            Symptom symptom = await _symptomsService.CreateSymptomAsync(request);

            return CreatedAtRoute("Get", symptom.Id, symptom);
        }

        // PUT: api/Symptoms/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Symptom>> Put(string id, [FromBody] UpdateSymptomRequest request)
        {
            return Ok(await _symptomsService.UpdateSymptomAsync(id, request));
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> Delete(string id)
        {
            await _symptomsService.DeleteSymptomAsync(id);

            return NoContent();
        }
    }
}
