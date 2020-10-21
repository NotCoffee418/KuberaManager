using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KuberaManager.Models;
using KuberaManager.Models.Logic;
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
        public OutputFormat Post([FromBody] InputFormat input)
        {
            // Validate Input data
            var output = new OutputFormat();
            if (input.RunescapeAccount == null || input.RunescapeAccount == "" || !input.HasValidStatus())
            {
                output.Errors.Add("Invalid API request. Missing or invalid information. Case sensitive.");
                return output;
            }


            Session relevantSession = Session.FromAccount(input.RunescapeAccount);

            // not sure how this is useful but it's here.
            if (input.Errors.Count > 0)
            {
                output.Errors.Add("Recieved an error from the client. End session.");
                relevantSession.IsFinished = true;
                return output;
            }

            switch (input.Status)
            {
                case "update":
                    Scenario scen = Brain.DetermineScenario(relevantSession);
                    output.Scenario = scen.Name;
                    break;

                case "stopping":
                    relevantSession.IsFinished = true;
                    break;

                case "discord-notify":
                    if (input.Details == null || input.Details == "")
                    {
                        output.Errors.Add("discord-notify failed due to lack of details field.");
                        relevantSession.IsFinished = true;
                    }
                    else DiscordHandler.PostMessage(input.Details);
                    break;

                default:
                    output.Errors.Add("NANI!?");
                    relevantSession.IsFinished = true;
                    break;
            }

            // Returns results or errors
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
