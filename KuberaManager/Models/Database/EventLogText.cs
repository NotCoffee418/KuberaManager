using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KuberaManager.Models.Database
{
    public class EventLogText
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Text { get; set; }


        // Create new text if needed and/or grab id
        public static int GetId(string text)
        {
            EventLogText txt = null;
            using (var db = new kuberaDbContext())
            {
                // Get known text id
                txt = db.EventLogTexts
                    .Where(x => x.Text == text)
                    .FirstOrDefault();

                // If it doesn't exist, create it
                db.EventLogTexts.Add(new EventLogText()
                {
                    Text = text
                });

                // Check it again
                txt = db.EventLogTexts
                    .Where(x => x.Text == text)
                    .First(); // first or exception
            }

            // Return result
            return txt.Id;
        }

        public static string FromId(int id)
        {
            try
            {
                using (var db = new kuberaDbContext())
                {
                    return db.EventLogTexts
                        .Where(x => x.Id == id)
                        .First().Text;
                }
            }
            catch
            {
                return "!UNDEFINED EVENT STRING!";
            }            
        }
    }
}
