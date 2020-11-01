using KuberaManager.Models.Database;
using KuberaManager.Models.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KuberaManager.Hangfire
{
    public class ScheduledTasks
    {
        public static void RunHourlyJobs()
        {
            // Cleans sessions that didn't close correctly
            Brain.ScheduledSessionJanitor();
        }

        public static void RunMinutelyJobs()
        {
            // Starts new sessions when needed
            Brain.ScheduledSessionStarter();
        }
    }
}
