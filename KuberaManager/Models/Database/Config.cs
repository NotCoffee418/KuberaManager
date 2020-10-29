using KuberaManager.Models.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace KuberaManager.Models.Database
{
    public class Config
    {
        static Config()
        {
            CheckUpdateConfigVersion();
        }

        [Key]
        public string ConfKey { get; set; }

        [AllowNull]
        public string ConfValue { get; set; }

        /// <summary>
        /// Return convertedvalue attached to confkey
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="confKey"></param>
        /// <returns>default(T) or value</returns>
        public static T Get<T>(string confKey)
        {
            var context = new kuberaDbContext();

            // Query database
            Config c = context.Configs
                .Where(x => x.ConfKey == confKey)
                .FirstOrDefault();

            // Return null or value
            TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));
            if (c == null || converter == null)
                return default(T);
            else // Convert to T & return
                return (T)converter.ConvertFrom(c.ConfValue);
        }

        public static void Set<T>(string confKey, T value)
        {
            using (var db = new kuberaDbContext())
            {
                var conf = db.Configs.SingleOrDefault(x => x.ConfKey == confKey);
                if (conf != null)
                {
                    conf.ConfValue = value.ToString();
                    db.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Should be run when opening the controller for config key/values
        /// Will add new values if there are any & set DB version to current
        /// </summary>
        public static void CheckUpdateConfigVersion()
        {
            // Get application's config version
            int applicationVersion = ConfigData.Keys
                .OrderByDescending(x => x.ConfigVersion)
                .First().ConfigVersion;

            // Det DB config version
            int databaseVersion = Config.Get<int>("InstalledConfigVersion");

            // Install new keys if needed
            if (applicationVersion > databaseVersion)
            {
                var keysToAdd = ConfigData.Keys.Where(x => x.ConfigVersion > databaseVersion);

                // Add values
                using (var db = new kuberaDbContext())
                {
                    var config = db.Set<Config>();
                    foreach (var key in keysToAdd)
                        config.Add(new Config { ConfKey = key.Name, ConfValue = key.DefaultValue});
                    db.SaveChanges();
                }

                // Update database version
                Set<int>("InstalledConfigVersion", applicationVersion);
            }
        }
    }
}
