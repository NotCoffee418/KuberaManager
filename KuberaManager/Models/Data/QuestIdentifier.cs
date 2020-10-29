using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KuberaManager.Models.Data
{
    public class QuestIdentifier
    {
        // Static prepare AllQuestIdentifiers & store in memory
        private static List<QuestIdentifier> _allQuestIdentifiers = null;
        public static List<QuestIdentifier> AllQuestIdentifiers {
            get
            {
                if (_allQuestIdentifiers == null)
                {
                    _allQuestIdentifiers = new List<QuestIdentifier>()
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

        #region Non-static properties
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsFreeToPlay { get; set; }
        #endregion

        // Shortcut method to populate AllQuestIdentifiers
        private static QuestIdentifier C(int id, string name, bool isFreeToPlay)
        {
            return new QuestIdentifier()
            {
                Id = id,
                Name = name,
                IsFreeToPlay = isFreeToPlay
            };
        }


        public static QuestIdentifier ById(int id)
        {
            return AllQuestIdentifiers
                .Where(x => x.Id == id)
                .FirstOrDefault();
        }

        public static QuestIdentifier ByName(string name)
        {
            return AllQuestIdentifiers
                .Where(x => x.Name == name)
                .FirstOrDefault();
        }

        public static List<QuestIdentifier> GetAllFreeToPlay()
        {
            return AllQuestIdentifiers
                .Where(x => x.IsFreeToPlay)
                .ToList();
        }

        public static List<QuestIdentifier> GetAllForMembers()
        {
            return AllQuestIdentifiers
                .Where(x => !x.IsFreeToPlay)
                .ToList();
        }
    }
}
