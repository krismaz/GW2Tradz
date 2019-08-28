using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GW2Tradz.Networking;
using GW2Tradz.Viewmodels;

namespace GW2Tradz.Analyzers
{
    class DyeSalvagingAnalyzer : IAnalyzer
    {
        public List<TradingAction> Analyse(Cache cache)
        {
            var result = new List<TradingAction>();


            var instantDyes = new List<Dye> { };
            cache.LoadListings(cache.Dyes.Where(d => d.ItemData != null).Select(i => i.ItemData.Id).ToList());

            foreach (var dye in cache.Dyes.Where(d => d.ItemData != null))
            {
                var salvage = Dye.Salvages[dye.Hue].Select(i => cache.Lookup[i]);
                var salvageRate = Dye.SalvageRates[dye.ItemData.Rarity];
                var sale = (salvageRate * salvage.Select(i => i.SellPrice).Sum() / salvage.Count()).AfterTP();
                var cost = dye.ItemData.FlipBuy + 3;
                var inventory = (int)(salvage.Select(i => cache.CurrentSells[i.Id]).Average() / salvageRate);
                result.Add(new TradingAction
                {
                    MaxAmount = (int)dye.ItemData.AdjustedBuyVelocity,
                    Description = "Buy and Salvage",
                    Item = dye.ItemData,
                    IncomePer = (int)sale,
                    CostPer = cost,
                    BaseCost = Settings.EasyTaskCost,
                    SafeProfitPercentage = Settings.SafeMinimumMargin,
                    Inventory = inventory
                });
                if (sale > dye.ItemData.SellPrice + 3)
                {
                    instantDyes.Add(dye);
                }
            }
            foreach (var dye in instantDyes)
            {
                var salvage = Dye.Salvages[dye.Hue].Select(i => cache.Lookup[i]);
                var salvageRate = Dye.SalvageRates[dye.ItemData.Rarity];
                var sale = (salvageRate * salvage.Select(i => i.SellPrice).Sum() / salvage.Count()).AfterTP();
                var cost = dye.ItemData.FlipBuy + 3;
                var inventory = (int)(salvage.Select(i => cache.CurrentSells[i.Id]).Average() / salvageRate);

                var goodListings = cache.SellListings[dye.ItemData.Id].Where(l => l.Price + 3 < sale);


                if (!goodListings.Any())
                {
                    continue;
                }

                var totalCount = goodListings.Sum(l => l.Quantity);
                var totalPrice = goodListings.Sum(l => l.Quantity * l.Price);
                var maxPrice = goodListings.Max(l => l.Price);

                result.Add(new TradingAction
                {
                    MaxAmount = totalCount,
                    Description = $"InstaBuy and Salvage ({maxPrice.GoldFormat()})",
                    Item = dye.ItemData,
                    CostPer = totalPrice / totalCount,
                    IncomePer = (int)sale,
                    BaseCost = 0,
                    SafeProfitPercentage = 0,
                    Inventory = inventory
                });
            }
            return result;
        }
    }
}
