using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GW2Tradz.Networking;
using GW2Tradz.Viewmodels;

namespace GW2Tradz.Analyzers
{
    class HideSalvagingAnalyzer : IAnalyzer
    {
        public List<TradingAction> Analyse(Cache cache)
        {
            List<TradingAction> result = new List<TradingAction> { };
            var hide = cache.Lookup[80681];
            var t1 = cache.Lookup[19719];
            var t2 = cache.Lookup[19728];
            var t3 = cache.Lookup[19730];
            var t4 = cache.Lookup[19731];
            var t5 = cache.Lookup[19729];
            var t6 = cache.Lookup[19732];

            result.Add(new TradingAction($"hidesalvage_copper")
            {
                Description = "Salvage (copperfed)",
                Item = hide,
                MaxAmount = (int)(hide.AdjustedBuyVelocity),
                BaseCost = Settings.MediumTaskCost,
                CostPer = hide.FlipBuy + 3,
                IncomePer = (int)(0.044 * t1.FlipSell + 0.044 * t2.FlipSell + 0.045 * t3.FlipSell + 0.042 * t4.FlipSell + 0.442 * t5.FlipSell + 0.49 * t6.FlipSell).AfterTP(),
                SafeProfitPercentage = Settings.SafeMinimumMargin,
                Inventory = 0
            });

            result.Add(new TradingAction($"hidesalvag_silver")
            {
                Description = "Salvage (Silverfed)",
                Item = hide,
                MaxAmount = (int)(hide.AdjustedBuyVelocity),
                BaseCost = Settings.MediumTaskCost,
                CostPer = hide.FlipBuy + 60,
                IncomePer = (int)(0.05 * t1.FlipSell + 0.049 * t2.FlipSell + 0.048 * t3.FlipSell + 0.051 * t4.FlipSell + 0.506 * t5.FlipSell + 0.553 * t6.FlipSell).AfterTP(),
                SafeProfitPercentage = Settings.SafeMinimumMargin,
                Inventory = 0
            });

            return result;
        }
    }
}
