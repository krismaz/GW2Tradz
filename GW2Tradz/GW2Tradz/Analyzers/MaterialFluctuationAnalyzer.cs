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
            foreach (var item in cache.Materials)
            {
                result.Add(new TradingAction
                {
                    Description = $"Sell @ {item.MedianFlipSellMax.GoldFormat()} (Experiment, plz be care!)",
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
