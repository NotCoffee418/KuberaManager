using KuberaManager.Models.Database;
using KuberaManager.Models.Logic.ScenarioLogic.Scenarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KuberaManager.Models.Logic.ScenarioLogic.Requirements
{
    public interface IRequirement
    {
        // Implements a check to see if requirements are met
        public bool DoesMeetCondition(Account acc);

        // Scenario to run INSTEAD, in order to meet requirements
        public ScenarioBase GetFulfillScenario();
    }
}
