﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GW2Tradz.Networking;
using GW2Tradz.Viewmodels;

namespace GW2Tradz.Analyzers
{
    class MaterialSalvagingAnalyzer : IAnalyzer
    {

        public List<TradingAction> Analyse(Cache cache)
        {
            List<TradingAction> result = new List<TradingAction> { };

            void salvage(Dictionary<int, double> results, int salvageCost, int itemId)
            {
                var item = cache.Lookup[itemId];
                var salvager = "???";
                if(salvageCost == 3)
                {
                    salvager = "Copperfed";
                }
                if (salvageCost == 60)
                {
                    salvager = "Silverfed";
                }

                var salvageSell = results.Select(kv => kv.Value * cache.Lookup[kv.Key].FlipSell).Sum().AfterTP();
                var salvageUse = results.Select(kv => kv.Value * cache.Lookup[kv.Key].FlipBuy).Sum();

                result.Add(new TradingAction($"salvage_{salvageCost}_{itemId}_{item.Name}_sell")
                {
                    Description = $"Salvage ({salvager}) and Sell",
                    Item = item,
                    MaxAmount = (int)(item.AdjustedBuyVelocity),
                    BaseCost = Settings.MediumTaskCost,
                    CostPer = item.FlipBuy + salvageCost,
                    IncomePer = (int)salvageSell,
                    SafeProfitPercentage = Settings.SafeMinimumMargin,
                    Inventory = 0
                });

                result.Add(new TradingAction($"salvage_{salvageCost}_{itemId}_{item.Name}_sell")
                {
                    Description = $"Salvage ({salvager}) and Use",
                    Item = item,
                    MaxAmount = (int)(item.AdjustedBuyVelocity),
                    BaseCost = Settings.MediumTaskCost,
                    CostPer = item.FlipBuy + salvageCost,
                    IncomePer = (int)salvageUse,
                    SafeProfitPercentage = Settings.SafeMinimumMargin,
                    Inventory = 0
                });
            }

            //Bloodstone hide
            salvage(new Dictionary<int, double>
            {
                [19719] = 0.044,
                [19728] = 0.044,
                [19730] = 0.045,
                [19731] = 0.042,
                [19729] = 0.442,
                [19732] = 0.49,
            }, 3, 80681);

            salvage(new Dictionary<int, double>
            {
                [19719] = 0.05,
                [19728] = 0.049,
                [19730] = 0.048,
                [19731] = 0.051,
                [19729] = 0.506,
                [19732] = 0.553,
            }, 60, 80681);


            //Valuable metal scrap
            salvage(new Dictionary<int, double>
            {
                [19700] = 1.175,
                [19701] = 0.217
            }, 3, 21683);

            return result;
        }
    }
}
