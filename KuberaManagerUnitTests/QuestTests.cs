using KuberaManager.Models.Database;
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
        public void IsCompletedByAccount_something()
        {
            Assert.Fail("NIY");
        }



    }
}
