using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GW2Tradz.Viewmodels
{
    public class Item
    {
        public int Id { get; set; }
        public int BuyPrice { get; set; }
        public int SellPrice { get; set; }
        public string Name { get; set; }
        public string Rarity { get; set; }
        public int VendorValue { get; set; }

    public void Update(Item other)
        {
            Id = other.Id;
            BuyPrice = other.BuyPrice;
            SellPrice = other.SellPrice;
            Name = other.Name ?? Name;
            Rarity = other.Rarity ?? Rarity;
            VendorValue = other.VendorValue;
        }

        public int FlipBuy => (BuyPrice != 0 ? BuyPrice : VendorValue*6/5) + 1;
        public int FlipSell => SellPrice - 1;

        public int FlippingProfit => (int)((FlipSell)*0.85) - (FlipBuy);
        public float FlippingPercentage => (float)(FlippingProfit) / (float)(FlipBuy);

        public double SellV => History?.Select(h => h.SellVelocity).Average() ?? 0;
        public double BuyV => History?.Select(h => h.BuyVelocity).Average() ?? 0;
        public double Velocity => Math.Min(SellV, BuyV);
        public int GoldPerDay => (int)(Velocity * FlippingProfit);


        public List<History> History { get; set; }
        
}
}
