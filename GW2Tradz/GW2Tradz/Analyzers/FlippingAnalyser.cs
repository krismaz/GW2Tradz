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
                result.Add(new TradingAction
                {
                    Description = "Flip",
                    Item = item,
                    MaxAmount = (int)(item.Velocity) - Settings.VelocityUncertainty,
                    BaseCost = Settings.MediumTaskCost,
                    CostPer = item.FlipBuy,
                    IncomePer = item.FlipSell,
                    SafeProfitPercentage = Settings.UnsafeMinumumMargin
                });
            }
            return result;

        }
    }
}
