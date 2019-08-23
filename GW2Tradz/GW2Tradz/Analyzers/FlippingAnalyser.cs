using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GW2Tradz.Networking;
using GW2Tradz.Viewmodels;

namespace GW2Tradz.Analyzers
{
    class FlippingAnalyzer : IAnalyzer
    {
        public List<TradingAction> Analyse(Cache cache)
        {
            List<TradingAction> result = new List<TradingAction> { };
            foreach (var item in cache.Items)
            {
                var spike = item.SellPrice > item.YearSellAvg * 5;
                result.Add(new TradingAction
                {
                    Description = spike ? "(spike) Flip" : "Flip ",
                    Item = item,
                    MaxAmount = (int)(item.Velocity),
                    BaseCost = Settings.MediumTaskCost,
                    CostPer = item.FlipBuy,
                    IncomePer = item.FlipSell.AfterTP(),
                    SafeProfitPercentage = (spike && (item.Type == "Weapon" || item.Type == "Armor")) ? float.PositiveInfinity : Settings.SafeMinimumMargin,
                    Inventory = cache.CurrentSells[item.Id]
                });
            }
            return result;

        }
    }
}
