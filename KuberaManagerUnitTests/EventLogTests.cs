using KuberaManager.Models.Database;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KuberaManagerUnitTests
{
    class EventLogTests
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
        public void AddEntry_OneNewEntry_CanRetrieve()
        {
            // Prepare
            _TestHelper.DbCreateMockAccounts(1);
            _TestHelper.DbCreateMockSession(1, true);

            // Add & retrieve
            EventLog.AddEntry(1, "Test Entry");
            var retrieved = EventLog.GetSessionDisplayLogs(1);

            // Assert
            Assert.AreEqual(1, retrieved.Count());
            Assert.AreEqual("Test Entry", retrieved.First().Text);
        }

        [Test]
        public void AddEntry_SameTextEntries_CanRetrieve()
        {
            // Prepare
            _TestHelper.DbCreateMockAccounts(1);
            _TestHelper.DbCreateMockSession(1, true);

            // Add & retrieve
            EventLog.AddEntry(1, "Test Entry");
            EventLog.AddEntry(1, "Test Entry");
            var retrieved = EventLog.GetSessionDisplayLogs(1);

            // Assert
            Assert.AreEqual(2, retrieved.Count());
            Assert.AreEqual("Test Entry", retrieved[0].Text);
            Assert.AreEqual("Test Entry", retrieved[1].Text);
        }

        [Test]
        public void AddEntry_MixedEntries_CanRetrieve()
        {
            // Prepare
            _TestHelper.DbCreateMockAccounts(1);
            _TestHelper.DbCreateMockSession(1, true);

            // Add & retrieve
            EventLog.AddEntry(1, "Test Entry");
            EventLog.AddEntry(1, "Another Test Entry");
            var retrieved = EventLog.GetSessionDisplayLogs(1);

            // Assert
            Assert.AreEqual(2, retrieved.Count());

            // Inverted because descending order
            Assert.AreEqual("Another Test Entry", retrieved[0].Text);
            Assert.AreEqual("Test Entry", retrieved[1].Text);
        }
    }
}
