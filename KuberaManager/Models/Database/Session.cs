using KuberaManager.Models.Logic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace KuberaManager.Models.Database
{
    public class Session
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Account")]
        public int AccountId { get; set; }

        [AllowNull]
        public string RspeerSessionTag { get; set; }

        [AllowNull]
        [ForeignKey("Computer")]
        public int ActiveComputer { get; set; }

        [Required]
        public TimeSpan TargetDuration { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [AllowNull]
        public DateTime LastUpdateTime { get; set; }

        [DefaultValue(true)]
        public bool IsFinished { get; set; }

        #region Static
        /// <summary>
        /// Find Session based on RS login email
        /// </summary>
        /// <param name="runescapeaccount"></param>
        /// <returns>null or session</returns>
        public static Session FromAccount(string runescapeaccount, bool createNewAllowed = true)
        {
            // Get runescape account
            Account account = Account.FromLogin(runescapeaccount);

            // Attempt to find existing session or create new one
            using (var db = new kuberaDbContext())
            {
                Session foundSession = db.Sessions
                    .Where(x => x.IsFinished == false)
                    .Where(x => x.AccountId == account.Id)
                    .Where(x => x.StartTime.Add(x.TargetDuration) > DateTime.Now) // session must not be past target duration
                    .Where(x => x.LastUpdateTime > DateTime.Now.AddMinutes(-10)) // last update must be less than 10 minutes
                    .OrderByDescending(x => x.LastUpdateTime)
                    .FirstOrDefault();

                // if session is known, change LastUpdateTime and return
                if (foundSession != null)
                {
                    foundSession.LastUpdateTime = DateTime.Now;
                    db.SaveChanges();
                    return foundSession;
                }
            }
#warning move this to it's own thing and return null here always.
            /// We're still here. Session was not found. Create new one & return.
            if (!createNewAllowed || kuberaDbContext.IsUnitTesting)
                return null; // Can't unittest ClientManager stuff

            // Determine Get relevant session data
            var sessionCd = ClientManager.GetConnectedClients()
                .Where(x => x.runescapeEmail.ToLower() == runescapeaccount.ToLower())
                .FirstOrDefault();
            if (sessionCd == null)
                return null;

            // Determine relevant computer
            var computerKvp = ClientManager.GetConnectedComputers()
                .Where(x => x.Value.host.ToLower() == sessionCd.machineName.ToLower())
                .FirstOrDefault();

            // Prepare new session var
            Session sess = new Session()
            {
                AccountId = account.Id,
                IsFinished = false,
                StartTime = DateTime.Now,
                LastUpdateTime = DateTime.Now,
                TargetDuration = Brain.GetRandomSessionDuration(account),
                RspeerSessionTag = sessionCd.tag,
                ActiveComputer = Computer.ByHostname(sessionCd.machineName).Id
            };
            
            // Save new session to database & return
            using (var db = new kuberaDbContext())
            {
                db.Sessions.Add(sess);
                db.SaveChanges();
            }

            return sess;
        }

        public static Session FromId(int sessionId)
        {
            using (var db = new kuberaDbContext())
            {
                return db.Sessions
                    .Where(x => x.Id == sessionId)
                    .FirstOrDefault();
            }
        }

        public  void ReportFinished()
        {
            using (var db = new kuberaDbContext())
            {
                Session dbSess = db.Sessions
                    .Where(x => x.Id == this.Id)
                    .FirstOrDefault();
                if (dbSess != null)
                {
                    dbSess.IsFinished = true;
                    db.SaveChanges();
                }
            }
        }

        public void ReportHeartbeat()
        {
            using (var db = new kuberaDbContext())
            {
                this.LastUpdateTime = DateTime.Now;
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Gets the duration of the session until now.
        /// Best estimate for actual session length.
        /// </summary>
        public TimeSpan GetDuration()
        {
            DateTime endTime = LastUpdateTime;            
            if (LastUpdateTime == default(DateTime))
            {
                // Session never had a heartbeat but is expired.
                // Assume session never actually ran!
                if (StartTime.Add(TargetDuration) < DateTime.Now)
                    return TimeSpan.Zero;

                // Session is so new it has no heartbeat yet
                else endTime = DateTime.Now;
            }
                

            // If session never reported a heartbeat but is expired

            return endTime.Subtract(StartTime);
        }
        #endregion

        #region Non-static

        /// <summary>
        /// Determines if session should stop.
        /// Handles marking IsFinished and storing Account.ContinueScenario.
        /// </summary>
        /// <returns></returns>
        public bool ShouldStop()
        {
            // Keep going, do nothing.
            bool isPastExpTime = StartTime.Add(TargetDuration) < DateTime.Now;
            if (!isPastExpTime)
                return false;

            // watch?v=W6oQUDFV2C0
            if (IsFinished)
                return true;

            // Update job & account status
            Job currentJob = FindCurrentJob();
            if (currentJob != null)
            {
                // Store current job to be continued on next login if needed
                if (currentJob.ForceRunUntilComplete)
                {
                    Account acc = Account.FromId(AccountId);
                    using (var db = new kuberaDbContext())
                    {
                        db.Attach(acc);
                        acc.ContinueScenario = currentJob.ScenarioIdentifier;
                        db.SaveChanges();
                    }
                }

                // Mark JOB as finished
                using (var db = new kuberaDbContext())
                {
                    db.Attach(currentJob);
                    currentJob.IsFinished = true;
                    db.SaveChanges();
                }
            }

            // Mark SESSION as finished
            using (var db = new kuberaDbContext())
            {
                db.Attach(this);
                this.IsFinished = true;
                db.SaveChanges();
            }

            // Report that we should stop
            return true;
        }

        internal void SaveCurrentAction(string text)
        {
            EventLog.AddEntry(this.Id, text);
        }

        public Job FindCurrentJob()
        {
            using (var db = new kuberaDbContext())
            {
                return db.Jobs
                    .Where(x => x.SessionId == Id)
                    .Where(x => x.IsFinished == false)
                    .OrderByDescending(x => x.Id)
                    .FirstOrDefault();
            }
        }
        #endregion
    }
}
