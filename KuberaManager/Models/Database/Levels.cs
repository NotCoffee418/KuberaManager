using KuberaManager.Models.Data.Runescape;
using Microsoft.EntityFrameworkCore;
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
        [Column("AccountId", Order = 0)]
        public int AccountId { get; set; }

        [Column("Attack", Order = 1)]
        public int Attack { get; set; }

        [Column("Hitpoints", Order = 2)]
        public int Hitpoints { get; set; }

        [Column("Mining", Order = 3)]
        public int Mining { get; set; }

        [Column("Strength", Order = 4)]
        public int Strength { get; set; }

        [Column("Agility", Order = 5)]
        public int Agility { get; set; }

        [Column("Smithing", Order = 6)]
        public int Smithing { get; set; }

        [Column("Defence", Order = 7)]
        public int Defence { get; set; }

        [Column("Herblore", Order = 8)]
        public int Herblore { get; set; }

        [Column("Fishing", Order = 9)]
        public int Fishing { get; set; }

        [Column("Ranged", Order = 10)]
        public int Ranged { get; set; }

        [Column("Thieving", Order = 11)]
        public int Thieving { get; set; }

        [Column("Cooking", Order = 12)]
        public int Cooking { get; set; }

        [Column("Prayer", Order = 13)]
        public int Prayer { get; set; }

        [Column("Crafting", Order = 14)]
        public int Crafting { get; set; }

        [Column("Firemaking", Order = 15)]
        public int Firemaking { get; set; }

        [Column("Magic", Order = 16)]
        public int Magic { get; set; }

        [Column("Fletching", Order = 17)]
        public int Fletching { get; set; }

        [Column("Woodcutting", Order = 18)]
        public int Woodcutting { get; set; }

        [Column("Runecrafting", Order = 19)]
        public int Runecrafting { get; set; }

        [Column("Slayer", Order = 20)]
        public int Slayer { get; set; }

        [Column("Farming", Order = 21)]
        public int Farming { get; set; }

        [Column("Construction", Order = 22)]
        public int Construction { get; set; }

        [Column("Hunter", Order = 23)]
        public int Hunter { get; set; }


        public void Save()
        {
            if (AccountId == 0)
                throw new Exception("Account ID must be defined to store levels");

            using (var db = new kuberaDbContext())
            {
                try // Dodgy save method
                {
                    db.Levels.Update(this);
                    db.SaveChanges();
                }
                // Doesn't exist yet. Insert instead
                catch (DbUpdateConcurrencyException)
                {
                    db.Levels.Add(this);
                    db.SaveChanges();
                }
            }
        }

        public int GetTotalLevel()
        {
            return Agility + Attack + Construction + Cooking + Crafting + Defence + Farming +
                Firemaking + Fishing + Fletching + Herblore + Hitpoints + Hunter + Magic +
                Mining + Prayer + Ranged + Runecrafting + Slayer + Smithing + Strength +
                Thieving + Woodcutting;
        }

        public int GetSkillLevel(Skill skill)
        {
            switch (skill)
            {
                case Skill.Attack:
                    return Attack;
                case Skill.Hitpoints:
                    return Hitpoints;
                case Skill.Mining:
                    return Mining;
                case Skill.Strength:
                    return Strength;
                case Skill.Agility:
                    return Agility;
                case Skill.Smithing:
                    return Smithing;
                case Skill.Defence:
                    return Defence;
                case Skill.Herblore:
                    return Herblore;
                case Skill.Fishing:
                    return Fishing;
                case Skill.Ranged:
                    return Ranged;
                case Skill.Thieving:
                    return Thieving;
                case Skill.Cooking:
                    return Cooking;
                case Skill.Prayer:
                    return Prayer;
                case Skill.Crafting:
                    return Crafting;
                case Skill.Firemaking:
                    return Firemaking;
                case Skill.Magic:
                    return Magic;
                case Skill.Fletching:
                    return Fletching;
                case Skill.Woodcutting:
                    return Woodcutting;
                case Skill.Runecrafting:
                    return Runecrafting;
                case Skill.Slayer:
                    return Slayer;
                case Skill.Farming:
                    return Farming;
                case Skill.Construction:
                    return Construction;
                case Skill.Hunter:
                    return Hunter;
                default:
                    throw new Exception("This skill is not implemented.");
            }
        }


        #region Static Methods
        public static Levels FromAccount(int id)
        {
            using (var db = new kuberaDbContext())
            {
                return db.Levels
                    .Where(x => x.AccountId == id)
                    .FirstOrDefault();
            }
        }
        #endregion
    }
}
