using KuberaManager.Models.Database;
using KuberaManager.Models.Logic.ScenarioLogic.Scenarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KuberaManager.Models.Logic
{
    public class Brain
    {
        public static ScenarioBase DetermineScenario(Session sess)
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

        internal static bool DoesClientNeedJobUpdate()
        {
            throw new NotImplementedException();
        }

        // pseudo:
        // Check Account.ContinueScenario prio, run it and NULL it if exist
        // else Figure out new job
        internal static Job GetNewJob()
        {
            throw new NotImplementedException();
        }

        // pseudo:
        // if past session exp time
        //   if ScenarioBase.AlwaysRunUntilComplete
        //      store job in Account.ContinueScenario
        public static bool ShouldStopSessionCheck(Account acc)
        {
            throw new NotImplementedException();
        } 
    }
}
