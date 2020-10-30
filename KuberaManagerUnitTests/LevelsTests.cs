using KuberaManager.Models.Database;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace KuberaManagerUnitTests
{
    class LevelsTests
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
        public void Save_OneAccount_CanRetrieve()
        {
            // Create objects
            _TestHelper.DbCreateMockAccounts(1);
            Levels lev = new Levels()
            {
                AccountId = 1,
                Smithing = 12
            };

            // Save
            lev.Save();

            // Recover
            Account acc = Account.FromId(1);
            Levels calledLevels = acc.GetLevels();

            // Assert
            Assert.IsNotNull(calledLevels);
            Assert.AreEqual(1, calledLevels.AccountId);
            Assert.AreEqual(12, calledLevels.Smithing);
        }

        [Test]
        public void Save_OneAccountMultipleUpdates_NotSpamNewRows()
        {
            // Create account
            _TestHelper.DbCreateMockAccounts(1);

            // Insert
            Levels lev1 = new Levels()
            {
                AccountId = 1,
                Smithing = 12
            };
            lev1.Save();

            // Update
            Levels lev2 = new Levels()
            {
                AccountId = 1,
                Smithing = 13
            };
            lev2.Save();

            // Assert
            // No second row was added
            Assert.IsNull(Levels.FromAccount(2));

            // Row was successfully updated
            Assert.AreEqual(13, Levels.FromAccount(1).Smithing);
        }


        [Test]
        public void Save_AccountIdNotDefined_ExpectException()
        {
            // Define level with no account
            Levels lev = new Levels()
            {
                Smithing = 12
            };

            // Test
            try
            {
                lev.Save();
                Assert.Fail("Exception should be thrown when saving levels with no account defined");
            }
            catch
            {
                Assert.Pass();
            }
        }
    }
}
