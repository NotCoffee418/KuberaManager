﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KuberaManager.Models.Database;
using KuberaManager.Models.Logic;
using KuberaManager.Models.Logic.Api.Update;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace KuberaManager.Controllers.Api
{
    [AllowAnonymous]
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
            // Prepare result
            var output = new OutputFormat();

            // not sure how this is useful but it's here.
            Session relevantSession = Session.FromAccount(input.RunescapeAccount);
            if (input.Errors.Count > 0)
            {
                output.Errors.Add("Recieved an error from the client. End session.");
                relevantSession.ReportFinished();
                return output;
            }


            switch (input.Status)
            {
                case "update":
                    // Demand account
                    if (relevantSession == null)
                    {
                        output.Errors.Add("No username was provided in the request. Failed to execute.");
                        return output;
                    }
                    Scenario scen = Brain.DetermineScenario(relevantSession);
                    output.Scenario = scen.Name;
                    break;

                case "stopping":
                    // Demand account
                    if (relevantSession == null)
                    {
                        output.Errors.Add("No username was provided in the request. Failed to execute.");
                        return output;
                    }
                    relevantSession.ReportFinished();
                    break;

                case "report-banned":
                    // Demand account
                    if (relevantSession == null)
                    {
                        output.Errors.Add("No username was provided in the request. Failed to execute.");
                        return output;
                    }

                    // Update banned state in database
                    using (var db = new kuberaDbContext())
                    {
                        Account acct = db.Accounts
                            .Where(x => x.Id == relevantSession.AccountId)
                            .FirstOrDefault();
                        if (acct != null)
                        {
                            acct.IsBanned = true;
                            db.SaveChanges();
                        }
                    }
                    relevantSession.ReportFinished();
                    break;

                case "discord-notify":
                    if (input.Details == null || input.Details == "")
                    {
                        output.Errors.Add("discord-notify failed due to lack of details field.");
                        relevantSession.IsFinished = true;
                    }
                    else
                        DiscordHandler.PostMessage($"{input.RunescapeAccount}: {input.Details}");
                    break;

                default:
                    output.Errors.Add("Invalid API request. Invalid status.");
                    relevantSession.ReportFinished();
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
