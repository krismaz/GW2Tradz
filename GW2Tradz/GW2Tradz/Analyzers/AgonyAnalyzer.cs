using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using GW2Tradz.Networking;
using GW2Tradz.Viewmodels;
using Newtonsoft.Json;

namespace GW2Tradz.Analyzers
{
    class AgonyAnalyzer : IAnalyzer
    {
      

        public List<TradingAction> Analyse(Cache cache)
        {
            var infusions = new int[] { 49424, 49425, 49426, 49427, 49428, 49429, 49430, 49431, 49432, 49433, 49434 }.Select(i => cache.Lookup[i]).ToList();
            var termoCat = 150;
            var result = new List<TradingAction> { };

            for (int i = 1; i<infusions.Count; i++)
            {
                var target = infusions[i];
                for(int j = 0; j<i; j++)
                {
                    var source = infusions[j];

                    var count = (int)Math.Pow(2, i - j);
                    result.Add(new TradingAction($"agony_{i}_{j}")
                    {
                        MaxAmount = (int)(Math.Min(target.AdjustedSellVelocity, source.AdjustedBuyVelocity/count)),
                        Description = $"Buy Order {count} x {source.Name} -> {target.Name}",
                        Item = source,
                        CostPer = source.FlipBuy*count+ termoCat*(count-1),
                        IncomePer = target.FlipSell.AfterTP(),
                        BaseCost = Settings.HardTaskCost,
                        SafeProfitPercentage = Settings.SafeMinimumMargin
                    });

                }
            }

            return result;
        }

    }
}
