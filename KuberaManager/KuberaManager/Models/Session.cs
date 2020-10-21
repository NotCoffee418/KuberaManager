using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        public string RspeerSessionTag { get; set; }


        [AllowNull]
        [ForeignKey("Computer")]
        public int ActiveComputer { get; set; }


        [Required]
        public TimeSpan TargetDuration { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [AllowNull]
        public DateTime LastUpdateTime { get; set; }

        [DefaultValue(true)]
        public bool IsFinished { get; set; }

        #region Static
        /// <summary>
        /// Find Session based on RS login email
        /// </summary>
        /// <param name="runescapeaccount"></param>
        /// <returns>null or account</returns>
        public static Session FromAccount(string runescapeaccount)
        {
            // todos
            // ensure latest session only
            // ensure session id makes sense (no ancient ID)
            // Create session if it doesn't exist (and fill in id threaded)
            throw new NotImplementedException();
        }

        #endregion

        #region Non-static
        public bool ShouldStop()
        {
            return StartTime + TargetDuration > DateTime.Now;
        }

        #endregion
    }
}
