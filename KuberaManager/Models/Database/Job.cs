﻿using KuberaManager.Models.Logic.ScenarioLogic.Scenarios;
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
        public string ScenarioIdentifier { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public TimeSpan TargetDuration { get; set; }

        [Required]
        public bool ForceRunUntilComplete { get; set; } = false;

        [Required]
        [DefaultValue(false)]
        public bool IsFinished { get; set; } = false;

        public bool ShouldStop()
        {
            // Job is past expiry date and expiry date is relevant
            // NOTE: Does not factor in session time.
            DateTime endTime = StartTime.Add(TargetDuration);
            if (!ForceRunUntilComplete && endTime < DateTime.Now)
            {
                // Mark current job as finished
                using (var db = new kuberaDbContext())
                {
                    IsFinished = true;
                    db.Update(this);
                    db.SaveChanges();
                }

                // Return status
                return true;
            }
            // Still here, no close conditions met. Keep going.
            else return false;
        }
    }
}
