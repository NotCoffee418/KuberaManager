using KuberaManager.Hangfire;
using KuberaManager.Helpers;
using KuberaManager.Models.Database;
using KuberaManager.Models.Logic.ScenarioLogic.Scenarios;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KuberaManager.Models.Logic
{
    public class Brain
    {
        /// <summary>
        /// Called minutely, starts new sessions if needed
        /// </summary>
        internal static void ScheduledSessionStarter()
        {
            // Do nothing if brain is not enabled
            if (!Config.Get<bool>("BrainEnabled"))
                return;

            // Determine if we have a free slot, return if not
            Computer computer = Computer.GetAvailableComputer();
            if (computer == null)
                return;

            // Get available account, return if none
            Account account = Account.GetAvailableAccount();
            if (account == null)
                return;

            // Create a new session with the gathered info.
            Session.Create(account, computer);
            Session session = account.GetActiveSession();

            // Launch the session
            ClientManager.StartClient(account, computer, session);

            // Background job to get session tag on launch
            BackgroundJobs.Launch_FindRspeerSessionTag(account);
        }

        // input args etc need to be defined
        // false means we can't do it for some reason
        public static void ManualSessionStarter(int accountId, string selectedScenario, int computerId)
        {
            // Get data from input
            Account account = Account.FromId(accountId);
            ScenarioBase scenario = ScenarioHelper.ByIdentifier(selectedScenario);
            Computer computer = Computer.ById(computerId);

            // Create a new session with the gathered info.
            Session.Create(account, computer);

            // Launch the session
            ClientManager.StartClient(account, computer, session:null, isManualSession: true);

            // Background job to get session tag on launch
            BackgroundJobs.Launch_FindRspeerSessionTag(account);

            // catch & logger me
        }

        /// <summary>
        /// Lazy bork check. Report on discord when sessions or jobs don't close correctly + close them.
        /// </summary>
        /// <param name="acc"></param>
        /// <returns></returns>
        public static void ScheduledJanitor()
        {
            using (var db = new kuberaDbContext())
            {
                // Find borked sessions
                var borkSession = db.Sessions
                    .Where(x => !x.IsFinished)
                    .Where(x => x.LastUpdateTime < DateTime.Now.AddHours(-2))
                    .ToList();

                // Find bork jobs
                var selectJobs = db.Jobs
                    .Where(x => !x.IsFinished)
                    .Where(x => !x.ForceRunUntilComplete)
                    .ToList();

                var borkJobs = selectJobs
                    .Where(x => x.StartTime.Add(x.TargetDuration) < DateTime.Now.AddHours(-2))
                    .ToList();

                // Warn discord TTS
                if (borkSession.Count() > 0 || borkJobs.Count() > 0)
                    DiscordHandler.PostMessage("Manager: Founds sessions that didn't close correctly.", tts:true);
                
                // No problems, go home
                else return;

                // Kill sessions
                foreach (var sess in borkSession)
                {
                    db.Attach(sess);
                    sess.IsFinished = true;
                    db.SaveChanges();

                    DiscordHandler.PostMessage($"Session: {sess.Id} Account: {sess.AccountId}");
                }

                // Kill jobs
                foreach (var job in borkJobs)
                {
                    db.Attach(job);
                    job.IsFinished = true;
                    db.SaveChanges();

                    DiscordHandler.PostMessage($"Session: {job.SessionId} Job: {job.Id}");
                }
            }
        }

        /// <summary>
        /// Gets duration for a new session
        /// </summary>
        /// <param name="account"></param>
        /// <param name="scenario"></param>
        /// <returns></returns>
        public static TimeSpan GetRandomSessionDuration(Account account)
        {
            // Hardcoded min/max session duration
            TimeSpan minTime = TimeSpan.FromMinutes(30);
            TimeSpan maxTime = TimeSpan.FromHours(3);

            // Get max time from config & Subtract time account already played today
            TimeSpan todayTimeLeft = TimeSpan.FromHours(Config.Get<int>("MaxHoursPerDay"))
                .Subtract(account.GetTodayPlayedTime());

            // Don't bother
            if (todayTimeLeft < minTime)
                return TimeSpan.Zero;

            // Reduce max time to whatever we have left
            if (todayTimeLeft < maxTime)
                maxTime = todayTimeLeft;

            // Determine random duration
            int minSeconds = Convert.ToInt32(minTime.TotalSeconds);
            int maxSeconds = Convert.ToInt32(maxTime.TotalSeconds);
            int randSeconds = RandomHelper.GetRandom(minSeconds, maxSeconds);

            // return result
            return TimeSpan.FromSeconds(randSeconds);
        }

        /// <summary>
        /// Gets duration for a new job
        /// </summary>
        /// <param name="account"></param>
        /// <param name="session"></param>
        /// <param name="scenario"></param>
        /// <returns>Zero means exit</returns>
        public static TimeSpan GetRandomJobDuration(Session session, ScenarioBase scenario)
        {
            // Determine remaining time in session
            TimeSpan maxTime = session.StartTime.Add(session.TargetDuration).Subtract(DateTime.Now);

            // Don't bother if time limit is nearly reached
            if (maxTime.TotalMinutes < 5 || maxTime.TotalMinutes < scenario.MinimumRunTime.TotalMinutes)
                return TimeSpan.Zero;

            // Factor in scenario prefered timespan
            if (scenario.MaximumRunTime.TotalMinutes < maxTime.TotalMinutes)
                maxTime = scenario.MaximumRunTime;

            // Determine random duration
            int minSeconds = Convert.ToInt32(scenario.MinimumRunTime.TotalSeconds);
            int maxSeconds = Convert.ToInt32(maxTime.TotalSeconds);
            int randSeconds = RandomHelper.GetRandom(minSeconds, maxSeconds);

            // Return random timespan within defined parameters
            // todo: logger me
            return TimeSpan.FromSeconds(randSeconds);
        }

        /// <summary>
        /// Closes job if needed. Returns status
        /// </summary>
        /// <param name="sess"></param>
        /// <returns></returns>
        public static bool DoesClientNeedJobUpdate(Session session)
        {
            Job currentJob = session.FindCurrentJob();
            if (currentJob == null)
                return true;
            else return currentJob.ShouldStop();
        }

        // pseudo:
        // Check Account.ContinueScenario prio, run it and NULL it if exist
        // else Figure out new job
        public static Job FindNewJob(Session sess)
        {
            // Prepare
            Account account = Account.FromId(sess.AccountId);

            // Determine scenario & duration
            ScenarioBase scenario = DetermineNextScenario(sess, account);
            TimeSpan jobDuration = GetRandomJobDuration(sess, scenario);

            // Create new job with gathered data
            using (var db = new kuberaDbContext())
            {
                // Create job
                db.Jobs.Add(new Job()
                {
                    SessionId = sess.Id,
                    ScenarioIdentifier = scenario.Identifier,
                    StartTime = DateTime.Now,
                    TargetDuration = jobDuration,
                    ForceRunUntilComplete = scenario.AlwaysRunsUntilComplete,
                    IsFinished = false
                });
                db.SaveChanges();

                // Retrieve & return it
                return db.Jobs
                    .Where(x => x.IsFinished == false)
                    .Where(x => x.SessionId == sess.Id)
                    .OrderByDescending(x => x.Id)
                    .FirstOrDefault();
            }
        }


        public static ScenarioBase DetermineNextScenario(Session session, Account account)
        {
            // Continue job if remembered for account
            string continueScenario = account.ContinueScenario;
            if (continueScenario != null && continueScenario != "")
            {
                // Stop remembering it regardless of validity
                using (var db = new kuberaDbContext())
                {
                    db.Attach(account);
                    account.ContinueScenario = null;
                    db.SaveChanges();
                }

                // Not null but invalid
                if (continueScenario == null)
                {
                    DiscordHandler.PostMessage($"Trying to load remembered scenario '{account.ContinueScenario}' as remembered by '{account.Login}' but scenario doesn't exist (anymore?). Cancelling.");
                    throw new Exception($"Failed to load invalid remembered scenario '{account.ContinueScenario}' for '{account.Login}'.");
                }

                // Return valid result
                return ScenarioHelper.ByIdentifier(continueScenario);
            }

            // No remembered scenario.
            // Determine which new scenario to run by running
            return ScenarioHelper.FindViableScenario(account);
        }
    }
}
