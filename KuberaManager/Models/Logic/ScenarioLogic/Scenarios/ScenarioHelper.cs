using KuberaManager.Models.Logic.ScenarioLogic.Scenarios.Assigners;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KuberaManager.Models.Logic.ScenarioLogic.Scenarios
{
    public class ScenarioHelper
    {
        
#warning ensure new scenario types are added here
        public static List<ScenarioBase> AllScenarios { 
            get
            {
                return new List<ScenarioBase>()
                    .Concat(QuestAssigner.AllQuests)
                    //.Concat(...)
                    .ToList();
            }
        }

        /// <summary>
        /// Case insensistive.
        /// </summary>
        /// <param name="identifier"></param>
        /// <returns></returns>
        public static ScenarioBase ByIdentifier(string identifier)
        {
            // Validate
            string[] parts = identifier.Split('.');
            if (parts.Count() != 2)
                throw new Exception($"{identifier} does not have a valid scenario identifier format. Valid format must be structured like: Quest.ExampleQuest");

            // Find the correct scenario
            return AllScenarios
                .Where(x => x.ScenarioName.ToLower() == parts[0].ToLower())
                .Where(x => x.ScenarioArgument.ToLower() == parts[1].ToLower())
                .FirstOrDefault();
        }


    }
}
