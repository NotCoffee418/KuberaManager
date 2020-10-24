using KuberaManager.Models.Logic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KuberaManager.Models.Database
{
    public class Computer
    {

        [Key]
        public int Id { get; set; }

        [Required]
        public string Hostname{ get; set; }

        [Required]
        [DefaultValue(3)]
        [Display(Name = "Max Clients", Description = "0 for unlimited")]
        public int MaxClients { get; set; }

        [DefaultValue(true)]
        public bool LowCpuMode { get; set; }

        [DefaultValue(true)]
        public bool SuperLowCpuMode { get; set; }

        [DefaultValue(true)]
        public bool DisableModelRendering { get; set; }

        [DefaultValue(true)]
        public bool DisableSceneRendering { get; set; }

        public static Computer ByHostname(string hostName)
        {
            // Try to return a known computer
            Computer comp = null;
            using (var db = new kuberaDbContext())
            {
                comp = db.Computers
                    .Where(x => x.Hostname == hostName)
                    .FirstOrDefault();
                if (comp != null)
                    return comp;
                else
                {
                    /// Computer didn't exist. Creating it.
                    comp = new Computer()
                    {
                        Hostname = hostName
                    };
                    db.Computers.Add(comp);
                    db.SaveChanges();
                }
            }

            // Send discord notification if this computer is new
            DiscordHandler.PostMessage(
                $"New computer '{hostName}' has been added to the database. Change the config in KuberaManager as needed.");
            return comp;
        }

        /// <summary>
        /// Get the computer's current unique indentifier
        /// </summary>
        /// <returns></returns>
        public string GetTag()
        {
            return ClientManager.GetConnectedComputers()
                .Where(x => x.Value.host == Hostname)
                .FirstOrDefault().Key;
        }
    }
}
