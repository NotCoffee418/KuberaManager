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
    class QuestAssignerTests
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
        public void GetRandomEligibleQuest_FreeOnly_TenRunsCorrectMatch()
        {
            // prep accs
            _TestHelper.DbCreateMockAccounts(1);
            Account freeAcc = Account.FromId(1);

            // Get 10 random quests
            for (int i = 0; i < 10; i++)
            {
                // Get random
                Quest q = QuestAssigner.GetRandomEligibleQuest(freeAcc);
                Assert.NotNull(q);
                Assert.IsTrue(q.IsFreeToPlay);
            }
        }

        [Test]
        public void ByVarp_ThreeFreeQuest_GetsCorrectQuestsByCompletionDataDefinition()
        {
            // Grab three random quests
            Quest blackKnightsFortress = QuestAssigner.ByVarp(130);
            Quest cooksAssistant = QuestAssigner.ByVarp(29);
            Quest ernestTheChicken = QuestAssigner.ByVarp(32);

            // Assert
            Assert.AreEqual(CompletionDataDefinition.QUEST_BLACK_KNIGHTS_FORTRESS, blackKnightsFortress.CompletionDefinition);
            Assert.AreEqual(CompletionDataDefinition.QUEST_COOKS_ASSISTANT, cooksAssistant.CompletionDefinition);
            Assert.AreEqual(CompletionDataDefinition.QUEST_ERNEST_THE_CHICKEN, ernestTheChicken.CompletionDefinition);
        }


        [Test]
        public void ByName_ThreeFreeQuest_GetsCorrectQuestsByCompletionDataDefinition()
        {
            // Grab three random quests
            Quest blackKnightsFortress = QuestAssigner.ByName("BLACK_KNIGHTS_FORTRESS");
            Quest cooksAssistant = QuestAssigner.ByName("COOKS_ASSISTANT");
            Quest ernestTheChicken = QuestAssigner.ByName("ERNEST_THE_CHICKEN");

            // Assert
            Assert.AreEqual(CompletionDataDefinition.QUEST_BLACK_KNIGHTS_FORTRESS, blackKnightsFortress.CompletionDefinition);
            Assert.AreEqual(CompletionDataDefinition.QUEST_COOKS_ASSISTANT, cooksAssistant.CompletionDefinition);
            Assert.AreEqual(CompletionDataDefinition.QUEST_ERNEST_THE_CHICKEN, ernestTheChicken.CompletionDefinition);
        }
    }
}
