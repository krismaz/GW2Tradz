using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GW2Tradz.Viewmodels
{
    public class History
    {
        [JsonProperty(PropertyName = "itemID")]
        public int ItemId { get; set; }
        [JsonProperty(PropertyName = "sell_sold")]
        public int SellVelocity { get; set; }
        [JsonProperty(PropertyName = "buy_sold")]
        public int BuyVelocity { get; set; }
        [JsonProperty(PropertyName = "date")]
        public DateTime Date { get; set; }
        public int SellPriceMax { get; set; }
    }
}
