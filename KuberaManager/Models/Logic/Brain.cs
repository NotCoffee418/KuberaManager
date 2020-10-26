using KuberaManager.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KuberaManager.Models.Logic
{
    public class Brain
    {
        public static Scenario DetermineScenario(Session sess)
        {
            throw new NotImplementedException();
        }

        public static TimeSpan GetTargetDuration(Account account)
        {
            return TimeSpan.FromMinutes(5);
            //throw new NotImplementedException();
        }

        // input args etc need to be defined
        // false means we can't do it for some reason
        public static bool StartNewClient()
        {
            // Get available computer. return if none is available
            Computer availableComputer = Computer.GetAvailableComputer();
            if (availableComputer == null)
                return false;

            //

            throw new NotImplementedException();
        }
    }
}
