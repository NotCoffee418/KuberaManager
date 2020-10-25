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
            public KeyData(string name, int confVersion, string details = null, bool hidden = false)
            {
                Name = name;
                ConfigVersion = confVersion;
                Hidden = hidden;
                Details = details;
            }
            public string Name;
            public int ConfigVersion;
            public bool Hidden;
            public string Details;
        }

        /// <summary>
        /// These values are placed in the database.
        /// If DbVersion of a KeyData > InstalledConfigVersion, any new values should be installed
        /// </summary>
        public readonly List<KeyData> Keys = new List<KeyData>()
        {
            new KeyData("InstalledConfigVersion", 1, hidden:true),
            new KeyData("RspeerApiKey1", 1),
            new KeyData("DiscordApiKey", 1, "Channel webhook"),
            new KeyData("AdminPassHash", 2, "Edit brings you to change password page"),
            new KeyData("MaxHoursPerDay", 3, "Maximum hours per day an account will run"),
        };
    }
}
