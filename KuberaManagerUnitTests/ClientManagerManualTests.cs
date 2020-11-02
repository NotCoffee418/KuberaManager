using KuberaManager.Models.Database;
using KuberaManager.Models.Logic;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace KuberaManagerUnitTests
{
    class ClientManagerManualTests
    {
        [SetUp]
        public void Setup()
        {
            // Set db context to testing
            kuberaDbContext.IsUnitTesting = true;

            // Manual vars here
        }

        Computer Comp { get; set; }
        string RspeerApiKey { get; set; }

        [TearDown]
        public void TearDown()
        {
            using (var db = new kuberaDbContext())
            {
                db.Database.EnsureDeleted();
            }
        }

        [Test]
        public void StartClientStopClient_ManualInput_ShouldStartClient()
        {
            // Only run when manual vars are defined
            if (RspeerApiKey == "" || Comp == null)
                return;

            // Automatic vars
            _TestHelper.DbCreateMockAccounts(1);
            Account acc = Account.FromLogin("testAccount1@mockup.fake");
            Config.Set<string>("RspeerApiKey1", RspeerApiKey);

            // Find relevant session
            Session sess = Session.FromAccount(acc.Login);

            // Start client
            ClientManager.StartClient(acc, Comp, sess);

            // Wait 20 seconds and shut down
            Thread.Sleep(20000);

            //Stop client
            ClientManager.StopClient(sess);
        }
    }
}
