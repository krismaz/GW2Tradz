using GW2Tradz.Viewmodels;
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
            var result = httpClient.GetAsync("https://api.silveress.ie/gw2/v1/items/json?beautify=min&fields=id,buy_price,sell_price,name,rarity,vendor_value,week_buy_velocity,week_sell_velocity,year_sell_avg").Result; //worst coding practice or worsest coding practice
            var content = result.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<List<Item>>(content, jsonSettings);
        }

        public List<History> FetchHistory()
        {
            var OneWeek = (DateTime.Now - TimeSpan.FromDays(8)).ToString("yyyy-MM-dd");
            var result = httpClient.GetAsync($"https://api.silveress.ie/gw2/v1/history?beautify=min&start={OneWeek}&fields=id,date,buy_velocity,sell_velocity").Result; //worst coding practice or worsest coding practice
            var content = result.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<List<History>>(content, jsonSettings);
        }
    }
}
