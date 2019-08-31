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

            var toxicStone = cache.Lookup[48915];
            var toxicCrystal = cache.Lookup[48917];
            var toxicOil = cache.Lookup[48916];
            var toxicSample = cache.Lookup[48884];


            var dustcost = (ecto.FlipBuy + 60) / 1.85;
            var waterCost = 8;
            var result = new List<TradingAction> { };
            cache.LoadListings(new int[] { 24277, 9476, 43449, 9461, 43450, 89157, 89203, 48915, 48916, 48917 });

            void HandleItem(Item item, int cost, int amount)
            {
                result.Add(new TradingAction($"ecto_{item.Id}_{item.Name}")
                {
                    MaxAmount = (int)item.AdjustedSellVelocity / amount,
                    Description = $"Ecto Salvage and Sell",
                    Item = item,
                    CostPer = cost,
                    IncomePer = amount * item.SellPrice.AfterTP(),
                    BaseCost = 0,
                    SafeProfitPercentage = Settings.UnsafeMinumumMargin,
                    Inventory = cache.CurrentSells[item.Id]/amount
                });

                var goodListings = cache.BuyListings[item.Id].Where(l => l.Price.AfterTP() > cost / amount);

                if (!goodListings.Any())
                {
                    return;
                }

                var totalCount = goodListings.Sum(l => l.Quantity);
                var totalIncome = goodListings.Sum(l => l.Quantity * l.Price).AfterTP();
                var minPrice = goodListings.Min(l => l.Price);

                result.Add(new TradingAction($"ecto_instant_{item.Id}_{item.Name}")
                {
                    MaxAmount = totalCount / amount,
                    Description = $"Ecto Salvage and InstaSell ({minPrice.GoldFormat()})",
                    Item = item,
                    CostPer = cost,
                    IncomePer = totalIncome / totalCount * amount,
                    BaseCost = 0,
                    SafeProfitPercentage = Settings.SafeMinimumMargin
                });
            }

            HandleItem(dust, (int)(dustcost), 1);
            HandleItem(master, (int)(dustcost * 5), 5);
            HandleItem(potent, (int)(dustcost * 6), 5);
            HandleItem(enhancedLucentOil, (int)((dustcost * 3 + lucent.FlipBuy * 5 + waterCost * 20 + enhancement.FlipBuy)), 5);
            HandleItem(potentLucentOil, (int)((dustcost * 3 + lucent.FlipBuy * 5 + waterCost * 20 + potence.FlipBuy)), 5);
            HandleItem(masterOil, (int)((dustcost * 3 + waterCost * 20)), 5);
            HandleItem(potentOil, (int)(((dustcost * 3 + waterCost * 20) / 5 + dustcost * 3 + waterCost * 20)), 5);

            HandleItem(toxicCrystal, (int)((dustcost * 3 + toxicSample.FlipBuy * 5)), 5);
            HandleItem(toxicOil, (int)((dustcost * 3 + toxicSample.FlipBuy * 5)), 5);
            HandleItem(toxicStone, (int)((dustcost * 3 + toxicSample.FlipBuy * 5)), 5);



            return result;
        }
    }
}
