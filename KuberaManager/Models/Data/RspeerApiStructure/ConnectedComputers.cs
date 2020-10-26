using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KuberaManager.Models.Data.RspeerApiStructure
{
    public class ConnectedComputers : Dictionary<string, ConnectedComputers.ConnectedComputerData>
    {
        public class ConnectedComputerData
        {
            public string ip { get; set; }
            public string host { get; set; }
            public string platform { get; set; }
            public string type { get; set; }
            public dynamic userInfo { get; set; }

            // Additional Info
            // "platform": "Windows_NT",
            // "type": "Windows_NT",
            // "userInfo": {
            //     "username": "WINDOWSLINUXUSERNAME"
            // }
        }
    }
}
