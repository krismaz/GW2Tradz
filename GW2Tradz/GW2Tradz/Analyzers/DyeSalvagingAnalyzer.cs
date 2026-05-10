using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            cache.LoadListings(cache.Dyes.Where(d => d.ItemData != null).Select(i => i.ItemData.Id));

            var stats = cache.Dyes.GroupBy(d => d.Hue).Select(g => new
            {
                Hue = g.Key,
                Dyes = g.ToList(),
                Count = g.Count()
            }).ToList();

            foreach (var dye in cache.Dyes.Where(d => d.ItemData != null))
            {
                if(!Dye.Salvages.ContainsKey(dye.Hue) || !Dye.SalvageRates.ContainsKey(dye.ItemData.Rarity))
                {
                    continue;
                }

                var salvage = Dye.Salvages[dye.Hue].Select(i => cache.Lookup[i]).ToList();
                var salvageRate = Dye.SalvageRates[dye.ItemData.Rarity];
                var sale = (salvageRate * salvage.Select(i => i.SellPrice).Sum() / salvage.Count()).AfterTP();
                var cost = dye.ItemData.FlipBuy + 3;
                var inventory = (int)(salvage.Select(i => cache.CurrentSells[i.Id]).Max() / salvageRate);
                result.Add(new TradingAction($"dyes_{dye.ItemData.Id}_{dye.ItemData.Name}")
                {
                    MaxIn = (int)dye.ItemData.AdjustedBuyVelocity,
                    MaxOut = (int)(salvage.Select(i => i.AdjustedSellVelocity).Min() / salvageRate),
                    Description = $"Buy and Salvage - {dye.Hue}",
                    Item = dye.ItemData,
                    IncomePer = (int)sale,
                    CostPer = cost,
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
                if (!Dye.Salvages.ContainsKey(dye.Hue) || !Dye.SalvageRates.ContainsKey(dye.ItemData.Rarity))
                {
                    continue;
                }
                var salvage = Dye.Salvages[dye.Hue].Select(i => cache.Lookup[i]).ToList();
                var salvageRate = Dye.SalvageRates[dye.ItemData.Rarity];
                var sale = (salvageRate * salvage.Select(i => i.SellPrice).Sum() / salvage.Count()).AfterTP();
                var cost = dye.ItemData.FlipBuy + 3;
                var inventory = (int)(salvage.Select(i => cache.CurrentSells[i.Id]).Max() / salvageRate);

                var goodListings = cache.SellListings[dye.ItemData.Id].Where(l => l.Price + 3 < sale).ToList();


                if (!goodListings.Any())
                {
                    continue;
                }

                var totalCount = goodListings.Sum(l => l.Quantity);
                var totalPrice = goodListings.Sum(l => l.Quantity * l.Price);
                var maxPrice = goodListings.Max(l => l.Price); 
                

                result.Add(new TradingAction($"dyes_instant_{dye.ItemData.Id}_{dye.ItemData.Name}")
                {
                    MaxIn = totalCount,
                    MaxOut = (int)(salvage.Select(i => i.AdjustedSellVelocity).Min() / salvageRate),
                    Description = $"InstaBuy and Salvage ({maxPrice.GoldFormat()}) - {dye.Hue}",
                    Item = dye.ItemData,
                    CostPer = totalPrice / totalCount,
                    IncomePer = (int)sale,
                    SafeProfitPercentage = 0,
                    Inventory = inventory
                });


            }
            return result;
        }
    }
}
