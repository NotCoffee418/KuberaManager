using KuberaManager.Models.Data;
using KuberaManager.Models.Logic;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace KuberaManager.Models.Database
{
    public class Account
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Login { get; set; }

        [AllowNull]
        [PasswordPropertyText]
        public string Password { get; set; }

        [Required]
        [DefaultValue(false)]
        public bool IsMember { get; set; }

        [Required]
        [DefaultValue(true)]
        public bool IsEnabled { get; set; }

        [Required]
        [DefaultValue(false)]
        public bool IsBanned { get; set; }

        [Range(0, 23)]
        [DefaultValue(0)]
        [Display(Name = "Start Time of Day", Description = "Preferred time of the day to start 0-23. Logic is that an account should have consistent playing habits.")]
        public int PrefStartTimeDay { get; set; } = 0;

        [Range(0, 23)]
        [DefaultValue(23)]
        [Display(Name = "Stop Time of Day", Description = "Preferred time of the day to stop 0-23. Logic is that an account should have consistent playing habits.")]
        public int PrefStopTimeDay { get; set; } = 23;

        /// <summary>
        /// returns and creates account if not exist
        /// </summary>
        /// <param name="runescapeaccount"></param>
        /// <returns></returns>
        public static Account FromLogin(string runescapeaccount)
        {
            Account result = null;
            bool newCreated = false;
            using (var db = new kuberaDbContext())
            {
                result = db.Accounts
                    .Where(x => x.Login == runescapeaccount)
                    .FirstOrDefault();
                if (result == null)
                {
                    // Account was manually opened. Adding to database without password
                    result = new Account()
                    {
                        Login = runescapeaccount,
                        IsEnabled = false,
                    };
                    db.Accounts.Add(result);
                }
            }

            // Report to discord
            if (newCreated)
                DiscordHandler.PostMessage($"New account '{runescapeaccount}' detected. You must manually define the password & enabled before Brain will assign tasks to it.");

            return result;
        }

        /// <summary>
        /// Returns account or null from ID. No creating.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Account FromId(int id)
        {
            using (var db = new kuberaDbContext())
            {
                return db.Accounts
                    .Where(x => x.Id == id)
                    .FirstOrDefault();
            }
        }


        /// <summary>
        /// Gets an available account, keeping in mind PrefStartTimeDay 
        /// </summary>
        /// <returns></returns>
        public static Account GetAvailableAccount()
        {
            // Define maxTimePerDayPerAcc from config
            TimeSpan maxTimePerDayPerAcc = TimeSpan.FromHours(Config.Get<int>("MaxHoursPerDay"));
            if (maxTimePerDayPerAcc == TimeSpan.FromSeconds(0))
                maxTimePerDayPerAcc = TimeSpan.FromHours(25);

            // Find eligible account
            using (var db = new kuberaDbContext())
            {
                var result = db.Accounts
                    // Enabled and not banned
                    .Where(x => !x.IsBanned && x.IsEnabled)

                    // Password is defined
                    .Where(x => x.Password != null && x.Password != "")

                    // Within preferred time
                    .Where(x => x.PrefStartTimeDay <= DateTime.Now.Hour)
                    //.Where(x => x.PrefStopTimeDay >= DateTime.Now.Hour)

                    // Complete DB query
                    .ToList();

                return result // Continue outside of db
                    // Isn't currently playing
                    .Where(x => x.GetActiveSession() == null)

                    // Hasn't reached the maximum allocated time for the day
                    .Where(x => x.GetTodayPlayedTime() < maxTimePerDayPerAcc)

                    // Prefer accounts with narrowed down playable timespans
                    .OrderBy(x => x.PrefStopTimeDay - x.PrefStartTimeDay)

                    // Prefer least played account today
                    .ThenBy(x => x.GetTodayPlayedTime())

                    .ToList() // Required for some reason
                    .FirstOrDefault();
            }
        }



        /// <summary>
        /// Stored since it's it's an intensive operation 
        /// and is called twice by GetAvailableAccount()
        /// </summary>
        private Nullable<TimeSpan> _todayPlayedTime = null;
        public TimeSpan GetTodayPlayedTime()
        {
            // Get value if already calculated
            if (_todayPlayedTime.HasValue)
                return _todayPlayedTime.Value;


            // one day var
            DateTime dayAgo = DateTime.Now.AddDays(-1);

            double totalSeconds = 0;
            using (var db = new kuberaDbContext())
            {
                /// Calculate time played from finished sessions
                totalSeconds = db.Sessions
                    .Where(x => x.AccountId == this.Id)

                    // Only include finished sessions in first check
                    .Where(x => x.IsFinished)

                    // if session started within the past day
                    .Where(x => x.StartTime > dayAgo)

                    // Sum of relevant durations
                    .Sum(x => x.TargetDuration.TotalSeconds);


                /// Append session that was ongoing 24 hours ago
                var possiblyOngoingYday = db.Sessions
                    .Where(x => x.AccountId == this.Id)
                    .Where(x => x.IsFinished)

                    // Grab first session based on END time
                    .Where(x => x.StartTime.Add(x.TargetDuration) > dayAgo)
                    .OrderBy(x => x.StartTime)
                    .FirstOrDefault();

                // Only if start time wasn't already included in previous calculation
                if (possiblyOngoingYday != null && possiblyOngoingYday.StartTime < dayAgo)
                {
                    // Amount of time that should be subtracted from the target duration
                    TimeSpan offset = dayAgo.Subtract(possiblyOngoingYday.StartTime);

                    // Append the result to totalSeconds
                    totalSeconds += possiblyOngoingYday.TargetDuration.Subtract(offset).TotalSeconds;
                }

                /// Append active session if any
                Session activeSession = GetActiveSession();
                if (activeSession != null)
                {
                    // Add time since we started to totalSeconds
                    totalSeconds += DateTime.Now.Subtract(activeSession.StartTime).TotalSeconds;
                }
            }

            // Store in memory & return
            _todayPlayedTime = TimeSpan.FromSeconds(totalSeconds);
            return _todayPlayedTime.Value;
        }

        /// <summary>
        /// If this account has a session not flagged as IsFinished
        /// </summary>
        /// <returns></returns>
        public Session GetActiveSession()
        {
            using (var db = new kuberaDbContext())
            {
                return db.Sessions
                    .Where(x => x.AccountId == this.Id)
                    .Where(x => !x.IsFinished)
                    .OrderByDescending(x => x.StartTime) // Should only be one but bugs exist
                    .FirstOrDefault();
            }
        }

        /// <summary>
        /// Gets known levels. Nullable
        /// </summary>
        /// <returns></returns>
        public Levels GetLevels()
        {
            return Levels.FromAccount(Id);
        }

        /// <summary>
        /// Return all of this account's definitions
        /// </summary>
        /// <returns></returns>
        public List<CompletionDataDefinition> GetDefinitions()
        {
            return AccountCompletionData.GetDefinitions(Id);
        }

        public bool HasDefinition(CompletionDataDefinition def)
        {
            return AccountCompletionData.AccountHasDefinition(Id, def);
        }

        public void AddDefinition(CompletionDataDefinition def)
        {
            AccountCompletionData.AddDefinition(Id, def);
        }
    }
}
