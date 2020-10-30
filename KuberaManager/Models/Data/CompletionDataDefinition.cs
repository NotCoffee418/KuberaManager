using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KuberaManager.Models.Data
{
    // WARNING: Hardcoded, existing IDs should not be changed
    // Check for duplicates before adding stuff
    // These are used by Database AccountCompletionData and Scenario Requirements
    public enum CompletionDataDefinition
    {
        Invalid = 0,
        TutorialComplete = 1,
        TwentyHoursPlaytime = 2,
        HunderedSkillPoints = 3,
        QUEST_BLACK_KNIGHTS_FORTRESS = 4,
        QUEST_COOKS_ASSISTANT = 5,
        QUEST_THE_CORSAIR_CURSE = 6,
        QUEST_DEMON_SLAYER = 7,
        QUEST_DORICS_QUEST = 8,
        QUEST_DRAGON_SLAYER = 9,
        QUEST_ERNEST_THE_CHICKEN = 10,
        QUEST_GOBLIN_DIPLOMACY = 11,
        QUEST_IMP_CATCHER = 12,
        QUEST_THE_KNIGHTS_SWORD = 13,
        QUEST_MISTHALIN_MYSTERY = 14,
        QUEST_PIRATES_TREASURE = 15,
        QUEST_PRINCE_ALI_RESCUE = 16,
        QUEST_THE_RESTLESS_GHOST = 17,
        QUEST_ROMEO_AND_JULIET = 18,
        QUEST_RUNE_MYSTERIES = 19,
        QUEST_SHEEP_SHEARER = 20,
        QUEST_SHIELD_OF_ARRAV = 21,
        QUEST_VAMPIRE_SLAYER = 22,
        QUEST_WITCHS_POTION = 23,

    }
}
