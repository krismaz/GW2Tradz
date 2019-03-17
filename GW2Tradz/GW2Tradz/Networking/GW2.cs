using GW2Tradz.Viewmodels;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GW2Tradz.Networking
{
    class GW2
    {
        private HttpClient httpClient = new HttpClient();
        private JsonSerializerSettings jsonSettings = new JsonSerializerSettings { ContractResolver = new DefaultContractResolver { NamingStrategy = new SnakeCaseNamingStrategy() } };

        public List<Dye> FetchDyes()
        {
            var result = httpClient.GetAsync("https://api.guildwars2.com/v2/colors?ids=all").Result; //worst coding practice or worsest coding practice
            var content = result.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<List<Dye>>(content, jsonSettings);

        }

        public List<Recipe> FetchRecipes()
        {
            var result = httpClient.GetAsync("https://api.guildwars2.com/v2/recipes").Result; //worst coding practice or worsest coding practice
            var content = result.Content.ReadAsStringAsync().Result;
            var ids =  JsonConvert.DeserializeObject<List<int>>(content, jsonSettings);
            var recipes = new List<Recipe> { }; 
            foreach(var chunk in ids.Chunk(200))
            {
                var result2 = httpClient.GetAsync("https://api.guildwars2.com/v2/recipes?ids=" + string.Join(",", chunk)).Result; //worst coding practice or worsest coding practice
                var content2 = result2.Content.ReadAsStringAsync().Result;
                recipes.AddRange( JsonConvert.DeserializeObject<List<Recipe>>(content2, jsonSettings));
            }
            return recipes;
        }




    }
}
