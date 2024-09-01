using AngleSharp.Dom;
using GW2Tradz.Networking;
using GW2Tradz.Viewmodels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;

namespace GW2Tradz.Analyzers
{
    class GearSalvagingAnalyzer : IAnalyzer
    {
        public GearSalvagingAnalyzer()
        {
        }

        public List<TradingAction> Analyse(Cache cache)
        {

            var ecto = cache.Lookup[19721];
            List<TradingAction> result = new List<TradingAction> { };
            foreach (var item in cache.Items.Where(i => i.Rarity == "Rare" && (i.Type == "Weapon" || i.Type == "Armor" || i.Type == "Trinket") && i.Level >= 68))
            {
                var upgrade = cache.Lookup.ContainsKey(item.Upgrade1) ? cache.Lookup[item.Upgrade1].FlipSell : 0;

                result.Add(new TradingAction($"gearsalvage_{item.Id}_{item.Name}")
                {
                    Description = "Extract + Salvage (Silverfed)",
                    Item = item,
                    MaxAmount = (int)(item.AdjustedBuyVelocity),
                    BaseCost = Settings.MediumTaskCost,
                    CostPer = item.FlipBuy + 60,
                    IncomePer = (int)(ecto.FlipSell * 0.875 + upgrade).AfterTP(),
                    SafeProfitPercentage = float.PositiveInfinity,
                    Inventory = cache.CurrentSells[ecto.Id]
                });
            }

            var statTypes = new List<String> { "Solder's", "Rabid", "Dire", "Cavalier's", "Shaman's" };
            var insignias = new List<int> { 46712, 46710, 49522, 46709, 46708 }.Select(id => cache.Lookup[id]).ToList();
            var inscriptions = new List<int> { 46688, 46686, 46690, 46685, 46684 }.Select(id => cache.Lookup[id]).ToList();

            var ChampItems = new List<int> { 44978, 44980, 44983, 72191, 44982, 44960, 44977, 44991, 44984, 44985, 44967, 44964, 44974, 44965, 44971, 44976, 44962, 44961, 44986, 44973, 44969, 44988, 44992, 44968, 44987, 44963, 44990, 44966, 44972, 44975, 44989, 44979, 44981, 44999, 44970 };


            var exotics = cache.Items.Where(i => i.Rarity == "Exotic" && (i.Type == "Weapon" || i.Type == "Armor" || i.Type == "Trinket") && i.Level >= 68).ToList();
            cache.LoadListings(exotics.Select(i => i.Id));

            foreach (var item in exotics)
            {
                var inscriptionProfit = 0;
                if (statTypes.Contains(item.StatName) && item.Upgrade1 != 0)
                {
                    if (item.Type == "Weapon" && !ChampItems.Contains(item.Id))
                    {
                        inscriptionProfit = (int)(0.40 * inscriptions.First(i => i.Name.StartsWith(item.StatName)).FlipSell);
                    }
                    else if (item.Type == "Armor")
                    {
                        inscriptionProfit = (int)(0.40 * insignias.First(i => i.Name.StartsWith(item.StatName)).FlipSell);
                    }
                }

                var upgrade = cache.Lookup.ContainsKey(item.Upgrade1) ? cache.Lookup[item.Upgrade1].FlipSell : 0;

                int income = (int)(ecto.FlipSell * 1.2 + inscriptionProfit + upgrade).AfterTP();
                result.Add(new TradingAction($"gearsalvage_{item.Id}_{item.Name}")
                {
                    Description = "Extract + Salvage (Silverfed)",
                    Item = item,
                    MaxAmount = (int)(item.AdjustedBuyVelocity),
                    BaseCost = Settings.MediumTaskCost,
                    CostPer = item.FlipBuy + 60,
                    IncomePer = income,
                    SafeProfitPercentage = float.PositiveInfinity,
                    Inventory = cache.CurrentSells[ecto.Id]
                });

                var goodListings = cache.SellListings[item.Id].Where(l => l.Price < income).ToList();
                if (goodListings.Any())
                {
                    var totalCount = goodListings.Sum(l => l.Quantity);
                    var totalPrice = goodListings.Sum(l => l.Quantity * l.Price);
                    var maxPrice = goodListings.Max(l => l.Price);

                    result.Add(new TradingAction($"gearsalvage_{item.Id}_{item.Name}")
                    {
                        MaxAmount = totalCount,
                        Description = $"Instabuy {item.Name} @{maxPrice.GoldFormat()}, Extract + Salvage (Silverfed)",
                        Item = item,
                        CostPer = totalPrice / totalCount,
                        IncomePer = income,
                        BaseCost = Settings.HardTaskCost,
                        SafeProfitPercentage = float.PositiveInfinity
                    });
                }
            }
            return result;

        }
    }
}
