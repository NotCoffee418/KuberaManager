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
                if (txt == null)
                {
                    txt = new EventLogText()
                    {
                        Text = text
                    };

                    db.Attach(txt);
                    db.EventLogTexts.Add(txt);
                    db.SaveChanges();
                }
            }

            // Return result or try again if newly created
            return txt == null ? GetId(text) : txt.Id;
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
