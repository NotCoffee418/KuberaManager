using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KuberaManager.Models.Logic.Api.Update
{
    /// <summary>
    /// POST request that will be sent by RSPeer clients
    /// </summary>
    public class InputFormat
    {


        /// <summary>
        /// Empty for no errors
        /// </summary>
        public List<string> Errors { get; set; } = new List<string>();

        /// <summary>
        /// Possible statuses:
        /// 
        /// started
        /// stopping
        /// update (no changes, just phoning in)
        /// discord-notify
        /// 
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Nullable, required for some statuses
        /// </summary>
        public string Details { get; set; }

        /// <summary>
        /// The RS account sending the request
        /// </summary>
        public string RunescapeAccount { get; set; }
    }
}
