using KuberaManager.Models.Database;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace KuberaManager.Models.Logic
{
    public class DiscordHandler
    {
        static HttpClient client = new HttpClient();

        public static void PostMessage(string msg) => PostMessageTask(msg).GetAwaiter().GetResult();

        private static async Task PostMessageTask(string msg)
        {
            string webhookToken = Config.Get<string>("DiscordApiKey");
            string json = JsonConvert.SerializeObject(new Dictionary<string, string>
            {
                { "content", msg },
            });
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("https://discordapp.com/api/webhooks/" + webhookToken, content);

            var responseString = await response.Content.ReadAsStringAsync();
        }
    }
}
