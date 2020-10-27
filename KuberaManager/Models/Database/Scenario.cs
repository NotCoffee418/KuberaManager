using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KuberaManager.Models.Database
{
    public class Scenario
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name="Name", Description = "These are hardcoded in the script.")]
        public string Name { get; set; }

        internal static Scenario FromId(int scenarioId)
        {
            using (var db = new kuberaDbContext())
            {
                return db.Scenarios
                    .Where(x => x.Id == scenarioId)
                    .FirstOrDefault();
            }
        }
    }
}
