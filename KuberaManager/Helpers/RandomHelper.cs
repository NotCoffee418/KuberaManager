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
            return rand.Next(minValue, maxValue);
        }
    }
}
