using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GW2Tradz.Networking;
using GW2Tradz.Viewmodels;
using Newtonsoft.Json;

namespace GW2Tradz.Analyzers
{
    class UnidentifiedGearSalvager : IAnalyzer
    {
        public List<TradingAction> Analyse(Cache cache)
        {
            var result = new List<TradingAction> { };

            var ectos = cache.Lookup[19721];
            var rareGear = cache.Lookup[83008];

            var outPut = JsonConvert.DeserializeObject<List<DataEntry>>(File.ReadAllText("2000RareUnids.json"));

            var counts = outPut.GroupBy(e => cache.Lookup[e.ID], e => e.Quantity).ToDictionary(g => g.Key, g => g.Sum());
            var gear = counts.Where(kv => kv.Key.Type == "Armor" || kv.Key.Type == "Weapon").ToDictionary(kv => kv.Key, kv => kv.Value);
            var mats = counts.Where(kv => !(kv.Key.Type == "Armor" || kv.Key.Type == "Weapon")).ToDictionary(kv => kv.Key, kv => kv.Value);
            var ectoCount = mats[ectos];
            mats.Remove(ectos);

            var SalvageCost = 60 * (2000 - gear.Sum(kv => kv.Value)) / 2000f;

            var gearSales = gear.Sum(kv => kv.Value * kv.Key.SellPrice.AfterTP()) / 2000f;

            var useMats = mats.Sum(kv => kv.Value * kv.Key.BuyPrice) / 2000f;
            var sellMats = mats.Sum(kv => kv.Value * kv.Key.SellPrice.AfterTP()) / 2000f;

            var useEctos = ectoCount * ectos.BuyPrice / 2000f;
            var sellEctos = ectoCount * ectos.SellPrice.AfterTP() / 2000f;


            result.Add(new TradingAction
            {
                MaxAmount = (int)rareGear.AdjustedBuyVelocity,
                Description = $"Rare gear, use mats, use ecto",
                Item = rareGear,
                CostPer = rareGear.FlipBuy + (int)(SalvageCost),
                IncomePer = (int)(gearSales + useMats + useEctos),
                BaseCost = 0,
                SafeProfitPercentage = Settings.SafeMinimumMargin
            });

            result.Add(new TradingAction
            {
                MaxAmount = (int)rareGear.AdjustedBuyVelocity,
                Description = $"Rare gear, sell mats, use ecto",
                Item = rareGear,
                CostPer = rareGear.FlipBuy + (int)(SalvageCost),
                IncomePer = (int)(gearSales + sellMats + useEctos),
                BaseCost = 0,
                SafeProfitPercentage = Settings.SafeMinimumMargin
            });

            result.Add(new TradingAction
            {
                MaxAmount = (int)rareGear.AdjustedBuyVelocity,
                Description = $"Rare gear, use mats, sell ecto",
                Item = rareGear,
                CostPer = rareGear.FlipBuy + (int)(SalvageCost),
                IncomePer = (int)(gearSales + useMats + sellEctos),
                BaseCost = 0,
                SafeProfitPercentage = Settings.SafeMinimumMargin
            });

            result.Add(new TradingAction
            {
                MaxAmount = (int)rareGear.AdjustedBuyVelocity,
                Description = $"Rare gear, sell mats, sell ecto",
                Item = rareGear,
                CostPer = rareGear.FlipBuy + (int)(SalvageCost),
                IncomePer = (int)(gearSales + sellMats + sellEctos),
                BaseCost = 0,
                SafeProfitPercentage = Settings.SafeMinimumMargin
            });

            return result;
        }

        public class DataEntry
        {
            public int ID { get; set; }
            public int Quantity { get; set; }
        }
    }
}
