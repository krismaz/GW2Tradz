using GW2Tradz.Networking;
using GW2Tradz.Viewmodels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GW2Tradz.Analyzers
{
    class ElonianAnalyzer : IAnalyzer
    {
        public List<TradingAction> Analyse(Cache cache)
        {
            List<TradingAction> result = new List<TradingAction> { };
            foreach (var item in cache.Items.Where(i => i.Rarity == "Rare" && i.Type == "Weapon"))
            {
                result.Add(new TradingAction
                {
                    Description = $"Sell @ {item.MedianFlipSellMax.GoldFormat()}",
                    Item = item,
                    MaxAmount = (int)(item.Velocity) - cache.CurrentSells[item.Id],
                    BaseCost = Settings.MediumTaskCost,
                    CostPer = item.FlipBuy,
                    IncomePer = item.MedianFlipSellMax.AfterTP(),
                    SafeProfitPercentage = Settings.UnsafeMinumumMargin
                });
            }
            return result;

        }
    }
}
