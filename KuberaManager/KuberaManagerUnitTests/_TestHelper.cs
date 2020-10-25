using KuberaManager.Models.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace KuberaManagerUnitTests
{
    class _TestHelper
    {
        public static void DbCreateMockAccounts(int enabled = 10, int banned = 2)
        {
            kuberaDbContext.IsUnitTesting = true;
            using (var db = new kuberaDbContext())
            {
                // Regular accounts
                for (int i = 0; i < enabled; i++)
                {
                    int ident = i + 1;
                    Account acc = new Account()
                    {
                        Login = "testAccount" + ident + "@mockup.fake",
                        Password = "testPass" + ident,
                        IsEnabled = true,
                    };
                    db.Accounts.Add(acc);
                }

                // banned accs
                for (int i = 0; i < banned; i++)
                {
                    int ident = i + 1;
                    Account acc = new Account()
                    {
                        Login = "testBannedAccount" + ident + "@mockup.fake",
                        Password = "testBannedPass" + ident,
                        IsEnabled = true,
                        IsBanned = true,
                    };
                    db.Accounts.Add(acc);
                }

                // Save
                db.SaveChanges();
            }
            
        }


        internal static void DbCreateMockSession(int acctId, bool wasToday, DateTime? overrideStartTime = null, TimeSpan? overrideDuration = null, bool isFinished = true)
        {
            kuberaDbContext.IsUnitTesting = true;

            // define startTime
            DateTime startTime;
            if (overrideStartTime.HasValue)
                startTime = overrideStartTime.Value;
            else
            {
                if (wasToday)
                    startTime = DateTime.Now.AddHours(-12);
                else startTime = DateTime.Now.AddDays(-2);
            }

            // Create
            using (var db = new kuberaDbContext())
            {
                db.Sessions.Add(new Session()
                {
                    AccountId = acctId,
                    ActiveComputer = 1,
                    TargetDuration = overrideDuration.HasValue ? overrideDuration.Value : TimeSpan.FromHours(1),
                    StartTime = startTime,
                    IsFinished = isFinished,
                });

                // Save
                db.SaveChanges();
            }
        }
    }
}
