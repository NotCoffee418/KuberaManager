using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KuberaManager.Models.Database;
using KuberaManager.Models.Logic;
using KuberaManager.Models.Data.KuberaCommStructure.Update;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using KuberaManager.Models.Data.KuberaCommStructure;
using KuberaManager.Models.Data.KuberaCommStructure.DetailsStructure;
using Microsoft.EntityFrameworkCore;
using KuberaManager.Models.Logic.ScenarioLogic.Scenarios;

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
            Session relevantSession = Session.FromId(input.Session);
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
                        output.Errors.Add($"Invalid session id '{input.Session}' was provided in the request. Failed to execute.");
                        return output;
                    }

                    // -- HANDLE STOPPING SESSION & JOB --//
                    Job currentJob = relevantSession.FindCurrentJob();
                    if (relevantSession.ShouldStop())
                    {
                        // Tell client to stop
                        relevantSession.ReportFinished();
                        output.Instruction = "stop-session";
                        return output;
                    }
                    else if (currentJob.ShouldStop())
                    {
                        currentJob.MarkCompleted();
                        currentJob = relevantSession.FindCurrentJob(); // redefine
                        output.Instruction = "stop-job"; // can be overridden by change-scenario 
                    }

                    // -- HANDLE CHANGING JOB ASSIGNMENT --//
                    // Interpret Details variable(s)
                    bool clientRequestsNewJob = false;
                    try
                    {
                        // This might bork
                        var detailsData = input.GetDetails<Dictionary<string, bool>>();
                        clientRequestsNewJob = detailsData["request-new-job"];
                    }
                    catch
                    {
                        output.Errors.Add($"Failed to interpret details as Dictionary<string, bool>.");
                        return output;
                    }

                    // Determine needsNewJob
                    bool needsNewJob = false;

                    // Check if CLIENT says we need a new job
                    if (clientRequestsNewJob && currentJob != null)
                    {
                        currentJob.MarkCompleted();
                        needsNewJob = true;
                    }
                    // Check if SERVER says we need a new job
                    else if (Brain.DoesClientNeedJobUpdate(relevantSession))
                    {
                        needsNewJob = true;
                    }

                    // Determine if client needs to be told what it's new job is
                    if (needsNewJob)
                    {
                        // Get new job's data
                        Job job = Brain.FindNewJob(relevantSession);
                        ScenarioBase scen = ScenarioHelper.ByIdentifier(job.ScenarioIdentifier);

                        // Prepare output data
                        output.Instruction = "change-scenario";
                        output.Data.Add("scenario-name", scen.ScenarioName);
                        output.Data.Add("scenario-argument", scen.ScenarioArgument);

                        // All set! report
                        return output;
                    }
                    // Still doing the same old. Don't send update
                    else output.Instruction = "none";

                    // Keep session alive
                    relevantSession.ReportHeartbeat();
                    break;

                case "stopping":
                    // Demand account
                    if (relevantSession == null)
                    {
                        output.Errors.Add("Session ID provided in the request does not exist. Failed to execute.");
                        return output;
                    }
                    relevantSession.ReportFinished();
                    break;

                case "report-banned":
                    // Demand account
                    if (relevantSession == null)
                    {
                        output.Errors.Add("Session ID provided in the request does not exist. Failed to execute.");
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
                    try
                    {
                        // Determine display name by session if provided
                        string displayName = "Unknown";
                        if (relevantSession != null)
                            displayName = Account.FromId(relevantSession.AccountId).Login;

                        // Get details
                        DiscordMessageStructure details = input.GetDetails<DiscordMessageStructure>();

                        // Send message
                        DiscordHandler.PostMessage($"{displayName}: {details.message}", details.tts);
                    }
                    catch
                    {
                        output.Errors.Add("discord-notify failed to invalid request structure. Must conform to DiscordMessageStructure.");
                    }
                    break;

                case "report-skills":
                    try
                    {
                        // Demand account
                        if (relevantSession == null)
                        {
                            output.Errors.Add("Session ID provided in the request does not exist. Failed to execute.");
                            return output;
                        }

                        // Decode levels
                        Levels levels = input.GetDetails<Levels>();

                        // Store account id & save
                        levels.AccountId = relevantSession.AccountId;
                        levels.Save();
                    }
                    catch (Exception ex)
                    {
                        output.Errors.Add("report-skills error storing provided skill levels. This can occcur when account is not registered in the database." + ex.Message);
                    }
                    break;

                case "report-current-action":
                    // Demand account
                    if (relevantSession == null)
                    {
                        output.Errors.Add("Session ID provided in the request does not exist. Failed to execute.");
                        return output;
                    }
                    relevantSession.SaveCurrentAction(input.GetDetails<string>());
                    break;
                case "report-quest-complete":
                    if (relevantSession == null)
                    {
                        output.Errors.Add("Session ID provided in the request does not exist. Failed to execute.");
                        return output;
                    }

                    // Grab the dictionary
                    var questStatusDict = input.GetDetails<Dictionary<int, bool>>();

                    // Get acct & save
                    Account acc = Account.FromId(relevantSession.AccountId);
                    acc.UpdateQuestCompletionData(questStatusDict);
                    break;

                default:
                    output.Errors.Add("This API request is not implemented.");
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
