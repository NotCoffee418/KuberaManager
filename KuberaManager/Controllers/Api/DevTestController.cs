using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KuberaManager.Models.Database;
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
    }
}
