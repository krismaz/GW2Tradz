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
            var weapons = cache.Items.Where(i => i.Rarity == "Rare" && i.Type == "Weapon" && i.Name.StartsWith("Elonian"));
            cache.LoadHistory(weapons.Where(i => i.History == null).Select(i => i.Id));
            
            foreach (var item in weapons)
            {
                result.Add(new TradingAction($"elonian_{item.Id}_{item.Name}")
                {
                    Description = $"Sell @ {item.MedianFlipSellMax.GoldFormat()}",
                    Item = item,
                    MaxAmount = (int)(item.Velocity),
                    BaseCost = Settings.MediumTaskCost,
                    CostPer = item.FlipBuy,
                    IncomePer = item.MedianFlipSellMax.AfterTP(),
                    SafeProfitPercentage = Settings.UnsafeMinimumMargin,
                    Inventory = cache.CurrentSells[item.Id]
                });
            }
            return result;

        }
    }
}
