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
        public static Session FromAccount(string runescapeaccount)
        {
            // Get runescape account
            Account account = Account.FromLogin(runescapeaccount);

            // Attempt to find existing session or create new one
            using (var db = new kuberaDbContext())
            {
                Session foundSession = db.Sessions
                    .Where(x => x.IsFinished == false)
                    .Where(x => x.AccountId == account.Id)
                    .Where(x => x.LastUpdateTime < DateTime.Now.AddHours(-1)) // last update must be less than an hour old.
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

            /// We're still here. Session was not found. Create new one & return.
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
                TargetDuration = Brain.GetTargetDuration(account),
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

        internal void ReportFinished()
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

        #endregion

        #region Non-static
        public bool ShouldStop()
        {
            return StartTime + TargetDuration > DateTime.Now;
        }

        #endregion
    }
}
