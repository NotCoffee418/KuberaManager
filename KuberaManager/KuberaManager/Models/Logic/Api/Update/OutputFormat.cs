using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KuberaManager.Models.Logic.Api.Update
{
    /// <summary>
    /// JSON that will be returned to RSPeer Clients.
    /// Tells clients what to do
    /// </summary>
    public class OutputFormat
    {

        // Empty or error descriptiopn
        public List<string> Errors { get; set; } = new List<string>();

        // when true, client should stop
        public bool IsStopRequest { get; set; } = false;

        // Which scenario should be running
        public string Scenario { get; set; }


    }
}
