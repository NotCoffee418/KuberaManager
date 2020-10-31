using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KuberaManager.Models.Data.KuberaCommStructure.Update
{
    /// <summary>
    /// JSON that will be returned to RSPeer Clients.
    /// Tells clients what to do
    /// </summary>
    public class OutputFormat
    {

        // Empty or error descriptiopn
        public List<string> Errors { get; set; } = new List<string>();

        // Which scenario should be running. Null is acceptable.
        public string Instruction { get; set; }

        // Additional Information
        // vague documentation can be found in java ResponseFormat comments
        public Dictionary<string, dynamic> Data { get; set; } = new Dictionary<string, dynamic>();
    }
}
