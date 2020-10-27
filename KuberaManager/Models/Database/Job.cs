using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace KuberaManager.Models.Database
{
    public class Job
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Session")]
        public int SessionId { get; set; }

        [AllowNull]
        [ForeignKey("Scenario")]
        public int ActiveScenarioId { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public TimeSpan TargetDuration { get; set; }

        [Required]
        [DefaultValue(false)]
        public bool IsFinished { get; set; } = false;



        public Session SessionObj 
        { get { return Session.FromId(SessionId); } }
        public Scenario ActiveScenarioObj 
        { get { return Scenario.FromId(ActiveScenarioId); } }


        public static Job ForceSetNewJob()
        {
            throw new NotImplementedException();
        }
    }
}
