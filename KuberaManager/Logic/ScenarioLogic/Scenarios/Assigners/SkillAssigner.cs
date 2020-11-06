using KuberaManager.Helpers;
using KuberaManager.Logic.ScenarioLogic.Scenarios.Types;
using KuberaManager.Models.Data.Runescape;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KuberaManager.Logic.ScenarioLogic.Scenarios.Assigners
{
    public class SkillAssigner
    {
        private static List<SkillScenario> _allSkillScenarios = null;
        public static List<SkillScenario> AllSkillScenarios
        {
            get
            {
                if (_allSkillScenarios == null)
                    _allSkillScenarios = new List<SkillScenario>()
                    {
                        C(Skill.Woodcutting, "BarbLogs", 0), // Skill requirement optional
                        C(Skill.Woodcutting, "BarbPower"),
                    };
                return _allSkillScenarios;
            }
        }

        // Helper function for cleanliness.
        private static SkillScenario C(Skill skill, string argument, int minLevel = 0)
        {
            return new SkillScenario(skill, argument, minLevel);
        }

        /// <summary>
        /// Finds the best training area for an account with the provided level
        /// </summary>
        /// <param name="requiredSkill"></param>
        /// <param name="accountLevel"></param>
        /// <returns></returns>
        internal static ScenarioBase GetBestTrainingArea(Skill requiredSkill, int accountLevel)
        {
            var result = AllSkillScenarios
                // Meets conditions
                .Where(x => x.Skill == requiredSkill)
                .Where(x => x.RequiredLevel <= accountLevel)

                // Highest level = most xp, therefore has priority
                .OrderByDescending(x => x.RequiredLevel)

                // Order by random if there are multiple scenarios at this level
                .ThenBy(x => RandomHelper.GetRandom(0, int.MaxValue))
                .FirstOrDefault();

            // Handle skill not implemented
            if (result == null)
                throw new Exception("No scenarios for this skill are implemented on the server");

            return result;
        }
    }
}
