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
                MaxIn = (int)bag.AdjustedBuyVelocity,
                MaxOut = Settings.MaxSaneAmount,
                Description = $"Open bag, use mats",
                Item = bag,
                CostPer = bag.FlipBuy,
                IncomePer = (int)(useMats) + 3, // 3-5 copper content
                SafeProfitPercentage = float.PositiveInfinity
            });

            result.Add(new TradingAction($"open_sell_8920")
            {
                MaxIn = (int)bag.AdjustedBuyVelocity,
                MaxOut = Settings.MaxSaneAmount,
                Description = $"Open bag, sell mats",
                Item = bag,
                CostPer = bag.FlipBuy,
                IncomePer = (int)(sellMats) + 3, // 3-5 copper content
                SafeProfitPercentage = float.PositiveInfinity
            });


            var trickbag = cache.Lookup[36038];
            var corn = cache.Lookup[36041];
            var cornAmount = 4.28f;
            var income = (int)(corn.FlipSell.AfterTP() * cornAmount + 75 * 1.1 + Settings.EmpyrialShardValue*0.25); //75 is total vendor value of trash crafting mats;

            result.Add(new TradingAction($"open_sell_36038")
            {
                MaxIn = (int)trickbag.AdjustedBuyVelocity,
                MaxOut = (int)(corn.AdjustedSellVelocity / cornAmount),
                Inventory = (int)(cache.CurrentSells[36041]/cornAmount),
                Description = $"Open trick or treat bag, sell corn, vendor trash",
                Item = trickbag,
                CostPer = trickbag.FlipBuy,
                IncomePer = income,
                SafeProfitPercentage = float.PositiveInfinity
            });

            result.Add(new TradingAction($"open_sell_36038_2")
            {
                MaxIn = (int)trickbag.AdjustedBuyVelocity,
                MaxOut = (int)(corn.AdjustedSellVelocity / cornAmount),
                Inventory = (int)(cache.CurrentSells[36041] / cornAmount),
                Description = $"(Highly Unsafe) Be lucky! Open trick or treat bag, sell corn, vendor trash",
                Item = trickbag,
                CostPer = trickbag.FlipBuy,
                IncomePer = income + 100,
                SafeProfitPercentage = float.PositiveInfinity
            });

            cache.LoadListings(new int[] { 36038 });
            var trick = cache.Lookup[36038];

            var goodListings = cache.AccumulateSellListings(trick, income, (int)(corn.AdjustedSellVelocity / cornAmount));
            if (goodListings.Valid)
            {
                result.Add(new TradingAction($"open_sell_36038_3")
                {
                    MaxIn = goodListings.Amount,
                    MaxOut = (int)(corn.AdjustedSellVelocity / cornAmount),
                    Inventory = (int)(cache.CurrentSells[36041] / cornAmount),
                    Description = $"Instabuy trick or treat bag @{goodListings.MaxPrice.GoldFormat()}, sell corn, vendor trash",
                    Item = trickbag,
                    CostPer = goodListings.Cost / goodListings.Amount,
                    IncomePer = income,
                    SafeProfitPercentage = 0
                });
            }

            var luckyListings = cache.AccumulateSellListings(trick, income+100, (int)(corn.AdjustedSellVelocity / cornAmount));

            if (luckyListings.Valid)
            {

                result.Add(new TradingAction($"open_sell_36038_4")
                {
                    MaxIn = luckyListings.Amount,
                    MaxOut = (int)(corn.AdjustedSellVelocity / cornAmount),
                    Inventory = (int)(cache.CurrentSells[36041] / cornAmount),
                    Description = $"(Highly Unsafe) Be lucky! Instabuy trick or treat bag @{luckyListings.MaxPrice.GoldFormat()}, sell corn, vendor trash",
                    Item = trickbag,
                    CostPer = luckyListings.Cost / luckyListings.Amount,
                    IncomePer = income +100,
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
