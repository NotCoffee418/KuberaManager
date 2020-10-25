using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KuberaManager.Models.Data.RspeerApiStructure
{
    public class ConnectedClients : List<ConnectedClients.ClientData>
    {
        public struct ClientData
        {
            public string machineName { get; set; }
            public string runescapeEmail { get; set; }
            public string tag { get; set; }

            // Additional info:
            // lastUpdate "2020-10-17T23:36:48.3164155+00:00"
            // proxyIp
            // scriptName
            // rsn
            // "game": 0
        }
    }
}
