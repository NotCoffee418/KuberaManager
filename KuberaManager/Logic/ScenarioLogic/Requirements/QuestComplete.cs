using KuberaManager.Models.Database;
using KuberaManager.Logic.ScenarioLogic.Scenarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KuberaManager.Logic.ScenarioLogic.Scenarios.Types;
using KuberaManager.Logic.ScenarioLogic.Scenarios.Assigners;

namespace KuberaManager.Logic.ScenarioLogic.Requirements
{
    public class QuestComplete : IRequirement
    {
        /// <summary>
        /// Quest name as defined in QuestAssigner
        /// eg TUTORIAL_ISLAND
        /// </summary>
        /// <param name="questName"></param>
        public QuestComplete(string questName)
        {
            QuestName = questName;
        }

        public bool IsPriority { get; set; } = false;

        private string QuestName { get; set; }

        public bool DoesMeetCondition(Account acc)
        {
            QuestScenario questScenario = QuestAssigner.ByName(QuestName);
            return acc.HasDefinition(questScenario.CompletionDefinition);
        }

        public ScenarioBase GetFulfillScenario(Account acc)
        {
            return QuestAssigner.ByName(QuestName);
        }
    }
}
