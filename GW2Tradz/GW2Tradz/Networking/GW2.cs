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
            return new List<Dye>();
            return JsonConvert.DeserializeObject<List<Dye>>(content, jsonSettings);
        }

        public List<Recipe> FetchRecipes()
        {
            var recipes = new List<Recipe> { };
            return recipes;

            var result = httpClient.GetAsync("https://api.guildwars2.com/v2/recipes").Result; //worst coding practice or worsest coding practice
            var content = result.Content.ReadAsStringAsync().Result;
            var ids = JsonConvert.DeserializeObject<List<int>>(content, jsonSettings);
            

            foreach (var chunk in ids.Chunk(200))
            {
                var result2 = httpClient.GetAsync("https://api.guildwars2.com/v2/recipes?ids=" + string.Join(",", chunk)).Result; //worst coding practice or worsest coding practice
                var content2 = result2.Content.ReadAsStringAsync().Result;
                recipes.AddRange(JsonConvert.DeserializeObject<List<Recipe>>(content2, jsonSettings));
            }
            return recipes;
        }

        public Dictionary<int, int> FetchCurrentSells()
        {
            var maxPage = 0;
            List<Listing> listings = new List<Listing> { };
            for (int page = 0; page <= maxPage; page++)
            {
                var result = httpClient.GetAsync($"https://api.guildwars2.com/v2/commerce/transactions/current/sells?access_token={Settings.ApiKey}&page={page}").Result; //worst coding practice or worsest coding practice
                var content = result.Content.ReadAsStringAsync().Result;
                maxPage = int.Parse(result.Headers.GetValues("X-Page-Total").FirstOrDefault()) - 1;
                listings.AddRange(JsonConvert.DeserializeObject<List<Listing>>(content, jsonSettings));
            }
            return listings.GroupBy(l => l.ItemId, l => l.Quantity).ToDictionary(g => g.Key, g => g.Sum());
        }

        public Dictionary<int, int> FetchCurrentBuys()
        {
            var maxPage = 0;
            List<Listing> listings = new List<Listing> { };
            for (int page = 0; page <= maxPage; page++)
            {
                var result = httpClient.GetAsync($"https://api.guildwars2.com/v2/commerce/transactions/current/buys?access_token={Settings.ApiKey}&page={page}").Result; //worst coding practice or worsest coding practice
                var content = result.Content.ReadAsStringAsync().Result;
                maxPage = int.Parse(result.Headers.GetValues("X-Page-Total").FirstOrDefault()) - 1;
                listings.AddRange(JsonConvert.DeserializeObject<List<Listing>>(content, jsonSettings));
            }
            return listings.GroupBy(l => l.ItemId, l => l.Price * l.Quantity).ToDictionary(g => g.Key, g => g.Sum());
        }

        

        public int WalletGold()
        {
            var result = httpClient.GetAsync($"https://api.guildwars2.com/v2/account/wallet?access_token={Settings.ApiKey}").Result; //worst coding practice or worsest coding practice
            var content = result.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<List<WalletItem>>(content, jsonSettings).First((i) => i.Id == 1).Value; //id 1 = Gold
        }

        public DeliveryBox FetchDeliveryBox()
        {
            var result = httpClient.GetAsync($"https://api.guildwars2.com/v2/commerce/delivery?access_token={Settings.ApiKey}").Result; //worst coding practice or worsest coding practice
            var content = result.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<DeliveryBox>(content, jsonSettings); //id 1 = Gold
        }

        public List<ListingsData> FetchListings(IEnumerable<int> ids)
        {
            var listings = new List<ListingsData> { };
            foreach (var chunk in ids.Chunk(200))
            {
                var result = httpClient.GetAsync("https://api.guildwars2.com/v2/commerce/listings?ids=" + string.Join(",", chunk)).Result; //worst coding practice or worsest coding practice
                var content = result.Content.ReadAsStringAsync().Result;
                listings.AddRange(JsonConvert.DeserializeObject<List<ListingsData>>(content, jsonSettings));
            }
            return listings;
        }

    }
}

