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
        public int SellVelocity { get; set; }
        public int BuyVelocity { get; set; }
        public DateTime Date { get; set; }
        public int SellPriceMax { get; set; }
    }
}
