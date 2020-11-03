using KuberaManager.Helpers;
using KuberaManager.Models.Database;
using KuberaManager.Logic.ScenarioLogic.Requirements;
using KuberaManager.Logic.ScenarioLogic.Scenarios.Assigners;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KuberaManager.Logic.ScenarioLogic.Scenarios
{
    public class ScenarioHelper
    {
        /// <summary>
        /// This s a list of scenarios bots will end up running.
        /// If they don't meet the requirements, they'll get them first.
        /// </summary>
        public readonly static List<string> PrimaryScenarios = new List<string>()
        {
            "Quest.COOKS_ASSISTANT",
        };


        // WARNING!! ensure new scenario types are added here
        public static List<ScenarioBase> AllScenarios
        {
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

        /// <summary>
        /// Runs through requirements and selects a random viable scenario
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public static ScenarioBase FindViableScenario(Account account)
        {
            // Grab a primary scenario to run
            if (PrimaryScenarios.Count() == 0)
                throw new Exception("No primary scenarios are defined. Can't proceed");
            int rand = RandomHelper.GetRandom(0, PrimaryScenarios.Count() - 1);
            ScenarioBase baseScenario = ByIdentifier(PrimaryScenarios[rand]);

            // Return scenario or child requirement to run
            return SelectScenarioOrRequirement(baseScenario, account);
        }

        /// <summary>
        /// This is a tree function. It will grab a random requirement or requirement of requirement of requirement...
        /// </summary>
        /// <param name="scen"></param>
        /// <param name="acc"></param>
        /// <returns></returns>
        public static ScenarioBase SelectScenarioOrRequirement(ScenarioBase scen, Account acc)
        {
            // Randomize requirement order
            List<IRequirement> randReq = scen.Requirements
                .OrderBy(x => RandomHelper.GetRandom(0, int.MaxValue)).ToList();

            // Run SelectScenarioOrRequirement on each requirement
            // if we don't mee it, return that requirement
            foreach (IRequirement req in randReq)
                if (!req.DoesMeetCondition(acc))
                    return SelectScenarioOrRequirement(req.GetFulfillScenario(acc), acc);

            // We're still here, which means all requirements are met.
            // Return the original scenario
            return scen;
        }
    }
}
