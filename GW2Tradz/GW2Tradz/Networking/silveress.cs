﻿using GW2Tradz.Viewmodels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace GW2Tradz.Networking
{
    class Silveress
    {
        private HttpClient httpClient = new HttpClient(new HttpClientHandler()
        {
            AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
        })
        {
            Timeout = TimeSpan.FromMilliseconds(-1)
        };
        private JsonSerializerSettings jsonSettings = new JsonSerializerSettings { ContractResolver = new DefaultContractResolver { NamingStrategy = new SnakeCaseNamingStrategy() } };

        public Silveress()
        {
            httpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("GW2Tradz", "1.0"));
            httpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("(Krismaz.1250, aka. dōTYRIA Quaggan Oil Saleshun)"));
        }

        public List<Item> FetchBasicInfo()
        {
            var result = httpClient.GetAsync("https://api.silveress.ie/gw2/v1/items/json?beautify=min&fields=id,buy_price,sell_price,name,rarity,vendor_value,yesterday_buy_sold,yesterday_sell_sold").Result; //worst coding practice or worsest coding practice
            var content = result.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<List<Item>>(content, jsonSettings);
        }

        public List<History> FetchHistory()
        {
            return new List<History> { };
            var OneWeek = (DateTime.Now - TimeSpan.FromDays(7)).ToString("yyyy-MM-dd");
            var result = httpClient.GetAsync($"https://api.silveress.ie/gw2/v1/history?beautify=min&start={OneWeek}&fields=itemID,date,buy_velocity,sell_velocity,day_sell_max").Result; //worst coding practice or worsest coding practice
            var content = result.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<List<History>>(content, jsonSettings);
        }
    }
}
