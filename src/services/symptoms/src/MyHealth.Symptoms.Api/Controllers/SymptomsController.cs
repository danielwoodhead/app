using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace MyHealth.Symptoms.Api.Controllers
{
    [Route("api/symptoms")]
    [ApiController]
    public class SymptomsController : ControllerBase
    {
        // GET: api/Symptoms
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Symptoms/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Symptoms
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Symptoms/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
