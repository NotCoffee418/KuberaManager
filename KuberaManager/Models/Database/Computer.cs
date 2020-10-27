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
        [DefaultValue(false)]
        [Display(Description = "Indicates if bots should be automatically assigned to this computer")]
        public bool IsEnabled { get; set; }

        [Required]
        [DefaultValue(3)]
        [Display(Name = "Max Clients", Description = "0 for unlimited")]
        public int MaxClients { get; set; } = 3;

        [DefaultValue(true)]
        public bool LowCpuMode { get; set; } = true;

        [DefaultValue(true)]
        public bool SuperLowCpuMode { get; set; } = true;

        [DefaultValue(true)]
        public bool DisableModelRendering { get; set; } = true;

        [DefaultValue(true)]
        public bool DisableSceneRendering { get; set; } = true;


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
                $"New computer '{hostName}' has been added to the database. You must manually enable & configure it before it is used.");
            return comp;
        }

        public static Computer ById(int id)
        {
            using (var db = new kuberaDbContext())
            {
                return db.Computers
                    .Where(x => x.Id == id)
                    .FirstOrDefault();
            }
        }


        /// <summary>
        /// Returns an available computer to use for a new session
        /// </summary>
        /// <returns>computer or null</returns>
        public static Computer GetAvailableComputer()
        {
            var connectedComputers = ClientManager.GetConnectedComputers();
            Computer result = null;
            using (var db = new kuberaDbContext())
            {
                result = db.Computers
                    // Only if confifured to be on
                    .Where(x => x.IsEnabled)

                    // Only if we haven't reached the max allowed sessions on that computer yet
                    .Where(x => db.Sessions
                        .Where(y => y.IsFinished == false)
                        .Where(y => y.ActiveComputer == x.Id)
                        .Count() < x.MaxClients)

                    // Only if the computer is currently connected
                    .Where(x => connectedComputers.Values
                        .Where(z => z.host == x.Hostname).Count() > 0)

                    // Return first (no priority implemented atm)
                    .FirstOrDefault();
            }
            return result;
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
