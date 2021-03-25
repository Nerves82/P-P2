using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TakeMyAdvice.Models;

namespace TakeMyAdvice
{
    public class DataAccess
    {
        private static DataAccess _instance;
        public static DataAccess Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DataAccess();
                }

                return _instance;
            }
        }

        public async Task<Response> GetInfoAsync()
        {
            using (var client = new HttpClient())
            {
                string response = "";
                try
                {
                    var uri = "https://api.adviceslip.com/advice";
                    HttpResponseMessage responseMessage = await client.GetAsync(uri).ConfigureAwait(false);
                    response = await responseMessage.Content.ReadAsStringAsync();

                    var result = JsonConvert.DeserializeObject<IDictionary<string, object>>(response);

                    if (result.ContainsKey("slip"))
                    {
                        return JsonConvert.DeserializeObject<Response>(response);
                    }
                    return null;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        public async Task AddToBadAdviceAsync(Response advice)
        {
            var realm = Realms.Realm.GetInstance();
            await realm.WriteAsync((r) =>
             {
                 r.Add(new AdviceModel { AdviceNumber = advice.Slip.Id, AdviceType = "bad", AdviceMessage = advice.Slip.Advice }, update: true);
             });
        }

        public async Task AddToGoodAdviceAsync(Response advice)
        {
            var realm = Realms.Realm.GetInstance();
            await realm.WriteAsync((r) =>
            {
                r.Add(new AdviceModel { AdviceNumber = advice.Slip.Id, AdviceType = "good", AdviceMessage = advice.Slip.Advice }, update: true);
            });
        }

        public partial class Response
        {
            [JsonProperty("slip")]
            public Slip Slip { get; set; }
        }

        public partial class Slip
        {
            [JsonProperty("id")]
            public long Id { get; set; }

            [JsonProperty("advice")]
            public string Advice { get; set; }
        }


    }
}
