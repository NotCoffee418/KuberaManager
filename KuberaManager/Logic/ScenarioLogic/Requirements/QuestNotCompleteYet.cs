using KuberaManager.Models.Database;
using KuberaManager.Logic.ScenarioLogic.Scenarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KuberaManager.Logic.ScenarioLogic.Requirements
{
    public class QuestNotCompleteYet : IRequirement
    {
        public QuestNotCompleteYet(Quest quest)
        {
            Quest = quest;
        }

        public bool IsPriority { get; set; } = false;

        private Quest Quest { get; set; }

        public bool DoesMeetCondition(Account acc)
        {
            // return false if we did it
            return !acc.HasDefinition(Quest.CompletionDefinition);
        }

        public ScenarioBase GetFulfillScenario(Account acc)
        {
            // Quest is already done... Whatdoyawhant??
            return null;
        }
    }
}
