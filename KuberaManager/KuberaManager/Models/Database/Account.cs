using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace KuberaManager.Models.Database
{
    public class Account
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Login { get; set; }

        [Required]
        [PasswordPropertyText]
        public string Password { get; set; }

        [Required]
        [DefaultValue(false)]
        public bool IsBanned { get; set; }

        [Range(0, 23)]
        [DefaultValue(0)]
        [Display(Name = "Start Time of Day", Description = "Preferred time of the day to start 0-23. Logic is that an account should have consistent playing habits.")]
        public int PrefStartTimeDay { get; set; }

        [Range(0, 23)]
        [DefaultValue(23)]
        [Display(Name = "Stop Time of Day", Description = "Preferred time of the day to stop 0-23. Logic is that an account should have consistent playing habits.")]
        public int PrefStopTimeDay { get; set; }

        [Display(Name = "Preferred Activities", Description = "Will perform these actions unless manually overwritten.")]
        public List<Scenario> PreferredActivities { get; set; }

        internal static Account FromLogin(string runescapeaccount)
        {
            throw new NotImplementedException();
        }
    }
}
