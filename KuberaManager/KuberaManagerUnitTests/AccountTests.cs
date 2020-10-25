using KuberaManager.Models.Database;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KuberaManagerUnitTests
{
    class AccountTests
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
        public void FromLogin_ValidLogin_ExcpectAccount()
        {
            // Create test accounts.
            _TestHelper.DbCreateMockAccounts();

            // testAccount
            Account acct = Account.FromLogin("testAccount1@mockup.fake");

            //validate result
            Assert.IsNotNull(acct);
            Assert.AreEqual("testAccount1@mockup.fake", acct.Login);
        }

        [Test]
        public void GetTodayPlayedTime_FourSessionsToday_ExpectFourHours()
        {
            // Create test accounts.
            _TestHelper.DbCreateMockAccounts();

            // Create mock sessions
            for (int i = 0; i < 4; i++)
                _TestHelper.DbCreateMockSession(1, true);

            // And an older one 
            _TestHelper.DbCreateMockSession(1, wasToday: false);

            // Grab account
            Account acc;
            using (var db = new kuberaDbContext())
            {
                var fucc = db.Accounts.Count();
                acc = db.Accounts.Where(x => x.Id == 1).FirstOrDefault();
            }

            // Test result
            Assert.NotNull(acc);
            Assert.AreEqual(4, acc.GetTodayPlayedTime().TotalHours);
        }

        [Test]
        public void GetTodayPlayedTime_NotPlayedToday_ZeroHours()
        {
            // Create test accounts.
            _TestHelper.DbCreateMockAccounts();

            // Create outdated session
            _TestHelper.DbCreateMockSession(1, wasToday: false);

            // Grab account
            Account acc;
            using (var db = new kuberaDbContext())
            {
                var fucc = db.Accounts.Count();
                acc = db.Accounts.Where(x => x.Id == 1).FirstOrDefault();
            }


            // Test result
            Assert.NotNull(acc);
            Assert.NotNull(acc.GetTodayPlayedTime());
            Assert.AreEqual(0, acc.GetTodayPlayedTime().TotalHours);
        }

        [Test]
        public void GetAvailableAccount_NoAccountsExist_ExpectNull()
        {
            _TestHelper.DbCreateMockAccounts(0,0);
            Account acc = Account.GetAvailableAccount();
            Assert.IsNull(acc);
        }
        public void GetAvailableAccount_AllAccountsBusy_ExpectNull()
        {
            throw new NotImplementedException();
        }
        public void GetAvailableAccount_AccountAvailable_ExpectAccount()
        {
            throw new NotImplementedException();
        }
    }
}
