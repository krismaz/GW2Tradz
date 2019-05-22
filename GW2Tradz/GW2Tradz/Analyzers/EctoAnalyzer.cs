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

            var rareGear = cache.Lookup[83008];

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
            cache.LoadListings(new int[]{ 24277, 9476, 43449, 9461, 43450, 89157, 89203});

            void HandleItem(Item item, int cost)
            {
                result.Add(new TradingAction
                {
                    MaxAmount = (int)item.AdjustedSellVelocity  - cache.CurrentSells[item.Id],
                    Description = $"Ecto Salvage and Sell",
                    Item = item,
                    CostPer = cost,
                    IncomePer = item.SellPrice.AfterTP(),
                    BaseCost = 0,
                    SafeProfitPercentage = Settings.UnsafeMinumumMargin
                });

                var goodListings = cache.BuyListings[item.Id].Where(l => l.Price.AfterTP() > cost);

                if (!goodListings.Any())
                {
                    return;
                }

                var totalCount = goodListings.Sum(l => l.Quantity);
                var totalIncome = goodListings.Sum(l => l.Quantity * l.Price).AfterTP();
                var minPrice = goodListings.Min(l => l.Price);

                result.Add(new TradingAction
                {
                    MaxAmount = totalCount,
                    Description = $"Ecto Salvage and InstaSell ({minPrice.GoldFormat()})",
                    Item = item,
                    CostPer = cost,
                    IncomePer = totalIncome / totalCount,
                    BaseCost = 0,
                    SafeProfitPercentage = Settings.SafeMinimumMargin
                });
            }

            HandleItem(dust, (int)(dustcost));
            HandleItem(master, (int)(dustcost));
            HandleItem(potent, (int)(dustcost * 1.2));
            HandleItem(enhancedLucentOil, (int)((dustcost * 3 + lucent.BuyPrice * 5 + waterCost * 20 + enhancement.BuyPrice) / 5));
            HandleItem(potentLucentOil, (int)((dustcost * 3 + lucent.BuyPrice * 5 + waterCost * 20 + potence.BuyPrice) / 5));
            HandleItem(masterOil, (int)((dustcost * 3 + waterCost * 20) / 5));
            HandleItem(potentOil, (int)(((dustcost * 3 + waterCost * 20) / 5 + dustcost * 3 + waterCost * 20) / 5));

            result.Add(new TradingAction
            {
                MaxAmount = (int)rareGear.AdjustedBuyVelocity,
                Description = $"Rare gear and use",
                Item = rareGear,
                CostPer = rareGear.FlipBuy + 60,
                IncomePer = (int)(0.875 * ecto.BuyPrice),
                BaseCost = 0,
                SafeProfitPercentage = 0
            });

            result.Add(new TradingAction
            {
                MaxAmount = (int)rareGear.AdjustedBuyVelocity,
                Description = $"Rare gear and sell",
                Item = rareGear,
                CostPer = rareGear.FlipBuy + 60,
                IncomePer = (int)(0.875 * ecto.SellPrice.AfterTP()),
                BaseCost = 0,
                SafeProfitPercentage = 0
            });


            return result;
        }
    }
}
