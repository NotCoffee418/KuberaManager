using KuberaManager.Helpers;
using KuberaManager.Models.Data;
using KuberaManager.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace KuberaManager.Models.Logic.ScenarioLogic.Scenarios.Assigners
{
    public class QuestAssigner
    {
        private static List<Quest> _allQuestIdentifiers = null;
        public static List<Quest> AllQuests
        {
            get
            {
                if (_allQuestIdentifiers == null)
                {
                    _allQuestIdentifiers = new List<Quest>()
                    {
                        // Free to play
                        C(130, "BLACK_KNIGHTS_FORTRESS", true, CompletionDataDefinition.QUEST_BLACK_KNIGHTS_FORTRESS),
                        C(29, "COOKS_ASSISTANT", true, CompletionDataDefinition.QUEST_COOKS_ASSISTANT),
                        C(6071, "THE_CORSAIR_CURSE", true, CompletionDataDefinition.QUEST_THE_CORSAIR_CURSE),
                        C(2561, "DEMON_SLAYER", true, CompletionDataDefinition.QUEST_DEMON_SLAYER),
                        C(31, "DORICS_QUEST", true, CompletionDataDefinition.QUEST_DORICS_QUEST),
                        C(176, "DRAGON_SLAYER", true, CompletionDataDefinition.QUEST_DRAGON_SLAYER),
                        C(32, "ERNEST_THE_CHICKEN", true, CompletionDataDefinition.QUEST_ERNEST_THE_CHICKEN),
                        C(2378, "GOBLIN_DIPLOMACY", true, CompletionDataDefinition.QUEST_GOBLIN_DIPLOMACY),
                        C(160, "IMP_CATCHER", true, CompletionDataDefinition.QUEST_IMP_CATCHER),
                        C(122, "THE_KNIGHTS_SWORD", true, CompletionDataDefinition.QUEST_THE_KNIGHTS_SWORD),
                        C(3468, "MISTHALIN_MYSTERY", true, CompletionDataDefinition.QUEST_MISTHALIN_MYSTERY),
                        C(71, "PIRATES_TREASURE", true, CompletionDataDefinition.QUEST_PIRATES_TREASURE),
                        C(273, "PRINCE_ALI_RESCUE", true, CompletionDataDefinition.QUEST_PRINCE_ALI_RESCUE),
                        C(107, "THE_RESTLESS_GHOST", true, CompletionDataDefinition.QUEST_THE_RESTLESS_GHOST),
                        C(144, "ROMEO_AND_JULIET", true, CompletionDataDefinition.QUEST_ROMEO_AND_JULIET),
                        C(63, "RUNE_MYSTERIES", true, CompletionDataDefinition.QUEST_RUNE_MYSTERIES),
                        C(179, "SHEEP_SHEARER", true, CompletionDataDefinition.QUEST_SHEEP_SHEARER),
                        C(145, "SHIELD_OF_ARRAV", true, CompletionDataDefinition.QUEST_SHIELD_OF_ARRAV),
                        C(178, "VAMPIRE_SLAYER", true, CompletionDataDefinition.QUEST_VAMPIRE_SLAYER),
                        C(67, "WITCHS_POTION", true, CompletionDataDefinition.QUEST_WITCHS_POTION),

                        // Members
                        // ...
                    };
                }
                return _allQuestIdentifiers;
            }
        }

        // Helper function for cleanliness
        private static Quest C(int varp, string questName, bool isFreeToPlay, CompletionDataDefinition def)
        {
            return new Quest(varp, questName, isFreeToPlay, def);
        }


        public static Quest GetRandomEligibleQuest(Account account)
        {
            // Get eligible quests
            var eligibleQuests = AllQuests
                .Where(q => account.IsMember || q.IsFreeToPlay)
                .ToList();

            // return random quest
            int rand = RandomHelper.GetRandom(0, eligibleQuests.Count() - 1);
            return eligibleQuests[rand];
        }

        public static Quest ByVarp(int id)
        {
            return AllQuests
                .Where(x => x.Varp == id)
                .FirstOrDefault();
        }

        public static Quest ByName(string name)
        {
            return AllQuests
                .Where(x => x.QuestName == name)
                .FirstOrDefault();
        }
    }
}
