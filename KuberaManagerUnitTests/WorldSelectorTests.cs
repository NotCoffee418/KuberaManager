using KuberaManager.Models.Database;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

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
        public void SelectWorld_550SkillAccount_ExpectSkillWorld()
        {
            throw new NotImplementedException();
        }

        [Test]
        public void SelectWorld_1400SkillNonMember_ExpectRandomFree()
        {
            throw new NotImplementedException();
        }

        [Test]
        public void SelectWorld_1400SkillMember_ExpectMemberSkillWorld()
        {
            throw new NotImplementedException();
        }
    }
}
