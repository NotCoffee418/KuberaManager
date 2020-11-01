using KuberaManager.Models.Database;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KuberaManagerUnitTests
{
    class ComputerTests
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
        public void ById_CreateThenRetrieve_ExpectComputer()
        {
            _TestHelper.DbCreateMockComputers(1);
            Computer comp = Computer.ById(1);
            Assert.NotNull(comp);
            Assert.AreEqual("mockComputer1", comp.Hostname);
        }

        [Test]
        public void ByHostName_CaseMismatch_ExpectComputerNotNew()
        {
            _TestHelper.DbCreateMockComputers(1,0);
            Computer comp = Computer.ByHostname("mOckComPutEr1");
            Assert.NotNull(comp);
            Assert.AreEqual("mockComputer1", comp.Hostname);

            // Assert that no new computer was created with different cases
            int computerCount = 0;
            using (var db = new kuberaDbContext())
            {
                computerCount = db.Computers.Count();
            }
            Assert.AreEqual(1, computerCount);
        }

        [Test]
        public void ByHostName_NewComputer_ExpectCreated()
        {
            // Create & retrieve
            Computer comp = Computer.ByHostname("AutoCreatedComp");
            Computer retrievedComp = null;
            using (var db = new kuberaDbContext())
            {
                retrievedComp = db.Computers.FirstOrDefault();
            }

            // Assert
            Assert.IsNotNull(comp);
            Assert.IsNotNull(retrievedComp);
            Assert.AreEqual("AutoCreatedComp", comp.Hostname);
            Assert.AreEqual(retrievedComp.Hostname, comp.Hostname); // Case sensitive?
        }
    }
}
