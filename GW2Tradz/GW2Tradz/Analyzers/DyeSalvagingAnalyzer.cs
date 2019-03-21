using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GW2Tradz.Networking;
using GW2Tradz.Viewmodels;

namespace GW2Tradz.Analyzers
{
    class DyeSalvagingAnalyzer : IAnalyzer
    {
        public List<TradingAction> Analyse(Cache cache)
        {
            var result = new List<TradingAction>();
            foreach (var dye in cache.Dyes.Where(d => d.ItemData != null))
            {
                var salvage = Dye.Salvages[dye.Hue].Select(i => cache.Lookup[i]);
                var salvageRate = Dye.SalvageRates[dye.ItemData.Rarity];
                var sale = salvageRate * salvage.Select(i => i.SellPrice).Sum() / salvage.Count() * 0.85;
                var cost = dye.ItemData.FlipBuy + 3;
                result.Add(new TradingAction
                {
                    MaxAmount = (int)dye.ItemData.WeekBuyVelocity - Settings.VelocityUncertainty,
                    Description = "Buy and Salvage",
                    Item = dye.ItemData,
                    IncomePer = (int)sale,
                    CostPer = cost,
                    BaseCost = Settings.EasyTaskCost,
                    SafeProfitPercentage = Settings.SafeMinimumMargin
                });
            }
            return result;
        }
    }
}
