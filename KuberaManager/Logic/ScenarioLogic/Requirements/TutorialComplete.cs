using KuberaManager.Models.Database;
using KuberaManager.Logic.ScenarioLogic.Scenarios;
using KuberaManager.Logic.ScenarioLogic.Scenarios.Assigners;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KuberaManager.Models.Data;

namespace KuberaManager.Logic.ScenarioLogic.Requirements
{
    public class TutorialComplete : IRequirement
    {
        public bool IsPriority { get; set; } = true;

        public bool DoesMeetCondition(Account acc)
        {
            return acc.HasDefinition(CompletionDataDefinition.TutorialComplete);
        }

        public ScenarioBase GetFulfillScenario(Account acc)
        {
            return QuestAssigner.ByName("TUTORIAL_ISLAND");
        }
    }
}
