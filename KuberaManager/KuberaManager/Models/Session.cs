using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace KuberaManager.Models
{
    public class Session
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Account")]
        public int AccountId { get; set; }

        [AllowNull]
        public string RspeerSession { get; set; }


        [AllowNull]
        [ForeignKey("Computer")]
        public int ActiveComputer { get; set; }


        [Required]
        public TimeSpan TargetDuration { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [AllowNull]
        public DateTime EndTime { get; set; }


    }
}
