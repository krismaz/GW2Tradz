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
        float count = 327250f;

        public List<TradingAction> Analyse(Cache cache)
        {
            var result = new List<TradingAction> { };

            var bag = cache.Lookup[8920];
            var outPut = JsonConvert.DeserializeObject<List<DataEntry>>(File.ReadAllText("327250HeavyLootBags.json"));

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


            var trickbag = cache.Lookup[36038];
            var corn = cache.Lookup[36041];
            var cornAmount = 4.28f;
            var income = (int)(corn.FlipSell.AfterTP() * cornAmount + 75 * 1.1); //75 is total vendor value of trash crafting mats;

            result.Add(new TradingAction($"open_sell_36038")
            {
                MaxAmount = Math.Min((int)trickbag.AdjustedBuyVelocity, (int)(corn.AdjustedSellVelocity/cornAmount)),
                Description = $"Open trick or treat bag, sell corn, vendor trash",
                Item = trickbag,
                CostPer = trickbag.FlipBuy,
                IncomePer = income,
                BaseCost = Settings.HardTaskCost,
                SafeProfitPercentage = float.PositiveInfinity
            });

            cache.LoadListings(new int[] { 36038 });

            var goodListings = cache.SellListings[36038].Where(l => l.Price  < income).ToList();
            if(goodListings.Any())
            {
                var totalCount = goodListings.Sum(l => l.Quantity);
                var totalPrice = goodListings.Sum(l => l.Quantity * l.Price);
                var maxPrice = goodListings.Max(l => l.Price);

                result.Add(new TradingAction($"open_sell_36038_2")
                {
                    MaxAmount = totalCount,
                    Description = $"Instabuy trick or treat bag @{maxPrice.GoldFormat()}, sell corn, vendor trash",
                    Item = trickbag,
                    CostPer = trickbag.FlipBuy,
                    IncomePer = totalPrice / totalCount,
                    BaseCost = Settings.HardTaskCost,
                    SafeProfitPercentage = 0
                });
            }

            var luckyListings = cache.SellListings[36038].Where(l => l.Price < income +100).ToList();
            if (luckyListings.Any())
            {
                var totalCount = luckyListings.Sum(l => l.Quantity);
                var totalPrice = luckyListings.Sum(l => l.Quantity * l.Price);
                var maxPrice = luckyListings.Max(l => l.Price);

                result.Add(new TradingAction($"open_sell_36038_3")
                {
                    MaxAmount = Math.Min(totalCount, (int)(corn.AdjustedSellVelocity / cornAmount)),
                    Description = $"(Highly Unsafe) Be lucky! Instabuy trick or treat bag @{maxPrice.GoldFormat()}, sell corn, vendor trash",
                    Item = trickbag,
                    CostPer = trickbag.FlipBuy,
                    IncomePer = totalPrice / totalCount,
                    BaseCost = Settings.HardTaskCost,
                    SafeProfitPercentage = double.PositiveInfinity
                });
            }

            return result;
        }

        public class DataEntry
        {
            public int ID { get; set; }
            public int Quantity { get; set; }
        }
    }
}
