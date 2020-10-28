using KuberaManager.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KuberaManager.Models.Data.Ajax
{
    public class BotStatus
    {
        public bool IsRunning { get; set; } = false;
        public string Account { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime StopTime { get; set; }
        public int PercentageComplete { 
            get
            {
                // 0 if inactive session
                if (StartTime == default(DateTime) || StopTime == default(DateTime))
                    return 0;

                // Calculate the result
                double total = StopTime.Subtract(StartTime).TotalSeconds;
                double elapsed = StopTime.Subtract(DateTime.Now).TotalSeconds;
                double percentage = (elapsed / total) * 100;
                return (int)Math.Round(percentage);
            }
        }
        public string ActiveJob { get; set; }
        public string Host { get; set; }

        public static List<BotStatus> GetAllBotStatus()
        {
            List<BotStatus> result = new List<BotStatus>();
            using (var db = new kuberaDbContext())
            {
                // Find eligible accounts
                var relevantAccts = db.Accounts
                    .Where(x => !x.IsBanned && x.IsEnabled && x.Password != null && x.Password != "")
                    .ToList();

                // Get currently active sessions
                var currSessions = db.Sessions
                    .Where(x => x.IsFinished == false)
                    .ToList();

                // Gather additional data
                foreach (Account acc in relevantAccts)
                {
                    // Prepare row
                    BotStatus bs = new BotStatus();
                    bs.Account = acc.Login;

                    // Grab account's session if any
                    var accSess = currSessions
                        .Where(x => x.AccountId == acc.Id)
                        .FirstOrDefault();
                    if (accSess != null)
                    {
                        // Add session data
                        bs.IsRunning = true;
                        bs.StartTime = accSess.StartTime;
                        bs.StopTime = accSess.StartTime.Add(accSess.TargetDuration);

                        // Add computer data
                        Computer comp = Computer.ById(accSess.ActiveComputer);
                        if (comp != null)
                            bs.Host = comp.Hostname;

                        // Add job data
                        Job job = accSess.FindCurrentJob();
                        bs.ActiveJob = job.ActiveScenarioObj.Name;
                    }

                    result.Add(bs);
                }
            }
            return result;
        }
    }
}
