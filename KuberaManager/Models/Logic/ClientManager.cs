using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using KuberaManager.Models.Data.RspeerApiStructure;
using KuberaManager.Models.Database;
using Newtonsoft.Json;

namespace KuberaManager.Models.Logic
{
    public class ClientManager
    {
        public static void StartClient(Account account, Computer computer, Session session, int world = -1, bool isManualSession = false)
        {
            // Prepare data
            BotLauncherRequest req = new BotLauncherRequest(account, computer, session, world, isManualSession);
            
            // Send request
            RspeerPostRequest("api/botLauncher/send", req).GetAwaiter().GetResult();
        }

        public static void StopClient(Session session)
        {
            // Stop request only uses URL, no body
            RspeerPostRequest($"api/botLauncher/sendNew?message=:kill&tag={session.RspeerSessionTag}", null)
                .GetAwaiter().GetResult();
        }


        public static ConnectedComputers GetConnectedComputers()
        {
            return RspeerGetRequest<ConnectedComputers>
                    ("api/botLauncher/connected").GetAwaiter().GetResult();
        }

        public static ConnectedClients GetConnectedClients()
        {
            return RspeerGetRequest<ConnectedClients>
                    ("api/botLauncher/connectedClients").GetAwaiter().GetResult();
        }


        readonly HttpClient client = new HttpClient();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="path">eg. api/botLauncher/connected</param>
        /// <param name="data"></param>
        /// <returns></returns>
        private static async Task RspeerPostRequest(string path, dynamic data)
        {
            // Send spoof request
            if (!Config.Get<bool>("AllowRspeerApiCalls"))
            {
                SpoofRequest("Post", path, data);
                return;
            }

            // Send request
            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage(HttpMethod.Post, new Uri($"https://services.rspeer.org/{path}"));
                request.Headers.Add("ApiClient", Config.Get<string>("RspeerApiKey1"));
                if (data != null)
                {
                    string json = JsonConvert.SerializeObject(data);
                    request.Content = new StringContent(json, Encoding.UTF8, "application/json"); ;
                }
                await client.SendAsync(request);
            }
        }

        private static async Task<T> RspeerGetRequest<T>(string path)
        {
            // Send spoof request
            if (!Config.Get<bool>("AllowRspeerApiCalls"))
            {
                SpoofRequest("Get", path, null);
                // NOT RETURNING, SENDING REGARDLESS
            }

            // Send request
            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage(HttpMethod.Get, new Uri($"https://services.rspeer.org/{path}"));
                request.Headers.Add("ApiClient", Config.Get<string>("RspeerApiKey1"));
                var response = await client.SendAsync(request);
                string json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(json);
            }
        }

        // Sends a discord message instead of a real request
        private static void SpoofRequest(string type, string path, dynamic data)
        {
            // Prepare msg
            string msg = $"Spoof RSPeer Api Request: {type}" + Environment.NewLine +
                $"Path: {path}" + Environment.NewLine +
                $"Data: " + (data == null ? "" : JsonConvert.SerializeObject(data)) +
                (type == "Get" ? "WARN: Get requests do go through" : "");

            DiscordHandler.PostMessage(msg);
        }
    }
}
