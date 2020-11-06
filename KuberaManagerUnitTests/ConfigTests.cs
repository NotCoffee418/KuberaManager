using KuberaManager.Logic;
using KuberaManager.Models.Data;
using KuberaManager.Models.Database;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
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
            ConfigHelper ch = new ConfigHelper();

            // Set Config
            ch.Set("testKey", "testStringValue");

            // Get config
            Assert.AreEqual("testStringValue", ch.Get<string>("testKey"));
        }

        [Test]
        public void GetSet_VariousDataTypes_SuccessfulRetrieve()
        {
            ConfigHelper ch = new ConfigHelper();

            // test bool
            ch.Set<bool>("testKey", true);
            bool boolResult = ch.Get<bool>("testKey");
            Assert.AreEqual(boolResult, true);

            // test double
            ch.Set<double>("testKey", 1.23);
            double doubleResult = ch.Get<double>("testKey");
            Assert.AreEqual(doubleResult, 1.23);

            // test int
            ch.Set<int>("testKey", 123);
            int intResult = ch.Get<int>("testKey");
            Assert.AreEqual(intResult, 123);
        }

        [Test]
        public void StaticConstructor_InstallCorrectly_RetrieveDefaultValueWithoutSetting()
        {
            // Grab the actual DB result
            ConfigHelper ch = new ConfigHelper();
            int result = ch.Get<int>("MaxHoursPerDay");

            // Grab expected result from hardcoded default
            int expectedResult = Convert.ToInt32(ConfigData.Keys
                .Where(x => x.Name == "MaxHoursPerDay")
                .Select(x => x.DefaultValue)
                .First());

            Assert.NotNull(result);
            Assert.AreEqual(expectedResult, result);
        }
    }
}