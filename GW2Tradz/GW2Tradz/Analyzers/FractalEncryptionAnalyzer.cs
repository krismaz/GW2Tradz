using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GW2Tradz.Networking;
using GW2Tradz.Viewmodels;

namespace GW2Tradz.Analyzers
{
    class FractalEncryptionAnalyzer : IAnalyzer
    {
        public List<TradingAction> Analyse(Cache cache)
        {
            var encryption = cache.Lookup[75919];
            var matrix = cache.Lookup[73248];

            var infusion = cache.Lookup[49424];
            var t5s = new List<int> { 24276, 24299, 24282, 24341, 24294, 24356, 24350, 24288 }.Select(id => cache.Lookup[id]).ToList();
            var mew = cache.Lookup[74268];

            var cost = encryption.FlipBuy + 0.9 * matrix.FlipBuy;

            cache.LoadHistory(t5s.Where(i => i.History == null).Select(i => i.Id));


            var infusionIncome = 2.26 * infusion.FlipSell.AfterTP();
            var t5sIncomeNoTPTax = 0.3375 * t5s.Sum(i => i.FlipBuy);
            var t5sIncomeWithTPTax = 0.3375 * t5s.Sum(i => i.FlipSell.AfterTP());
            var t5sMedians = 0.3375 * t5s.Sum(i => Math.Max(i.MedianFlipSellMax, i.FlipSell).AfterTP());
            var mewIncome = 0.02 * mew.FlipSell.AfterTP();
            var relicIncome = 0.2 * Settings.FractalValue;


            var result = new List<TradingAction> { };

            result.Add(new TradingAction($"fractal_use")
            {
                MaxIn = (int)(Math.Min(encryption.AdjustedBuyVelocity, matrix.AdjustedBuyVelocity)),
                MaxOut = Settings.MaxSaneAmount,
                Description = $"Fractal encryption and use",
                Item = encryption,
                CostPer = (int)cost,
                IncomePer = 4283 + (int)(infusionIncome + t5sIncomeNoTPTax + mewIncome + relicIncome) + Settings.EmpyrialShardValue,
                SafeProfitPercentage = 10
            });

            result.Add(new TradingAction($"fractal_sell")
            {
                MaxIn = (int)(Math.Min(encryption.AdjustedBuyVelocity, matrix.AdjustedBuyVelocity)),
                MaxOut = Settings.MaxSaneAmount,
                Description = $"Fractal encryption and sell",
                Item = encryption,
                CostPer = (int)cost,
                IncomePer = 4283 + (int)(infusionIncome + t5sIncomeWithTPTax + mewIncome + relicIncome) + Settings.EmpyrialShardValue,
                SafeProfitPercentage = 10
            });

            result.Add(new TradingAction($"fractal_median")
            {
                MaxIn = (int)(Math.Min(encryption.AdjustedBuyVelocity, matrix.AdjustedBuyVelocity)),
                MaxOut = Settings.MaxSaneAmount,
                Description = $"Fractal encryption and sell @median",
                Item = encryption,
                CostPer = (int)cost,
                IncomePer = 4283 + (int)(infusionIncome + t5sMedians + mewIncome + relicIncome) + Settings.EmpyrialShardValue,
                SafeProfitPercentage = 10
            });

            return result;
        }
    }
}
