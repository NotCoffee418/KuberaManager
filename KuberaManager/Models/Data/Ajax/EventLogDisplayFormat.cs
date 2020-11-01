using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KuberaManager.Models.Data.Ajax
{
    public class EventLogDisplayFormat
    {
        public int EventLogId { get; set; }

        public int SessionId { get; set; }

        public string Account { get; set; }

        public string Text { get; set; }

        public DateTime Timestamp { get; set; }
    }
}
