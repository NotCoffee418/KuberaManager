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

            // Launch the session
            ClientManager.StartClient(account, computer);

            // Background job to get session tag on launch
            BackgroundJobs.Launch_FindRspeerSessionTag(account);
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
                var borkJobs = db.Jobs
                    .Where(x => !x.IsFinished)
                    .Where(x => x.ForceRunUntilComplete)
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

        public static ScenarioBase DetermineScenario(Session sess)
        {
            throw new NotImplementedException();
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
        public static TimeSpan GetRandomJobDuration(Account account, Session session, ScenarioBase scenario)
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
        internal static bool DoesClientNeedJobUpdate(Session session)
        {
            throw new NotImplementedException(); // logic check
            Job currentJob = session.FindCurrentJob();
            if (currentJob == null)
                return true;
            else return currentJob.ShouldStop();
        }

        // pseudo:
        // Check Account.ContinueScenario prio, run it and NULL it if exist
        // else Figure out new job
        internal static Job FindNewJob(Session sess)
        {
            throw new NotImplementedException();
        }

        // input args etc need to be defined
        // false means we can't do it for some reason
        // Should be suitable for automatic and manual session starting.
        // Any missing information eeds to be calculated in another function
        public static bool StartNewClient(int accountId, string selectedScenario, int computerId, TimeSpan targetDuration, bool isManualSession = false)
        {
            try
            {
                // Get data from input
                Account account = Account.FromId(accountId);
                ScenarioBase scenario = ScenarioHelper.ByIdentifier(selectedScenario);
                Computer computer = Computer.ById(computerId);

                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message); // logger me
                return false;
            }
        }
    }
}
