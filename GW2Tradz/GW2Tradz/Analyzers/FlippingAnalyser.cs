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
        public List<TradingAction> Analyse(int budget, Cache cache)
        {
            var maxDeep = budget / 10;
            List<TradingAction> result = new List<TradingAction> { };
            var items = cache.Resolve().Where(i => i.FlippingProfit > 0.2 && i.Velocity > 5 && i.GoldPerDay > 250000).OrderByDescending(i => i.FlippingPercentage);
            foreach(var item in items)
            {
                var itemBudget = Math.Min(budget, maxDeep);
                int proposal = (int)(item.Velocity) * item.FlipBuy;
                if(proposal < itemBudget)
                {
                    budget -= proposal;
                    result.Add(new TradingAction
                    {
                        Description = "Flip",
                        Item = item,
                        Amount = (int)(item.Velocity),
                        Profit = item.FlippingProfit * (int)(item.Velocity),
                        ProfitPercentage = item.FlippingPercentage
                    });
                }
                else if (item.FlipBuy < itemBudget)
                {
                    var amount = itemBudget / item.FlipBuy;
                    budget -= amount * item.FlipBuy;
                    result.Add(new TradingAction
                    {
                        Description = "Flip",
                        Item = item,
                        Amount = amount,
                        Profit = amount * item.FlippingProfit,
                        ProfitPercentage = item.FlippingPercentage
                    });
                }
               
            }
            return result;

        }
    }
}
