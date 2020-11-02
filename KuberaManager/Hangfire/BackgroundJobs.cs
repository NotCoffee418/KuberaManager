using Hangfire;
using KuberaManager.Models.Data.RspeerApiStructure;
using KuberaManager.Models.Database;
using KuberaManager.Models.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace KuberaManager.Hangfire
{
    public class BackgroundJobs
    {
        public static void Launch_FindRspeerSessionTag(Account account)
        {
            BackgroundJob.Enqueue(() => FindRspeerSessionTag(account));
        }

        public static void FindRspeerSessionTag(Account account)
        {
            Session sess = null;

            // Try for 5 minutes
            for (int i = 1; i < 36; i++) // start at 1 to avoid duplicate launch
            {
                // Wait 5 seconds before all attempts
                Thread.Sleep(5000);

                // Return if we killed session
                sess = account.GetActiveSession(); // recheck every loop
                if (sess == null || sess.IsFinished)
                {
                    DiscordHandler.PostMessage("Manager: A new session was created and killed before it was able to launched.");
                    return;
                }

                // Try to find session tag
                ConnectedClients.ClientData client = null;
                var allClients = ClientManager.GetConnectedClients();
                if (allClients != null)
                    client = allClients
                        .Where(x => x.runescapeEmail.ToLower() == account.Login.ToLower())
                        .FirstOrDefault();

                // Found it! Save tag & return.
                if (client != null)
                {
                    sess.UpdateTag(client.tag);
                    return;
                }
            }

            // Still here. Report bork on discord
            string sessionDisplayId = sess == null ? "??" : sess.Id.ToString();
            string errorMsg = $"Failed to launch client after 5 minutes of trying. Session: '{sessionDisplayId}' Account: '{account.Id}'";
            DiscordHandler.PostMessage("Manager: " + errorMsg);
            EventLog.AddEntry(sess.Id, "Failed to launch client after 5 minutes of trying.");

            // Kill session
            using (var db = new kuberaDbContext())
            {
                db.Attach(sess);
                sess.IsFinished = true;
                db.SaveChanges();
            }
        }
    }
}
