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
            public KeyData(string name, int confVersion, string defaultValue, string details = null, bool hidden = false)
            {
                Name = name;
                ConfigVersion = confVersion;
                Hidden = hidden;
                Details = details;
                DefaultValue = defaultValue;
            }
            public string Name;
            public int ConfigVersion;
            public bool Hidden;
            public string Details;
            public string DefaultValue;
        }

        /// <summary>
        /// These values are placed in the database.
        /// If DbVersion of a KeyData > InstalledConfigVersion, any new values should be installed
        /// </summary>
        public static readonly List<KeyData> Keys = new List<KeyData>()
        {
            new KeyData("InstalledConfigVersion", 1, "0", hidden:true),
            new KeyData("RspeerApiKey1", 1, null),
            new KeyData("DiscordApiKey", 1, null, "Channel webhook (https://discordapp.com/api/webhooks/ThisPartOnly)"),
            new KeyData("AdminPassHash", 2, null, "Edit brings you to change password page"),
            new KeyData("MaxHoursPerDay", 3, "8", "Maximum hours per day an account will run."),
            new KeyData("DeveloperMode", 4, "False", "Enables dev tools for creating spoof data (True/False)"),
            new KeyData("BrainEnabled", 5, "False", "Determines if bots should start automatically (True/False)"),
        };

        public static string GetDescription(string confKey)
        {
            KeyData? kd = Keys.Where(x => x.Name == confKey).FirstOrDefault();
            return kd == null ? "" : kd.Value.Details;
        }

        public static bool IsShown(string confKey)
        {
            KeyData? kd = Keys.Where(x => x.Name == confKey).FirstOrDefault();
            return kd == null ? true : !kd.Value.Hidden;
        }
    }
}
