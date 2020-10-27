using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace KuberaManager.Models.Database
{
    public class Levels
    {
        [Key]
        [ForeignKey("Account")]
        public int AccountId { get; set; }

        public int Magic { get; set; }
        public int Slayer { get; set; }
        public int Strength { get; set; }
        public int Defence { get; set; }
        public int Fletching { get; set; }
        public int Fishing { get; set; }
        public int Mining { get; set; }
        public int Herblore { get; set; }
        public int Hitpoints { get; set; }
        public int Smithing { get; set; }
        public int Woodcutting { get; set; }
        public int Prayer { get; set; }
        public int Ranged { get; set; }
        public int Attack { get; set; }
        public int Crafting { get; set; }
        public int Farming { get; set; }
        public int Firemaking { get; set; }
        public int Runecrafting { get; set; }
        public int Construction { get; set; }
        public int Cooking { get; set; }
        public int Agility { get; set; }
        public int Hunter { get; set; }
        public int Thieving { get; set; }

        public static Levels FromAccount(int id)
        {
            using (var db = new kuberaDbContext())
            {
                return db.Levels
                    .Where(x => x.AccountId == id)
                    .FirstOrDefault();
            }
        }
    }
}
