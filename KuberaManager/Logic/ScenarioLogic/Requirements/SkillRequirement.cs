using KuberaManager.Logic.ScenarioLogic.Scenarios;
using KuberaManager.Logic.ScenarioLogic.Scenarios.Assigners;
using KuberaManager.Models.Data.Runescape;
using KuberaManager.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KuberaManager.Logic.ScenarioLogic.Requirements
{
    public class SkillRequirement : IRequirement
    {
        public SkillRequirement(Skill skill, int requiredLevel)
        {
            RequiredSkill = skill;
            RequiredLevel = requiredLevel;
        }

        public Skill RequiredSkill { get; set; }

        public int RequiredLevel { get; set; }

        public bool IsPriority { get; set; } = false;

        public bool DoesMeetCondition(Account acc)
        {
            Levels levels = acc.GetLevels();
            return levels.GetSkillLevel(RequiredSkill) >= RequiredLevel;
        }

        public ScenarioBase GetFulfillScenario(Account acc)
        {
            Levels levels = acc.GetLevels();
            int accountLevel = levels.GetSkillLevel(RequiredSkill);
            return SkillAssigner.GetBestTrainingArea(RequiredSkill, accountLevel);
        }
    }
}
