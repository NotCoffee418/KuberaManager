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
    public class TwentyHoursPlayed : IRequirement
    {
        public bool DoesMeetCondition(Account acc)
        {
            return acc.HasDefinition(CompletionDataDefinition.TwentyHoursPlaytime);
        }

        public ScenarioBase GetFulfillScenario(Account acc)
        {
            return QuestAssigner.GetRandomEligibleQuest(acc);
        }
    }
}
