using KuberaManager.Models.Database;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using KuberaManager.Logic;
using System.Linq;
using KuberaManager.Logic.ScenarioLogic.Scenarios;
using KuberaManager.Logic.ScenarioLogic.Scenarios.Assigners;

namespace KuberaManagerUnitTests
{
    class WorldSelectorTests
    {
        [SetUp]
        public void Setup()
        {
            // Set db context to testing
            kuberaDbContext.IsUnitTesting = true;

            // Create test key. Config can only update.
            using (var db = new kuberaDbContext())
            {
                Config conf = new Config()
                {
                    ConfKey = "testKey",
                    ConfValue = "initialValue"
                };
                db.Configs.Add(conf);
                db.SaveChanges();
            }
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
        [TestCase(true)]
        [TestCase(false)]
        public void SelectWorld_LowSkillAccount_ExpectRandomWorld(bool asMember)
        {
            // Prepare test data
            _TestHelper.DbCreateMockAccounts(1);
            Account acc = Account.FromId(1);

            // Set AsMember
            using (var db = new kuberaDbContext())
            {
                acc.IsMember = asMember;
                db.Accounts.Update(acc);
                db.SaveChanges();
            }

            // Set acceptable world 
            WorldSelector ws = new WorldSelector();
            int[] acceptableWorlds = asMember ?
                WorldSelector.MemberWorldsWithoutRestrictionOrPreference :
                WorldSelector.FreeToPlayWorldsWithoutRestrictionOrPreference;

            // Find world
            int world = ws.SelectWorld(acc, QuestAssigner.AllQuests.First());

            // Asserts
            Assert.NotZero(world);
            Assert.IsTrue(acceptableWorlds.Contains(world));
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void SelectWorld_550SkillAccount_ExpectFreeSkillWorld(bool asMember)
        {
            // Prepare test data
            _TestHelper.DbCreateMockAccounts(1);
            Account acc = Account.FromId(1);

            // Set AsMember
            using (var db = new kuberaDbContext())
            {
                acc.IsMember = asMember;
                db.Accounts.Update(acc);
                db.SaveChanges();
            }

            // Validate asMember update
            Account accUpd = Account.FromId(1);
            Assert.AreEqual(asMember, accUpd.IsMember);
            
            // Set skill level
            using (var db = new kuberaDbContext())
            {
                Levels levels = new Levels()
                {
                    AccountId = 1,
                    Slayer = 550, // spoof total level
                };

                db.Levels.Add(levels);
                db.SaveChanges();
            }

            // Set acceptable world 
            WorldSelector ws = new WorldSelector();
            List<int> acceptableWorlds = WorldSelector.RestrictedWorlds
                .Where(x => 550 >= x.MinSkill)
                .Where(x => 550 <= x.MaxSkill)
                .Select(x => x.Number)
                .ToList();

            // Select world
            int world = ws.SelectWorld(acc, QuestAssigner.AllQuests.First());

            // Asserts
            Assert.NotZero(world);
            Assert.IsTrue(acceptableWorlds.Contains(world));
        }

        [Test]
        [TestCase(true, Description = "As member, expect random free world regardless of skill")]
        [TestCase(false, Description = "As non-member, expect random free world regardless of skill")]
        public void SelectWorld_1400Skill(bool asMember)
        {
            // Prepare test data
            _TestHelper.DbCreateMockAccounts(1);
            Account acc = Account.FromId(1);

            // Set AsMember
            using (var db = new kuberaDbContext())
            {
                acc.IsMember = asMember;
                db.Accounts.Update(acc);
                db.SaveChanges();
            }

            // Validate asMember update
            Account accUpd = Account.FromId(1);

            // Set skill level
            using (var db = new kuberaDbContext())
            {
                Levels levels = new Levels()
                {
                    AccountId = 1,
                    Slayer = 1400, // spoof total level
                };

                db.Levels.Add(levels);
                db.SaveChanges();
            }

            // Set acceptable world 
            int[] acceptableWorlds = asMember ?
                WorldSelector.RestrictedWorlds // Members get member skill list
                .Where(x => 1250 >= x.MinSkill)
                .Where(x => 1499 <= x.MaxSkill)
                .Select(x => x.Number)
                .ToArray() : 
            WorldSelector.FreeToPlayWorldsWithoutRestrictionOrPreference; // Free players get random

            // Select world
            WorldSelector ws = new WorldSelector();
            int world = ws.SelectWorld(acc, QuestAssigner.AllQuests.First());

            // Asserts
            Assert.NotZero(world);
            Assert.IsTrue(acceptableWorlds.Contains(world));
        }
    }
}
