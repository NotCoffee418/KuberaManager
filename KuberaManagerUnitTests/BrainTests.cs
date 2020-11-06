using KuberaManager.Logic.ScenarioLogic.Requirements;
using KuberaManager.Logic.ScenarioLogic.Scenarios;
using KuberaManager.Logic.ScenarioLogic.Scenarios.Assigners;
using KuberaManager.Models.Data;
using KuberaManager.Models.Database;
using KuberaManager.Models.Logic;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KuberaManagerUnitTests
{
    class BrainTests
    {
        [SetUp]
        public void Setup()
        {
            // Set db context to testing
            kuberaDbContext.IsUnitTesting = true;
        }

        [TearDown]
        public void TearDown()
        {
            using (var db = new kuberaDbContext())
            {
                db.Database.EnsureDeleted();
            }
        }

        [Test]
        public void ScheduledJanitor_NoData_NoExceptions()
        {
            Brain.ScheduledJanitor();
            Assert.Pass();
        }

        [Test]
        public void ScheduledJanitor_BorkedSession_NoExceptions()
        {
            // Create spoof data
            _TestHelper.DbCreateMockAccounts(1);
            _TestHelper.DbCreateMockSession(1, true);

            // Break the session
            Session sess = Session.FromId(1);
            using (var db = new kuberaDbContext())
            {
                db.Attach(sess);
                sess.IsFinished = false;
                db.SaveChanges();
            }

            // Run
            Brain.ScheduledJanitor();
            Assert.Pass();
        }

        [Test]
        public void ScheduledJanitor_UnclosedJob_NoExceptions()
        {
            // Create spoof data
            _TestHelper.DbCreateMockAccounts(1);
            _TestHelper.DbCreateMockSession(1, true);

            // Create job
            Session sess = Session.FromId(1);
            Job job = new Job()
            {
                IsFinished = false,
                ScenarioIdentifier = "Quest.TUTORIAL_ISLAND",
                StartTime = DateTime.Now.AddDays(-2),
                TargetDuration = TimeSpan.FromHours(1),
                ForceRunUntilComplete = false,
                SessionId = 1,
            };

            using (var db = new kuberaDbContext())
            {
                // Save job
                db.Attach(job);
                db.SaveChanges();

                // Bork session too
                db.Attach(sess);
                sess.IsFinished = false;
                db.SaveChanges();
            }

            // Retrieve job 
            job = Job.FromId(1);

            // Run
            Brain.ScheduledJanitor();
            Assert.NotNull(job);
        }


        [Test]
        public void GetRandomSessionJobDuration_HunderedRuns_ReasonableDeviation()
        {
            // Prepare data
            Config.Set<int>("MaxHoursPerDay", 1);
            _TestHelper.DbCreateMockAccounts(1, 0);
            Account acc = Account.FromId(1);

            // Test Random Session
            TimeSpan span = Brain.GetRandomSessionDuration(acc);
            Assert.Greater(span, TimeSpan.Zero);
            Assert.LessOrEqual(span, TimeSpan.FromHours(1));

            // Prepare random job test
            Session session = new Session()
            {
                Id = 1,
                AccountId = 1,
                IsFinished = false,
                StartTime = DateTime.Now,
                TargetDuration = TimeSpan.FromHours(1),
            };

            // run method
            int unreasonableResults = 0;
            TimeSpan min = TimeSpan.FromMinutes(5);
            TimeSpan max = TimeSpan.FromMinutes(59);
            for (int i = 0; i < 100; i++)
            {
                span = Brain.GetRandomJobDuration(session, QuestAssigner.ByName("COOKS_ASSISTANT")) ;
                if (span < min || span > max)
                    unreasonableResults++;
            }

            // Assert
            // Test may fail randomly as it relies on RNG
            Assert.LessOrEqual(unreasonableResults, 20);
        }

        [Test]
        public void DoesClientNeedJobUpdate_NeverHadJob_True()
        {
            // Prepare data
            Config.Set<int>("MaxHoursPerDay", 1);
            _TestHelper.DbCreateMockAccounts(1, 0);
            Account acc = Account.FromId(1);

            // Create ongoing session
            _TestHelper.DbCreateMockSession(1, true, DateTime.Now.AddHours(-1), TimeSpan.FromHours(5), isFinished: false);
            Session sess = Session.FromId(1);

            // Assert
            bool result = Brain.DoesClientNeedJobUpdate(sess);
            Assert.IsTrue(result);
        }

        [Test]
        public void DoesClientNeedJobUpdate_ExpiredJob_True()
        {
            // Prepare data
            Config.Set<int>("MaxHoursPerDay", 1);
            _TestHelper.DbCreateMockAccounts(1, 0);
            Account acc = Account.FromId(1);

            // Create ongoing session
            _TestHelper.DbCreateMockSession(1, true, DateTime.Now.AddHours(-1), TimeSpan.FromHours(5), isFinished: false);
            Session sess = Session.FromId(1);

            // Create job
            using (var db = new kuberaDbContext())
            {
                db.Jobs.Add(new Job()
                {
                    SessionId = 1,
                    IsFinished = true,
                    StartTime = DateTime.Now.AddHours(-5),
                    TargetDuration = TimeSpan.FromHours(2),
                });
                db.SaveChanges();
            }

            // Assert
            bool result = Brain.DoesClientNeedJobUpdate(sess);
            Assert.IsTrue(result);
        }

        [Test]
        public void DoesClientNeedJobUpdate_HasJob_False()
        {
            // Prepare data
            Config.Set<int>("MaxHoursPerDay", 1);
            _TestHelper.DbCreateMockAccounts(1, 0);
            Account acc = Account.FromId(1);

            // Create ongoing session
            _TestHelper.DbCreateMockSession(1, true, DateTime.Now.AddHours(-1), TimeSpan.FromHours(5), isFinished: false);
            Session sess = Session.FromId(1);

            // Create job
            using (var db = new kuberaDbContext())
            {
                db.Jobs.Add(new Job()
                {
                    SessionId = 1,
                    IsFinished = true,
                    StartTime = DateTime.Now.AddHours(-1),
                    TargetDuration = TimeSpan.FromHours(2),
                });
                db.SaveChanges();
            }

            // Assert
            bool result = Brain.DoesClientNeedJobUpdate(sess);
            Assert.IsTrue(result);
        }

        [Test]
        public void FindNewJob_FreshAccount_ExpectTutorial()
        {
            // Prep data
            _TestHelper.DbCreateMockAccounts(1);
            _TestHelper.DbCreateMockSession(1, false, DateTime.Now.AddMinutes(-5), TimeSpan.FromHours(3), false);
            Session sess = Session.FromId(1);

            // Get new job's data
            Job job = Brain.FindNewJob(sess);
            ScenarioBase scen = ScenarioHelper.ByIdentifier(job.ScenarioIdentifier);

            // Assert
            Assert.AreEqual("Quest", scen.ScenarioName);
            Assert.AreEqual("TUTORIAL_ISLAND", scen.ScenarioArgument);
        }

        [Test]
        public void FindNewJob_FreeToPlayTutorialComplete_NotMemberJobNotTutorial()
        {
            // Prep data
            _TestHelper.DbCreateMockAccounts(1);
            _TestHelper.DbCreateMockSession(1, false, DateTime.Now.AddMinutes(-5), TimeSpan.FromHours(3), false);
            Session sess = Session.FromId(1);

            // Add account definition
            Account acc = Account.FromId(1);
            acc.AddDefinition(CompletionDataDefinition.TutorialComplete);
            
            // Get new job's data
            Job job = Brain.FindNewJob(sess);
            ScenarioBase scen = ScenarioHelper.ByIdentifier(job.ScenarioIdentifier);

            // Assert
            Assert.NotNull(scen);
            Assert.AreNotEqual("TUTORIAL_ISLAND", scen.ScenarioArgument);
            Assert.IsFalse(scen.MembersOnly);
        }
    }
}
