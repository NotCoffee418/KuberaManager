using KuberaManager.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace KuberaManager.Models.Data.RspeerApiStructure
{
    public class BotLauncherRequest
    {
        public BotLauncherRequest(Account account, Computer computer, int _world = -1, string overrideScriptStartupArgs = "")
        {
            // Define script startup args
            string scriptStartupArgs = overrideScriptStartupArgs == "" ?
                "-IsApiSession True" : overrideScriptStartupArgs;

            // Prepare clientstructure
            ClientStructure clientStruct = new ClientStructure()
            {
                rsUsername = account.Login,
                rsPassword = account.Password,
                world = _world,
                config = new ConfigStruct(computer),
                script = new ScriptStruct(scriptStartupArgs)
            };

            // Store Request data
            socket = computer.GetTag();
            payload = new PayloadStructure()
            {
                qs = new QsStructure(clientStruct)
            };
        }

        public class PayloadStructure
        {
            public string type = "start:client";
            public QsStructure qs = null; // define in BotLauncherRequest constructor
            public string jvmArgs = "-Xmx768m -Djava.net.preferIPv4Stack=true -Djava.net.preferIPv4Addresses=true -Xss2m";
            public int game = 0;
        }

        public class QsStructure
        {
            public QsStructure(ClientStructure c)
            {
                clients.Add(c);
            }

            public List<ClientStructure> clients = new List<ClientStructure>();
        }

        public class ClientStructure
        {
            public string rsUsername { get; set; }
            public string rsPassword { get; set; }
            public int world { get; set; } = -1;
            public ProxyStructure proxy = null; // NIY
            public ScriptStruct script { get; set; }
            public ConfigStruct config { get; set; }
        }

        public class ProxyStructure
        {
            //public int proxyId = null;
            //"date": "0001-01-01T00:00:00+00:00",
            //"userId": null,
            //"name": "",
            //"ip": null,
            //"port": null,
            //"username": null,
            //"password": null
        }

        public class ScriptStruct
        {
            public ScriptStruct(string _scriptArgs)
            {
                this.scriptArgs = _scriptArgs;
            }

            public string scriptArgs { get; set; } // startup args
            public string name { get; } = "Kubera";
            public string scriptId { get; set; } = null;
            public bool isRepoScript { get; } = false;
        }

        public class ConfigStruct
        {
            public ConfigStruct(Computer comp)
            {
                lowCpuMode = comp.LowCpuMode;
                superLowCpuMode = comp.SuperLowCpuMode;
                disableModelRendering = comp.DisableModelRendering;
                disableSceneRendering = comp.DisableSceneRendering;
            }

            public bool lowCpuMode { get; set; }
            public bool superLowCpuMode { get; set; }
            public bool disableModelRendering { get; set; }
            public bool disableSceneRendering { get; set; }
            public int engineTickDelay { get; set; } = 0;
        }

        public string socket { get; set; }
        public PayloadStructure payload { get; set; }
    }
}
