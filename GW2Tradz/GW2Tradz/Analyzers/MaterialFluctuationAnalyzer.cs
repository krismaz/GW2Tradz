using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GW2Tradz.Networking;
using GW2Tradz.Viewmodels;

namespace GW2Tradz.Analyzers
{
    class MaterialFluctuationAnalyzer : IAnalyzer
    {
        public List<TradingAction> Analyse(Cache cache)
        {
            List<TradingAction> result = new List<TradingAction> { };
            cache.LoadHistory(cache.Materials.Where(i => i.History == null).Select(i => i.Id));

            foreach (var item in cache.Materials)
            {
                var spike = item.BuyPrice > item.MonthSellAvg;
                var action = new TradingAction
                {
                    Description = $"Sell @ {item.MedianFlipSellMax.GoldFormat()} (Experiment, plz be care!)" + (spike ? "(Spike!)" : ""),
                    Item = item,
                    MaxAmount = (int)(item.Velocity),
                    BaseCost = Settings.MediumTaskCost,
                    CostPer = item.FlipBuy,
                    IncomePer = item.MedianFlipSellMax.AfterTP(),
                    SafeProfitPercentage = float.PositiveInfinity,
                    Inventory = cache.CurrentSells[item.Id]
                };
                result.Add(action);

            }
            return result;
        }
    }
}
