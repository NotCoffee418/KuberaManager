using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KuberaManager.Helpers
{
    public class RandomHelper
    {
        private static Random rand = new Random();

        // todo: better random
        public static int GetRandom(int minValue, int maxValue)
        {
            if (maxValue == -1 && minValue == 0)
                return 0; // There is no array content, not our problem

            return rand.Next(minValue, maxValue);
        }
    }
}
