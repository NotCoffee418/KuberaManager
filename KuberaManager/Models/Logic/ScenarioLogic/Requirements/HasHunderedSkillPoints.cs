using KuberaManager.Models.Data;
using KuberaManager.Models.Database;
using KuberaManager.Models.Logic.ScenarioLogic.Scenarios;
using KuberaManager.Models.Logic.ScenarioLogic.Scenarios.Assigners;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KuberaManager.Models.Logic.ScenarioLogic.Requirements
{
    public class HasHunderedSkillPoints : IRequirement
    {
        public bool IsPriority { get; set; } = true;

        public bool DoesMeetCondition(Account acc)
        {
            // Grab from definition
            bool hasDef = acc.HasDefinition(CompletionDataDefinition.HunderedSkillPoints);
            if (hasDef) return true;

            // todo: log
            // No skills have been reported yet? Assume false
            Levels lev = acc.GetLevels();
            if (lev == null) return false;

            // Got levels. Add them up
            int totalLevels = lev.GetTotalLevel();
            if (totalLevels >= 100)
            {
                // Store newly matched definition & return
                acc.AddDefinition(CompletionDataDefinition.HunderedSkillPoints);
                return true;
            }

            // Still here, return false
            return false;
        }

        public ScenarioBase GetFulfillScenario(Account acc)
        {
            return QuestAssigner.GetRandomEligibleQuest(acc);
        }
    }
}
