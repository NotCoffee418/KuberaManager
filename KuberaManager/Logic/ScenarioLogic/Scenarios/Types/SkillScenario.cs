using KuberaManager.Logic.ScenarioLogic.Requirements;
using KuberaManager.Models.Data.Runescape;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KuberaManager.Logic.ScenarioLogic.Scenarios.Types
{
    public class SkillScenario : ScenarioBase
    {
        public SkillScenario(Skill skill, string argument, int minLevel = 0)
            : base()
        {
            // Define ScenarioBase stuff
            ScenarioName = skill.ToString();
            ScenarioArgument = argument;

            // Define SkillScenario stuff
            Skill = skill;
            RequiredLevel = minLevel;

            // Create any requirement
            if (minLevel > 0)
                Requirements.Add(new SkillRequirement(skill, minLevel));
        }

        public Skill Skill { get; set; }
        public int RequiredLevel { get; set; }
    }
}
