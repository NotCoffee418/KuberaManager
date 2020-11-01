using KuberaManager.Models.Database;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KuberaManagerUnitTests
{
    class SessionTests
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
        public void FromAccount_ActiveSessionOnly_ValidSession()
        {
            // Prepare account
            _TestHelper.DbCreateMockAccounts(1);
            Account acc = Account.FromLogin("testAccount1@mockup.fake");

            // Prepare session
            _TestHelper.DbCreateMockSession(acc.Id, false, DateTime.Now.AddHours(-1), TimeSpan.FromHours(2), false);
            Session sess = Session.FromAccount(acc.Login);

            // assert
            Assert.IsNotNull(sess);
            Assert.AreEqual(acc.Id, sess.AccountId);
            Assert.Less(sess.StartTime, DateTime.Now.AddMinutes(-2)); // Not auto created.
        }

        [Test]
        public void FromAccount_ExpiredSessionButNotIsFinished_ShouldCreateNewButNullForUnitTestsBecauseRspeerApi()
        {
            // Prepare account
            _TestHelper.DbCreateMockAccounts(1);
            Account acc = Account.FromLogin("testAccount1@mockup.fake");

            // Prepare session
            _TestHelper.DbCreateMockSession(acc.Id, wasToday: true, isFinished: false); // expired session
            Session sess = Session.FromAccount(acc.Login);

            // Validate test conditions
            Assert.IsNull(sess);
        }

        [Test]
        public void ReportFinished_ExpiredSessionNotMarkedAsFinished_SetFinished()
        {
            // Prepare account
            _TestHelper.DbCreateMockAccounts(1);
            Account acc = Account.FromLogin("testAccount1@mockup.fake");

            // Prepare session
            _TestHelper.DbCreateMockSession(acc.Id, wasToday: true, isFinished: false); // expired session
            Session sess = Session.FromId(1);

            // Run
            sess.ReportFinished();

            // Assert
            sess = Session.FromId(1); // update
            Assert.IsTrue(sess.IsFinished);
        }

        [Test]
        public void GetDuration_LastUpdateTimeIsNullButIsExpired_SessionNeverHappened()
        {
            // Prepare account
            _TestHelper.DbCreateMockAccounts(1);
            Account acc = Account.FromLogin("testAccount1@mockup.fake");

            // Prepare session
            _TestHelper.DbCreateMockSession(acc.Id, wasToday: false, DateTime.Now.AddMinutes(-10), TimeSpan.FromMinutes(5), isFinished: true); // expired session
            using (var db = new kuberaDbContext())
            {
                db.Sessions.First().LastUpdateTime = default(DateTime);
                db.SaveChanges();
            }

            // Retrieve
            Session sess = Session.FromId(1);
            var duration = sess.GetDuration();

            // Assert
            Assert.AreEqual(TimeSpan.Zero, duration);
        }
    }
}
