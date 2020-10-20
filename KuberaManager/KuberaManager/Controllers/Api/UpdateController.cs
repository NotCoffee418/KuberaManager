using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KuberaManager.Models.Logic.Api.Update;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace KuberaManager.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class UpdateController : ControllerBase
    {
        // GET: api/Update
        [HttpGet]
        public OutputFormat Get()
        {
            var output = new OutputFormat();
            output.Errors.Add("This call requires a POST request.");
            return output;
        }

        // GET api/<UpdateController>/5
        [HttpGet("{id}")]
        public OutputFormat Get(int id)
        {
            var output = new OutputFormat();
            output.Errors.Add("This call requires a POST request.");
            return output;
        }

        // POST api/<UpdateController>
        [HttpPost]
        public OutputFormat Post([FromBody] InputFormat value)
        {
            var output = new OutputFormat();
            output.Errors.Add("Not Implemented Yet");
            return output;
        }

        // PUT api/<UpdateController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
            var output = new OutputFormat();
            output.Errors.Add("This call requires a POST request.");
        }

        // DELETE api/<UpdateController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            var output = new OutputFormat();
            output.Errors.Add("This call requires a POST request.");
        }
    }
}
