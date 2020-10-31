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
        public bool IsPriority { get; set; } = true;

        public bool DoesMeetCondition(Account acc)
        {
            // Check if we have completiondefinition set
            if (acc.HasDefinition(CompletionDataDefinition.TwentyHoursPlaytime))
                return true;

            // Otherwise calculate play time
            TimeSpan playTime = acc.GetTotalPlayedTime();
            bool hasMoreThanTwentyOneHours = playTime > TimeSpan.FromHours(21);

            // Set completiondefinition if condition is newly met
            if (hasMoreThanTwentyOneHours)
                acc.AddDefinition(CompletionDataDefinition.TwentyHoursPlaytime);

            // Return result
            return hasMoreThanTwentyOneHours;
        }

        public ScenarioBase GetFulfillScenario(Account acc)
        {
            return QuestAssigner.GetRandomEligibleQuest(acc);
        }
    }
}
