using GW2Tradz.Networking.viewmodels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GW2Tradz.Networking
{
    class Silveress
    {
        private HttpClient httpClient = new HttpClient();
        private JsonSerializerSettings jsonSettings = new JsonSerializerSettings { ContractResolver = new DefaultContractResolver { NamingStrategy = new SnakeCaseNamingStrategy() } };

    public List<Item> FetchBasicInfo()
        {
            var result = httpClient.GetAsync("https://api.silveress.ie/gw2/v1/items/json").Result; //worst coding practice or worsest coding practice
            var content = result.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<List<Item>>(content, jsonSettings);
        }
    }
}
