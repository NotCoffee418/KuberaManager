using KuberaManager.Logic;
using KuberaManager.Models.Database;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

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
        public void FromLogin_ValidLoginMismatchedCases_ExcpectAccount()
        {
            // Create test accounts.
            _TestHelper.DbCreateMockAccounts(1);

            // Get acct with abnormal cases
            Account acct = Account.FromLogin("TeStAcCoUnT1@mockup.faKE");

            //validate result
            Assert.IsNotNull(acct);
            Assert.AreEqual("testAccount1@mockup.fake".ToLower(), acct.Login.ToLower());
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
        public void GetActiveSession_HasActiveSession_ExpectSession()
        {
            // create mock account
            _TestHelper.DbCreateMockAccounts(1);
            
            // Create ongoing session
            _TestHelper.DbCreateMockSession(1, true, DateTime.Now.AddHours(-1), TimeSpan.FromHours(2), isFinished:false);

            // Find thee account
            Account acct = Account.FromLogin("testAccount1@mockup.fake");

            //Get active session
            Session sess = acct.GetActiveSession();

            // Assert
            Assert.NotNull(sess);
            Assert.IsInstanceOf<Session>(sess);
        }

        [Test]
        public void GetAvailableAccount_NoAccountsExist_ExpectNull()
        {
            _TestHelper.DbCreateMockAccounts(0, 0);
            Account acc = Account.GetAvailableAccount();
            Assert.IsNull(acc);
        }

        [Test]
        public void GetAvailableAccount_AllAccountsBanned_ExpectNull()
        {
            _TestHelper.DbCreateMockAccounts(0, 2);
            Account acc = Account.GetAvailableAccount();
            Assert.IsNull(acc);
        }

        [Test]
        public void GetAvailableAccount_AllAccountsBusy_ExpectNull()
        {
            // Create mock account
            _TestHelper.DbCreateMockAccounts(1, 0);
            // Create ongoing session
            _TestHelper.DbCreateMockSession(1, true, DateTime.Now.AddHours(-1), TimeSpan.FromHours(2), isFinished:false);
            // Set max hours per day
            ConfigHelper ch = new ConfigHelper();
            ch.Set<int>("MaxHoursPerDay", 8);


            // TEZSTDEL
            Account accTEE = Account.FromLogin("testAccount1@mockup.fake");

            // All accounts should be busy
            Account acc = Account.GetAvailableAccount();
            Assert.IsNull(acc);
        }

        [Test]
        public void GetAvailableAccount_AccountAvailable_ExpectAccount()
        {
            // Variables
            _TestHelper.DbCreateMockAccounts(2,0);
            ConfigHelper ch = new ConfigHelper();
            ch.Set<int>("MaxHoursPerDay", 8);

            // Assert
            Account acc = Account.GetAvailableAccount();
            Assert.NotNull(acc);
            Assert.IsInstanceOf(typeof(Account), acc);
        }

        [Test]
        public void GetAvailableAccount_AccountAvailableUnlimitedHoursPerDay_ExpectAccount()
        {
            // Variables
            _TestHelper.DbCreateMockAccounts(2, 0);
            ConfigHelper ch = new ConfigHelper();
            ch.Set<int>("MaxHoursPerDay", 0);

            // Assert
            Account acc = Account.GetAvailableAccount();
            Assert.IsInstanceOf(typeof(Account), acc);
        }
    }
}
