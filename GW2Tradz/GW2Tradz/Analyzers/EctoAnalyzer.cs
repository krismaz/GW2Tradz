using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GW2Tradz.Networking;
using GW2Tradz.Viewmodels;

namespace GW2Tradz.Analyzers
{
    class EctoAnalyzer : IAnalyzer
    {
        public List<TradingAction> Analyse(Cache cache)
        {
            var ecto = cache.Lookup[19721];
            var dust = cache.Lookup[24277];
            var master = cache.Lookup[9476];
            var potent = cache.Lookup[43449];

            var dustcost = (ecto.BuyPrice + 60) / 1.85;
            var result = new List<TradingAction> { };

            result.Add(new TradingAction
            {
                MaxAmount = (int)(dust.WeekSellVelocity ?? 0) - cache.CurrentSells[dust.Id],
                Description = $"Sell",
                Item = dust,
                CostPer = (int)dustcost,
                IncomePer = dust.SellPrice.AfterTP(),
                BaseCost = 0,
                SafeProfitPercentage = Settings.SafeMinimumMargin
            });

            result.Add(new TradingAction
            {
                MaxAmount = (int)(dust.WeekSellVelocity ?? 0),
                Description = $"InstaSell",
                Item = dust,
                CostPer = (int)dustcost,
                IncomePer = dust.BuyPrice.AfterTP(),
                BaseCost = 0,
                SafeProfitPercentage = 0
            });

            result.Add(new TradingAction
            {
                MaxAmount = (int)(dust.WeekSellVelocity ?? 0) - cache.CurrentSells[master.Id],
                Description = $"Sell",
                Item = master,
                CostPer = (int)dustcost,
                IncomePer = master.SellPrice.AfterTP(),
                BaseCost = 0,
                SafeProfitPercentage = Settings.SafeMinimumMargin
            });

            result.Add(new TradingAction
            {
                MaxAmount = (int)(dust.WeekSellVelocity ?? 0),
                Description = $"InstaSell",
                Item = master,
                CostPer = (int)dustcost,
                IncomePer = master.BuyPrice.AfterTP(),
                BaseCost = 0,
                SafeProfitPercentage = 0
            });

            result.Add(new TradingAction
            {
                MaxAmount = (int)(dust.WeekSellVelocity ?? 0) - cache.CurrentSells[potent.Id],
                Description = $"Sell",
                Item = potent,
                CostPer = (int)(dustcost * 1.2),
                IncomePer = potent.SellPrice.AfterTP(),
                BaseCost = 0,
                SafeProfitPercentage = Settings.SafeMinimumMargin
            });

            result.Add(new TradingAction
            {
                MaxAmount = (int)(dust.WeekSellVelocity ?? 0),
                Description = $"InstaSell",
                Item = potent,
                CostPer = (int)(dustcost * 1.2),
                IncomePer = potent.BuyPrice.AfterTP(),
                BaseCost = 0,
                SafeProfitPercentage = 0
            });

            return result;
        }
    }
}
