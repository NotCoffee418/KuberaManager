using KuberaManager.Models.Database;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Linq;

namespace KuberaManagerUnitTests
{
    public class ConfigTests
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
        public void GetSet_SetString_SuccessfulRetrieve()
        {
            // Set Config
            Config.Set("testKey", "testStringValue");

            // Get config
            Assert.AreEqual("testStringValue", Config.Get<string>("testKey"));
        }

        [Test]
        public void GetSet_VariousDataTypes_SuccessfulRetrieve()
        {
            // test bool
            Config.Set<bool>("testKey", true);
            bool boolResult = Config.Get<bool>("testKey");
            Assert.AreEqual(boolResult, true);

            // test double
            Config.Set<double>("testKey", 1.23);
            double doubleResult = Config.Get<double>("testKey");
            Assert.AreEqual(doubleResult, 1.23);

            // test int
            Config.Set<int>("testKey", 123);
            int intResult = Config.Get<int>("testKey");
            Assert.AreEqual(intResult, 123);
        }
    }
}