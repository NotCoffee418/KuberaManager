using KuberaManager.Models.Database;
using KuberaManager.Logic.ScenarioLogic.Scenarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KuberaManager.Logic.ScenarioLogic.Requirements
{
    public interface IRequirement
    {
        // Determines if this requirement should be fulfilled as a priority
        // Basically, no shuffle, always do it first when true
        public bool IsPriority { get; set; }

        // Implements a check to see if requirements are met
        public bool DoesMeetCondition(Account acc);

        // Scenario to run INSTEAD, in order to meet requirements
        public ScenarioBase GetFulfillScenario(Account acc);
    }
}
