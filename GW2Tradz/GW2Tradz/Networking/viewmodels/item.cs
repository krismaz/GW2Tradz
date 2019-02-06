using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GW2Tradz.Networking.viewmodels
{
    public class Item
    {
        public int Id { get; set; }
        public int BuyPrice { get; set; }
        public int SellPrice { get; set; }
        public string Name { get; set; }

        public void Update(Item other)
        {
            Id = other.Id;
            BuyPrice = other.BuyPrice;
            SellPrice = other.SellPrice;
            Name = other.Name ?? Name;
        }

        public int FlipBuy => BuyPrice + 1;
        public int FlipSell => SellPrice - 1;

        public int FlippingProfit => (int)((FlipSell)*0.85) - (FlipBuy);
        public float FlippingPercentage => (float)(FlippingProfit) / (float)(FlipBuy);
        
}
}
