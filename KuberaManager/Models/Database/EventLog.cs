using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace KuberaManager.Models.Database
{
    public class EventLog
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int SessionId { get; set; }

        [ForeignKey("EventLogText")]
        [Required]
        public int TextId { get; set; }


        public static void AddEntry(int sessionId, string message)
        {
            int textId = EventLogText.GetId(message);
            using (var db = new kuberaDbContext())
            {
                db.EventLogs.Add(new EventLog()
                {
                    SessionId = sessionId,
                    TextId = textId
                });
            }
        }
    }
}
