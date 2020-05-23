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
    class BagAnalyzer : IAnalyzer
    {
        float count = 2500f;

        public List<TradingAction> Analyse(Cache cache)
        {
            var result = new List<TradingAction> { };

            var bag = cache.Lookup[8920];
            var outPut = JsonConvert.DeserializeObject<List<DataEntry>>(File.ReadAllText("2500HeavyLootBags.json"));

            var counts = outPut.GroupBy(e => cache.Lookup[e.ID], e => e.Quantity).ToDictionary(g => g.Key, g => g.Sum());
            var useMats = counts.Sum(kv => kv.Value * kv.Key.FlipBuy)/count;
            var sellMats = counts.Sum(kv => kv.Value * kv.Key.FlipSell.AfterTP())/count;

            result.Add(new TradingAction($"open_use_8920")
            {
                MaxAmount = (int)bag.AdjustedBuyVelocity,
                Description = $"Open bag, use mats",
                Item = bag,
                CostPer = bag.FlipBuy,
                IncomePer = (int)(useMats) + 3, // 3-5 copper content
                BaseCost = Settings.HardTaskCost,
                SafeProfitPercentage = float.PositiveInfinity
            });

            result.Add(new TradingAction($"open_sell_8920")
            {
                MaxAmount = (int)bag.AdjustedBuyVelocity,
                Description = $"Open bag, sell mats",
                Item = bag,
                CostPer = bag.FlipBuy,
                IncomePer = (int)(sellMats) + 3, // 3-5 copper content
                BaseCost = Settings.HardTaskCost,
                SafeProfitPercentage = float.PositiveInfinity
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
