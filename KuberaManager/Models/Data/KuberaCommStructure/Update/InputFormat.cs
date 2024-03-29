﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace KuberaManager.Models.Data.KuberaCommStructure.Update
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
        /// stopping
        /// update (no changes, just phoning in)
        /// discord-notify
        /// 
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Nullable, required for some statuses
        /// </summary>
        public JsonElement Details { get; set; }

        /// <summary>
        /// The session ID for the request
        /// </summary>
        public int Session { get; set; }

        internal bool HasValidStatus()
        {
            string[] validStatuses = new string[]
            {
                "stopping",
                "update",
                "discord-notify",
                "report-banned"
            };
            return validStatuses.Contains(Status);
        }

        /// <summary>
        /// Use to return value from the Details dictionary
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T GetDetails<T>()
        {
            return JsonConvert.DeserializeObject<T>(this.Details.GetRawText());
        }
    }
}
