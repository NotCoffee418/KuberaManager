using KuberaManager.Helpers;
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
        private static List<Quest> AllQuests
        {
            get
            {
                if (_allQuestIdentifiers == null)
                {
                    _allQuestIdentifiers = new List<Quest>()
                    {
                        C(130, "BLACK_KNIGHTS_FORTRESS", true),
                        C(29, "COOKS_ASSISTANT", true),
                        C(6071, "THE_CORSAIR_CURSE", true),
                        C(2561, "DEMON_SLAYER", true),
                        C(31, "DORICS_QUEST", true),
                        C(176, "DRAGON_SLAYER", true),
                        C(32, "ERNEST_THE_CHICKEN", true),
                        C(2378, "GOBLIN_DIPLOMACY", true),
                        C(160, "IMP_CATCHER", true),
                        C(122, "THE_KNIGHTS_SWORD", true),
                        C(3468, "MISTHALIN_MYSTERY", true),
                        C(71, "PIRATES_TREASURE", true),
                        C(273, "PRINCE_ALI_RESCUE", true),
                        C(107, "THE_RESTLESS_GHOST", true),
                        C(144, "ROMEO_AND_JULIET", true),
                        C(63, "RUNE_MYSTERIES", true),
                        C(179, "SHEEP_SHEARER", true),
                        C(145, "SHIELD_OF_ARRAV", true),
                        C(178, "VAMPIRE_SLAYER", true),
                        C(67, "WITCHS_POTION", true),

                    };
                }
                return _allQuestIdentifiers;
            }
        }

        // Helper function for cleanliness
        private static Quest C(int varp, string questName, bool isFreeToPlay)
        {
            return new Quest(varp, questName, isFreeToPlay);
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
