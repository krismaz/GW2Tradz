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
                var maxOut = (int)(salvage.Select(i => i.AdjustedSellVelocity).Min() / salvageRate);
                result.Add(new TradingAction($"dyes_{dye.ItemData.Id}_{dye.ItemData.Name}")
                {
                    MaxIn = (int)dye.ItemData.AdjustedBuyVelocity,
                    MaxOut = maxOut,
                    Description = $"Buy and Salvage - {dye.Hue}",
                    Item = dye.ItemData,
                    IncomePer = (int)sale,
                    CostPer = cost,
                    SafeProfitPercentage = Settings.SafeMinimumMargin,
                    Inventory = inventory
                });

                var goodListings = cache.AccumulateSellListings(dye.ItemData, (int)(sale - 3.0 / salvageRate), maxOut);

                if (goodListings.Valid)
                {
                    result.Add(new TradingAction($"dyes_instant_{dye.ItemData.Id}_{dye.ItemData.Name}")
                    {
                        MaxIn = goodListings.Amount,
                        MaxOut = (int)(salvage.Select(i => i.AdjustedSellVelocity).Min() / salvageRate),
                        Description = $"InstaBuy and Salvage ({goodListings.MaxPrice.GoldFormat()}) - {dye.Hue}",
                        Item = dye.ItemData,
                        CostPer = goodListings.Cost / goodListings.Amount,
                        IncomePer = (int)sale,
                        SafeProfitPercentage = 0,
                        Inventory = inventory
                    });
                }
            }
            return result;
        }
    }
}
