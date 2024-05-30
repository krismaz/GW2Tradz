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
using System.Windows;

namespace GW2Tradz.Networking
{
    class GW2Profits
    {
        private JsonSerializerSettings jsonSettings = new JsonSerializerSettings { ContractResolver = new DefaultContractResolver { NamingStrategy = new SnakeCaseNamingStrategy() } };

        public List<Recipe> FetchRecipes()
        {
            try
            {
                var handler = new HttpClientHandler();
                handler.ClientCertificateOptions = ClientCertificateOption.Manual;
                handler.ServerCertificateCustomValidationCallback =
                    (httpRequestMessage, cert, cetChain, policyErrors) =>
                    {
                        return true;
                    };
                HttpClient httpClient = new HttpClient(handler);

                var result = httpClient.GetAsync("http://gw2profits.com/json/v3").Result; //worst coding practice or worsest coding practice
                var content = result.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<List<Recipe>>(content, jsonSettings);
            }
            catch
            {
                MessageBox.Show("Error getting special recipes");
                return new List<Recipe>();
            }
          
        }

    }
}
