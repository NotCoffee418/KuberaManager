using KuberaManager.Models.Data;
using KuberaManager.Models.Database;
using KuberaManager.Models.Logic.ScenarioLogic.Scenarios;
using KuberaManager.Models.Logic.ScenarioLogic.Scenarios.Assigners;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace KuberaManagerUnitTests
{
    class QuestTests
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
        public void IsCompletedByAccount_ArbitraryQuest_TrueAndFalseCorrectly()
        {
            // Prepare account
            _TestHelper.DbCreateMockAccounts(1);
            Account acc = Account.FromId(1);

            // Grab arbitrary quest
            Quest q = QuestAssigner.ByVarp(2561);

            // Verify that account returns false correctly
            Assert.IsFalse(acc.HasDefinition(q.CompletionDefinition));
            Assert.IsFalse(q.IsCompletedByAccount(acc));

            // Mark quest as complete
            acc.AddDefinition(q.CompletionDefinition);

            // Verify that true returns correctly
            Assert.IsTrue(acc.HasDefinition(q.CompletionDefinition));
            Assert.IsTrue(q.IsCompletedByAccount(acc));
        }



    }
}
