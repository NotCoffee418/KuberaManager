using KuberaManager.Models.Data.Ajax;
using Microsoft.EntityFrameworkCore;
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

        [Required]
        public DateTime Timestamp { get; set; }


        public static void AddEntry(int sessionId, string message)
        {
            int textId = EventLogText.GetId(message);
            using (var db = new kuberaDbContext())
            {
                db.EventLogs.Add(new EventLog()
                {
                    SessionId = sessionId,
                    TextId = textId,
                    Timestamp = DateTime.Now
                });
                db.SaveChanges();
            }
        }

        public static List<EventLogDisplayFormat> GetSessionDisplayLogs(int sessionId)
        {
            // Get account name for session
            Session sess = Session.FromId(sessionId);
            Account acc = Account.FromId(sess.AccountId);
            string accName = acc.Login;

            // Prepare result
            var result = new List<EventLogDisplayFormat>();
            List<EventLog> relevantEventLogs = null;
            using (var db = new kuberaDbContext())
            {
                // Get session logs
                relevantEventLogs = db.EventLogs
                    .Where(x => x.SessionId == sessionId)
                    .OrderByDescending(x => x.Id)
                    .ToList();
            }

            // Stick them in a EventLogDisplayFormat
            foreach (var el in relevantEventLogs)
                result.Add(new EventLogDisplayFormat()
                {
                    EventLogId = el.Id,
                    SessionId = sessionId,
                    Account = accName,
                    Text = EventLogText.FromId(el.TextId),
                    Timestamp = el.Timestamp
                });

            // done
            return result;
        }
    }
}
