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
    class GW2Profits
    {
        private HttpClient httpClient = new HttpClient();
        private JsonSerializerSettings jsonSettings = new JsonSerializerSettings { ContractResolver = new DefaultContractResolver { NamingStrategy = new SnakeCaseNamingStrategy() } };

        public List<Recipe> FetchRecipes()
        {
            var result = httpClient.GetAsync("http://gw2profits.com/json/v3").Result; //worst coding practice or worsest coding practice
            var content = result.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<List<Recipe>>(content, jsonSettings);
        }

    }
}
