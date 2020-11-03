using KuberaManager.Helpers;
using KuberaManager.Logic.ScenarioLogic.Scenarios;
using KuberaManager.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KuberaManager.Logic
{
    public class WorldsSelector
    {
        public int SelectWorld(Account acc, ScenarioBase scen)
        {
            // Member status definitions
            bool requiresMemberWorld = scen.MembersOnly;
            bool canJoinMemberWorlds = acc.IsMember;
            int totalSkill = 0;

            // Determine total skill level
            Levels lev = Levels.FromAccount(acc.Id);
            if (lev != null)
                totalSkill = lev.GetTotalLevel();

            // Validate member defs
            if (requiresMemberWorld && !canJoinMemberWorlds)
                throw new Exception("WorldSelector failed. Scenario requires member world but this account is not member.");

            // Prefer to find world with skill requirement
            List<World> skillRelevantWorlds = RestrictedWorlds
                .Where(x => totalSkill >= x.MinSkill)
                .Where(x => totalSkill <= x.MaxSkill)
                .OrderByDescending(x => x.MinSkill)
                .ThenBy(x => RandomHelper.GetRandom(0, int.MaxValue))
                .ToList();

            // Continue filtering skill worlds by member status / req
            if (skillRelevantWorlds.Count > 0)
            {
                List<World> filteredWorlds = new List<World>();

                // if we need members world, require it be member
                if (requiresMemberWorld)
                    filteredWorlds = skillRelevantWorlds
                        .Where(x => x.MembersOnly)
                        .ToList();

                // if we cannot join member worlds, filter them out.
                if (!canJoinMemberWorlds)
                    filteredWorlds = skillRelevantWorlds
                        .Where(x => !x.MembersOnly)
                        .ToList();

                // Otherwise we can join whatever skill approperiate
                else filteredWorlds = skillRelevantWorlds;

                // If we still have a match, return that world
                if (filteredWorlds.Count > 0)
                    return filteredWorlds.First().Number;
            }

            /// STILL HERE - Not eligible for restricted world. 
            // Select random member/free world without restrictions
            List<int> viableWorlds = new List<int>();
            viableWorlds.AddRange(acc.IsMember ? MemberWorldsWithoutRestrictionOrPreference : FreeToPlayWorldsWithoutRestrictionOrPreference);

            // Return a random world
            int rand = RandomHelper.GetRandom(0, viableWorlds.Count() - 1);
            return viableWorlds[rand];
        }

        public struct World
        {
            public World(int number, int minSkill, int maxSkill, bool membersOnly)
            {
                Number = number;
                MinSkill = minSkill;
                MaxSkill = maxSkill;
                MembersOnly = membersOnly;
            }

            public int Number { get; set; }
            public int MinSkill { get; set; }
            public int MaxSkill { get; set; }
            public bool MembersOnly { get; set; }
        }

        public static readonly List<World> RestrictedWorlds = new List<World>()
        {
            // 500 skill
            new World(168, 500, 749, false),
            new World(127, 500, 749, false),
            new World(119, 500, 749, false),
            new World(113, 500, 749, false),
            new World(81, 500, 749, false),

            // 750 skill
            new World(230, 750, 1249, false),
            new World(132, 750, 1249, false),
            new World(114, 750, 1249, false),
            new World(85, 750, 1249, false),
            new World(72, 750, 1249, false),

            // 1250 skill (member only from here)
            new World(53, 1250, 1499, true),
            new World(64, 1250, 1499, true),
            new World(129, 1250, 1499, true),
            new World(147, 1250, 1499, true),
            new World(229, 1250, 1499, true),

            // 1500 skill
            new World(66, 1500, 1749, true),
            new World(116, 1500, 1749, true),
            new World(120, 1500, 1749, true),
            new World(148, 1500, 1749, true),
            new World(228, 1500, 1749, true),

            // 1750 skill
            new World(73, 1750, 1999, true),
            new World(91, 1750, 1999, true),
            new World(149, 1750, 1999, true),
            new World(167, 1750, 1999, true),

            // 2000 skill
            new World(49, 2000, 2199, true),
            new World(61, 2000, 2199, true),
            new World(96, 2000, 2199, true),
            new World(128, 2000, 2199, true),
            new World(227, 2000, 2199, true),

            // 2200 skill
            new World(63, 2200, 99999, true),
            new World(115, 2200, 99999, true),
            new World(150, 2200, 99999, true),
            new World(226, 2200, 99999, true),
        };

        private static readonly int[] MemberWorldsWithoutRestrictionOrPreference = new int[]
        {
            3,
            12,
            13,
            15,
            17,
            20,
            24,
            28,
            29,
            31,
            32,
            36,
            38,
            39,
            40,
            41,
            43,
            47,
            48,
            50,
            51,
            59,
            60,
            67,
            68,
            121,
            122,
            143,
            144,
            145,
            146,
            163,
            164,
            177,
            178,
            179,
            180,
            181,
            182,
            184,
            185,
            186,
            187,
            188,
            189,
            190,
            205,
            206,
            207,
            208,
            209,
            210,
            211,
            217,
            218,
            219,
            220,
            221,
            222,
            223,
            224,
            225,
            231,
            232,
            234,
            235,

        };

        private static readonly int[] FreeToPlayWorldsWithoutRestrictionOrPreference = new int[]
        {
            35,
            79,
            80,
            82,
            84,
            93,
            97,
            98,
            99,
            100,
            117,
            118,
            126,
            130,
            131,
            133,
            134,
            135,
            136,
            137,
            151,
            152,
            153,
            154,
            155,
            156,
            170,
            171,
            172,
            173,
            183,
            197,
            198,
            199,
            200,
            201,

        };
    }
}
