using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KuberaManager.Models.Data
{
    public class ConfigData
    {
        public struct KeyData
        {
            public KeyData(string name, int confVersion)
            {
                Name = name;
                ConfigVersion = confVersion;
            }
            public string Name;
            public int ConfigVersion;
        }

        /// <summary>
        /// These values are placed in the database.
        /// If DbVersion of a KeyData > InstalledConfigVersion, any new values should be installed
        /// </summary>
        public readonly List<KeyData> Keys = new List<KeyData>()
        {
            new KeyData("InstalledConfigVersion", 1),
            new KeyData("RspeerApiKey1", 1),
            new KeyData("DiscordApiKey", 1),
            new KeyData("AdminPassHash", 2),
        };
    }
}
