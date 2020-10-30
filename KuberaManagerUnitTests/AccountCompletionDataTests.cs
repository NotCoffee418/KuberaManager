using KuberaManager.Models.Data;
using KuberaManager.Models.Database;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace KuberaManagerUnitTests
{
    class AccountCompletionDataTests
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
        public void AddDefinition_OneDefinition_NoException()
        {
            // Prepare
            _TestHelper.DbCreateMockAccounts(1);
            Account acc = Account.FromId(1);

            // Add definitions
            acc.AddDefinition(CompletionDataDefinition.HunderedSkillPoints);
        }

        [Test]
        public void AccountHasDefinition_MatchAndMismatch_CorrectlyIdentify()
        {
            // Prepare
            _TestHelper.DbCreateMockAccounts(1);
            Account acc = Account.FromId(1);

            // Add definitions
            acc.AddDefinition(CompletionDataDefinition.HunderedSkillPoints);

            // Assert
            Assert.IsTrue(acc.HasDefinition(CompletionDataDefinition.HunderedSkillPoints));
            Assert.IsFalse(acc.HasDefinition(CompletionDataDefinition.TwentyHoursPlaytime));
        }

        [Test]
        public void GetDefinitions_MatchAndMismatch_CorrectlyIdentify()
        {
            // Prepare
            _TestHelper.DbCreateMockAccounts(1);
            Account acc = Account.FromId(1);

            // Add definitions
            acc.AddDefinition(CompletionDataDefinition.HunderedSkillPoints);
            acc.AddDefinition(CompletionDataDefinition.TutorialComplete);

            // Get definitions & assert
            var accDefinitions = acc.GetDefinitions();
            Assert.IsTrue(acc.HasDefinition(CompletionDataDefinition.HunderedSkillPoints));
            Assert.IsTrue(acc.HasDefinition(CompletionDataDefinition.TutorialComplete));
            Assert.IsFalse(acc.HasDefinition(CompletionDataDefinition.TwentyHoursPlaytime));

        }
    }
}
