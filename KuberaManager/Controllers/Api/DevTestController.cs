using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KuberaManager.Models.Database;
using KuberaManager.Models.Logic.ScenarioLogic.Scenarios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KuberaManager.Controllers.Api
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DevTestController : ControllerBase
    {
        [HttpGet]
        public string CreateSpoofSession()
        {
            return "/accountId/status(brain|active|today|old)/durationHours";
        }
        [HttpGet("{accountId}/{status?}/{durationHours?}")]
        public dynamic CreateSpoofSession(int accountId, string status = "active", int durationHours = 8)
        {
            // Create session through brain
            if (status == "brain")
                return Session.FromAccount(Account.FromId(accountId).Login);

            // Prepare session for all other statuses
            Session sess = new Session()
            {
                AccountId = accountId,
                RspeerSessionTag = "fakesession-devtest",
                TargetDuration = TimeSpan.FromHours(durationHours),
                ActiveComputer = 1,
            };

            // Set active
            switch (status)
            {
                case "active":
                    sess.StartTime = DateTime.Now.AddHours((durationHours / 2) * -1);
                    sess.IsFinished = false;
                    break;
                case "today":
                    sess.StartTime = DateTime.Now.AddHours((durationHours - 1) * -1);
                    sess.IsFinished = true;
                    break;
                case "old":
                    sess.StartTime = DateTime.Now.AddDays(-2);
                    sess.IsFinished = true;
                    break;
                default:
                    return "typo in status. did nothing.";
            }

            // Add to DB & return
            using (var db = new kuberaDbContext())
            {
                // save
                db.Sessions.Add(sess);
                db.SaveChanges();

                // return added
                return db.Sessions
                    .Where(x => x.AccountId == accountId)
                    .OrderByDescending(x => x.Id)
                    .FirstOrDefault();
            }
        }

        [HttpGet]
        public string CreateSpoofJob()
        {
            return "/sessionId/scenarioIdentifier/" + Environment.NewLine +
                "Timetables inferred from session. Using full session duration";
        }
        [HttpGet("{sessionId}/{scenarioId?}")]
        public dynamic CreateSpoofJob(int sessionId, string scenarioIdentifier)
        {
            if (sessionId == 0)
                return "missing session id. Create it first";

            Session sess = Session.FromId(sessionId);
            ScenarioBase scen = ScenarioHelper.ByIdentifier(scenarioIdentifier);

            // Create job
            Job job = new Job()
            {
                SessionId = sessionId,
                ScenarioIdentifier = scenarioIdentifier,
                ForceRunUntilComplete = scen.AlwaysRunsUntilComplete,
                IsFinished = sess.IsFinished,
                StartTime = sess.StartTime,
                TargetDuration = sess.TargetDuration
            };

            // Add to DB & return
            using (var db = new kuberaDbContext())
            {
                // save
                db.Jobs.Add(job);
                db.SaveChanges();

                // return added
                return db.Jobs
                    .Where(x => x.SessionId == sess.Id)
                    .OrderByDescending(x => x.Id)
                    .FirstOrDefault();
            }
        }

        [HttpGet]
        public string WipeAllSessionsJobs()
        {
            return "This will DELETE ALL JOBS AND SESSIONS IN THE DB!" + Environment.NewLine +
                "Use arg 'yesimsureplsdeletealloftehthings' to run it.";
        }

        [HttpGet("{validationStr}")]
        public dynamic WipeAllSessionsJobs(string validationStr)
        {
            if (validationStr != "yesimsureplsdeletealloftehthings")
                return "Invalid validation string. Nothing was wiped.";

            try
            {
                // Delete all things
                using (var db = new kuberaDbContext())
                {
                    // Get & remove sessions
                    var allSessions = db.Sessions.ToList();
                    db.Sessions.RemoveRange(allSessions);


                    // Get & remove jobs
                    var allJobs = db.Jobs.ToList();
                    db.Jobs.RemoveRange(allJobs);

                    // write
                    db.SaveChanges();
                }
                return "Done. yeeted all the things.";
            }
            catch (Exception ex)
            {
                return "Failed: " + ex.Message;
            }
        }

        [HttpGet]
        public string WipeAllStoredAccountLevels()
        {
            return "This will DELETE ALL JOBS AND SESSIONS IN THE DB!" + Environment.NewLine +
                "Use arg 'yesimsureplsdeletealloftehthings' to run it.";
        }

        [HttpGet("{validationStr}")]
        public dynamic WipeAllStoredAccountLevels(string validationStr)
        {
            if (validationStr != "yesimsureplsdeletealloftehthings")
                return "Invalid validation string. Nothing was wiped.";

            try
            {
                // Delete all things
                using (var db = new kuberaDbContext())
                {
                    // Get & remove sessions
                    var allLevels = db.Levels.ToList();
                    db.Levels.RemoveRange(allLevels);

                    // write
                    db.SaveChanges();
                }
                return "Done. yeeted all the things.";
            }
            catch (Exception ex)
            {
                return "Failed: " + ex.Message;
            }
        }
    }
}
