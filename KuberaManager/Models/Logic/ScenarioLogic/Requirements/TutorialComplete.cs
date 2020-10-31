using KuberaManager.Models.Database;
using KuberaManager.Models.Logic.ScenarioLogic.Scenarios;
using KuberaManager.Models.Logic.ScenarioLogic.Scenarios.Assigners;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KuberaManager.Models.Logic.ScenarioLogic.Requirements
{
    public class TutorialComplete : IRequirement
    {
        public bool IsPriority { get; set; } = true;

        public bool DoesMeetCondition(Account acc)
        {
            return acc.HasDefinition(Data.CompletionDataDefinition.TutorialComplete);
        }

        public ScenarioBase GetFulfillScenario(Account acc)
        {
            return QuestAssigner.ByName("TUTORIAL_ISLAND");
        }
    }
}
