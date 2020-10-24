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
        public static void StartClient(Account account, Computer computer)
        {
            throw new NotImplementedException();
            // API key in request functions!
        }

        public static void StopClient(Session session)
        {
            throw new NotImplementedException();
            // API key in request functions!

        }

        // Store in memory during this request
        private static ConnectedComputers storedConnectedComputers = null;
        public static ConnectedComputers GetConnectedComputers()
        {
            if (storedConnectedComputers == null)
            {
                string response = RspeerGetRequest("api/botLauncher/connected")
                    .GetAwaiter().GetResult();

                storedConnectedComputers = JsonConvert.DeserializeObject<ConnectedComputers>(response);
            }
            return storedConnectedComputers;
        }

        // Store in memory during this request
        private static ConnectedClients storedConnectedClients = null;
        public static ConnectedClients GetConnectedClients()
        {
            if (storedConnectedClients == null)
            {
                string response = RspeerGetRequest("api/botLauncher/connectedClients")
                    .GetAwaiter().GetResult();

                var data = JsonConvert.DeserializeObject<ConnectedClients>(response);
            }
            return storedConnectedClients;
        }


        HttpClient client = new HttpClient();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="path">eg. api/botLauncher/connected</param>
        /// <param name="data"></param>
        /// <returns></returns>
        private static async Task RspeerPostRequest(string path, dynamic data)
        {
            string json = JsonConvert.SerializeObject(data);
            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage(HttpMethod.Post, new Uri($"https://services.rspeer.org/{path}"));
                request.Headers.Add("ApiClient", Config.Get<string>("RspeerApiKey1"));
                request.Content = new StringContent(json, Encoding.UTF8, "application/json"); ;
                var response = await client.SendAsync(request);
            }
        }

        private static async Task<dynamic> RspeerGetRequest(string path)
        {
            dynamic result;
            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage(HttpMethod.Get, new Uri($"https://services.rspeer.org/{path}"));
                request.Headers.Add("ApiClient", Config.Get<string>("RspeerApiKey1"));
                var response = await client.SendAsync(request);
                string json = await response.Content.ReadAsStringAsync();
                result = JsonConvert.DeserializeObject(json);
            }
            return result;
        }
    }
}
