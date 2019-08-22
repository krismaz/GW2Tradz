using GW2Tradz.Networking;
using GW2Tradz.Viewmodels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                result.Add(new TradingAction
                {
                    Description = "Salvage",
                    Item = item,
                    MaxAmount = (int)(item.AdjustedBuyVelocity),
                    BaseCost = Settings.MediumTaskCost,
                    CostPer = item.FlipBuy + 60,
                    IncomePer = (int)(ecto.FlipSell.AfterTP() * 0.875),
                    SafeProfitPercentage = Settings.SafeMinimumMargin,
                    Inventory = cache.CurrentSells[ecto.Id]
                });
            }

            var statTypes = new List<String> { "Solder's", "Rabid", "Dire", "Cavalier's", "Shaman's" };
            var insignias = new List<int> { 46712, 46710, 49522, 46709, 46708 }.Select(id => cache.Lookup[id]).ToList();
            var inscriptions = new List<int> { 46688, 46686, 46690, 46685, 46684 }.Select(id => cache.Lookup[id]).ToList();

            var ChampItems = new List<int> { 44978, 44980, 44983, 72191, 44982, 44960, 44977, 44991, 44984, 44985, 44967, 44964, 44974, 44965, 44971, 44976, 44962, 44961, 44986, 44973, 44969, 44988, 44992, 44968, 44987, 44963, 44990, 44966, 44972, 44975, 44989, 44979, 44981, 44999, 44970 };

            foreach (var item in cache.Items.Where(i => i.Rarity == "Exotic" && (i.Type == "Weapon" || i.Type == "Armor" || i.Type == "Trinket") && i.Level >= 68))
            {
                var inscriptionProfit = 0;
                if (statTypes.Contains(item.StatName) && item.Upgrade1 != 0)
                {
                    if (item.Type == "Weapon" && !ChampItems.Contains(item.Id))
                    {
                        inscriptionProfit = (int)(0.40 * inscriptions.First(i => i.Name.StartsWith(item.StatName)).FlipSell.AfterTP());
                    }
                    else if (item.Type == "Armor")
                    {
                        inscriptionProfit = (int)(0.40 * insignias.First(i => i.Name.StartsWith(item.StatName)).FlipSell.AfterTP());
                    }
                }



                result.Add(new TradingAction
                {
                    Description = "Salvage",
                    Item = item,
                    MaxAmount = (int)(item.AdjustedBuyVelocity),
                    BaseCost = Settings.MediumTaskCost,
                    CostPer = item.FlipBuy + 60,
                    IncomePer = (int)(ecto.FlipSell.AfterTP() * 1.2) + inscriptionProfit,
                    SafeProfitPercentage = Settings.SafeMinimumMargin,
                    Inventory = cache.CurrentSells[ecto.Id]
                });
            }
            return result;

        }
    }
}
