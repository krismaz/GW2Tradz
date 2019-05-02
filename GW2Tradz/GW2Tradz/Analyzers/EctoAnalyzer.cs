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

            var lucent = cache.Lookup[89271];
            var enhancement = cache.Lookup[89141];
            var potence = cache.Lookup[89258];

            var dust = cache.Lookup[24277];
            var master = cache.Lookup[9476];
            var potent = cache.Lookup[43449];
            var masterOil = cache.Lookup[9461];
            var potentOil = cache.Lookup[43450];
            var enhancedLucentOil = cache.Lookup[89157];
            var potentLucentOil = cache.Lookup[89203];


            var dustcost = (ecto.BuyPrice + 60) / 1.85;
            var waterCost = 8;
            var result = new List<TradingAction> { };

            result.Add(new TradingAction
            {
                MaxAmount = (int)(dust.WeekSellVelocity ?? 0) - Settings.VelocityUncertainty - cache.CurrentSells[dust.Id],
                Description = $"Ecto Salvage and Sell",
                Item = dust,
                CostPer = (int)dustcost,
                IncomePer = dust.SellPrice.AfterTP(),
                BaseCost = 0,
                SafeProfitPercentage = Settings.SafeMinimumMargin
            });

            result.Add(new TradingAction
            {
                MaxAmount = (int)(dust.WeekSellVelocity ?? 0),
                Description = $"Ecto Salvage and InstaSell",
                Item = dust,
                CostPer = (int)dustcost,
                IncomePer = dust.BuyPrice.AfterTP(),
                BaseCost = 0,
                SafeProfitPercentage = 0
            });

            result.Add(new TradingAction
            {
                MaxAmount = (int)(dust.WeekSellVelocity ?? 0) - Settings.VelocityUncertainty - cache.CurrentSells[master.Id],
                Description = $"Ecto Salvage and Sell",
                Item = master,
                CostPer = (int)dustcost,
                IncomePer = master.SellPrice.AfterTP(),
                BaseCost = 0,
                SafeProfitPercentage = Settings.SafeMinimumMargin
            });

            result.Add(new TradingAction
            {
                MaxAmount = (int)(dust.WeekSellVelocity ?? 0),
                Description = $"Ecto Salvage and InstaSell",
                Item = master,
                CostPer = (int)dustcost,
                IncomePer = master.BuyPrice.AfterTP(),
                BaseCost = 0,
                SafeProfitPercentage = 0
            });

            result.Add(new TradingAction
            {
                MaxAmount = (int)(dust.WeekSellVelocity ?? 0) - Settings.VelocityUncertainty - cache.CurrentSells[potent.Id],
                Description = $"Ecto Salvage and Sell",
                Item = potent,
                CostPer = (int)(dustcost * 1.2),
                IncomePer = potent.SellPrice.AfterTP(),
                BaseCost = 0,
                SafeProfitPercentage = Settings.SafeMinimumMargin
            });

            result.Add(new TradingAction
            {
                MaxAmount = (int)(dust.WeekSellVelocity ?? 0),
                Description = $"Ecto Salvage and InstaSell",
                Item = potent,
                CostPer = (int)(dustcost * 1.2),
                IncomePer = potent.BuyPrice.AfterTP(),
                BaseCost = 0,
                SafeProfitPercentage = 0
            });

            result.Add(new TradingAction
            {
                MaxAmount = (int)(enhancedLucentOil.WeekSellVelocity ?? 0) - Settings.VelocityUncertainty - cache.CurrentSells[enhancedLucentOil.Id],
                Description = $"Ecto Salvage and Sell",
                Item = enhancedLucentOil,
                CostPer = (int)((dustcost * 3 + lucent.BuyPrice * 5 + waterCost * 20 + enhancement.BuyPrice) / 5),
                IncomePer = enhancedLucentOil.SellPrice.AfterTP(),
                BaseCost = 0,
                SafeProfitPercentage = Settings.SafeMinimumMargin
            });

            result.Add(new TradingAction
            {
                MaxAmount = (int)(enhancedLucentOil.WeekSellVelocity ?? 0),
                Description = $"Ecto Salvage and InstaSell",
                Item = enhancedLucentOil,
                CostPer = (int)((dustcost * 3 + lucent.BuyPrice * 5 + waterCost*20 + enhancement.BuyPrice)/5),
                IncomePer = enhancedLucentOil.BuyPrice.AfterTP(),
                BaseCost = 0,
                SafeProfitPercentage = 0
            });

            result.Add(new TradingAction
            {
                MaxAmount = (int)(potentLucentOil.WeekSellVelocity ?? 0) - Settings.VelocityUncertainty - cache.CurrentSells[potentLucentOil.Id],
                Description = $"Ecto Salvage and Sell",
                Item = potentLucentOil,
                CostPer = (int)((dustcost * 3 + lucent.BuyPrice * 5 + waterCost * 20 + potence.BuyPrice) / 5),
                IncomePer = potentLucentOil.SellPrice.AfterTP(),
                BaseCost = 0,
                SafeProfitPercentage = Settings.SafeMinimumMargin
            });

            result.Add(new TradingAction
            {
                MaxAmount = (int)(potentLucentOil.WeekSellVelocity ?? 0),
                Description = $"Ecto Salvage and InstaSell",
                Item = potentLucentOil,
                CostPer = (int)((dustcost * 3 + lucent.BuyPrice * 5 + waterCost * 20 + potence.BuyPrice) / 5),
                IncomePer = potentLucentOil.BuyPrice.AfterTP(),
                BaseCost = 0,
                SafeProfitPercentage = 0
            });

            result.Add(new TradingAction
            {
                MaxAmount = (int)(masterOil.WeekSellVelocity ?? 0) - Settings.VelocityUncertainty - cache.CurrentSells[masterOil.Id],
                Description = $"Ecto Salvage and Sell",
                Item = masterOil,
                CostPer = (int)((dustcost * 3 + waterCost * 20) / 5),
                IncomePer = masterOil.SellPrice.AfterTP(),
                BaseCost = 0,
                SafeProfitPercentage = Settings.SafeMinimumMargin
            });

            result.Add(new TradingAction
            {
                MaxAmount = (int)(masterOil.WeekSellVelocity ?? 0),
                Description = $"Ecto Salvage and InstaSell",
                Item = masterOil,
                CostPer = (int)((dustcost * 3 + waterCost * 20)/5),
                IncomePer = masterOil.BuyPrice.AfterTP(),
                BaseCost = 0,
                SafeProfitPercentage = 0
            });

            result.Add(new TradingAction
            {
                MaxAmount = (int)(potentOil.WeekSellVelocity ?? 0) - Settings.VelocityUncertainty - cache.CurrentSells[potentOil.Id],
                Description = $"Ecto Salvage and Sell",
                Item = potentOil,
                CostPer = (int)(((dustcost * 3 + waterCost * 20) / 5 + dustcost * 3 + waterCost * 20)/5),
                IncomePer = potentOil.SellPrice.AfterTP(),
                BaseCost = 0,
                SafeProfitPercentage = Settings.SafeMinimumMargin
            });

            result.Add(new TradingAction
            {
                MaxAmount = (int)(potentOil.WeekSellVelocity ?? 0),
                Description = $"Ecto Salvage and InstaSell",
                Item = potentOil,
                CostPer = (int)(((dustcost * 3 + waterCost * 20) / 5 + dustcost * 3 + waterCost * 20) / 5),
                IncomePer = potentOil.BuyPrice.AfterTP(),
                BaseCost = 0,
                SafeProfitPercentage = 0
            });

            return result;
        }
    }
}
