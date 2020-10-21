using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
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

        public static Computer[] GetConnectedComputers()
        {
            // API key in request functions!
            throw new NotImplementedException();
        }

        public static Session[] GetActiveSessions()
        {
            throw new NotImplementedException();
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
                /////////////////request.Headers.Add("ApiClient", "fuck");
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
                /////////////////request.Headers.Add("ApiClient", "Value");
                var response = await client.SendAsync(request);
                string json = await response.Content.ReadAsStringAsync();
                result = JsonConvert.DeserializeObject(json);
            }
            return result;
        }
    }
}
