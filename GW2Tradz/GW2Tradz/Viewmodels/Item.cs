using Newtonsoft.Json;
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
        public string Type { get; set; }
        public string Rarity { get; set; }
        public int Upgrade1 { get; set; }
        public int Level { get; set; }
        [JsonProperty("statName")]
        public string StatName { get; set; }
        public int VendorValue { get; set; }
        [JsonProperty("7d_sell_sold")]
        public float? WeekSellVelocity { get; set; }
        [JsonProperty("7d_buy_sold")]
        public float? WeekBuyVelocity { get; set; }
        [JsonProperty("12m_sell_price_avg")]
        public float? YearSellAvg { get; set; }
        [JsonProperty("1m_sell_price_avg")]
        public float? MonthSellAvg { get; set; }
        public float AdjustedBuyVelocity => Math.Min(Settings.MaxSaneAmount, (WeekBuyVelocity ?? 0) / Settings.VelocityFactor - Settings.VelocityUncertainty);
        public float AdjustedSellVelocity => Math.Min(Settings.MaxSaneAmount, (WeekSellVelocity ?? 0) / Settings.VelocityFactor - Settings.VelocityUncertainty);

        public void Update(Item other)
        {
            Id = other.Id;
            BuyPrice = other.BuyPrice;
            SellPrice = other.SellPrice;
            Name = other.Name ?? Name;
            Rarity = other.Rarity ?? Rarity;
            VendorValue = Math.Max(other.VendorValue, VendorValue);
            StatName = other.StatName ?? StatName;
            YearSellAvg = other.YearSellAvg ?? YearSellAvg;
            MonthSellAvg = other.MonthSellAvg ?? MonthSellAvg;
            WeekSellVelocity = other.WeekSellVelocity ?? WeekSellVelocity;
            WeekBuyVelocity = other.WeekBuyVelocity ?? WeekBuyVelocity;
        }

        public int FlipBuy => (BuyPrice != 0 ? BuyPrice : VendorValue * 6 / 5) + 1;
        public int FlipSell => SellPrice - 1;

        public int FlippingProfit => FlipSell.AfterTP() - FlipBuy;
        public float FlippingPercentage => (float)(FlippingProfit) / (float)(FlipBuy);

        public double Velocity => Math.Min(AdjustedBuyVelocity, AdjustedSellVelocity);
        public int GoldPerDay => (int)(Velocity * FlippingProfit);

        public List<History> History { get; set; }
        public int MedianSellMax
        {
            get
            {
                if (History == null)
                {
                    return 0;
                }
                var values = History.Select(h => h.SellPriceMax).ToList();
                values.Sort();
                return values[values.Count / 2];
            }
        }
        public int MedianFlipSellMax => MedianSellMax - 1;

    }
}
