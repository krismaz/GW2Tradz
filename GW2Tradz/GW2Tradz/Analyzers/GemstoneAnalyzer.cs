using GW2Tradz.Networking;
using GW2Tradz.Viewmodels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GW2Tradz.Analyzers
{
    class GemstoneAnalyzer : IAnalyzer
    {
        public List<TradingAction> Analyse(Cache cache)
        {
            var result = new List<TradingAction>();

            var gemstones = new List<int> { 24773, 24502, 24884, 24516, 24508, 24522, 72504, 70957, 72315, 76179, 74988, 24515, 75654, 24510, 24512, 76491, 24520, 42010, 72436, 24524, 24533, 24532, 24518, 24514 }.Select(id => cache.Lookup[id]).ToList();
            var ecto = cache.Lookup[19721];
            var amal = cache.Lookup[68063];
            cache.LoadListings([amal.Id]);


            foreach (var gemstone in gemstones)
            {
                var totalCost = 5 * ecto.FlipBuy + 75 * gemstone.FlipBuy;
                var totalIncome = 11.5 * amal.FlipSell;

                result.Add(new TradingAction($"gemstone_{gemstone.Id}_{gemstone.Name}")
                {
                    MaxIn = (int)gemstone.AdjustedBuyVelocity,
                    MaxOut = (int)amal.AdjustedSellVelocity,
                    Description = $"Mystic Forge {gemstone.Name}x75 + 5 Ecto",
                    Item = gemstone,
                    CostPer = totalCost / 75,
                    IncomePer = (int)(totalIncome / 75).AfterTP(),
                    SafeProfitPercentage = Settings.SafeMinimumMargin,
                    Inventory = (int)(cache.CurrentSells[amal.Id] * 75 / 11.5)
                });

                var instant = cache.AccumulateBuyListings(amal, (int)(totalCost/11.5), (int)(gemstone.AdjustedBuyVelocity/75*11.5));
                if(instant.Valid)
                {
                    result.Add(new TradingAction($"gemstone_{gemstone.Id}_{gemstone.Name}_instant")
                    {
                        MaxIn = Settings.MaxSaneAmount,
                        MaxOut = instant.Amount,
                        Description = $"Mystic Forge {gemstone.Name}x75 + 5 Ecto, Sell instantly to {instant.MinPrice.GoldFormat()}",
                        Item = gemstone,
                        CostPer = (int)(totalCost / 11.5),
                        IncomePer = instant.Sell/instant.Amount,
                        SafeProfitPercentage = Settings.SafeMinimumMargin,
                    });
                };
            }
            var gemstoneIds = gemstones.Select(i => i.Id).ToList();
            foreach (var recipe in cache.Recipes.Where(r => r.Id > 0 && gemstoneIds.Contains(r.OutputItemId)))
            {
                var crystal = recipe.Ingredients.Select(i => cache.Lookup[i.ItemId]).Where(i => !i.Name.Contains("Dust")).FirstOrDefault(); //What's the dust called again?
                if (crystal == null)
                {
                    Debugger.Break();
                    continue;
                }
                var totalCost = 5 * ecto.FlipBuy + 75 * recipe.Ingredients.Sum(i => i.Count * cache.Lookup[i.ItemId].FlipBuy);
                var totalIncome = 11.5 * amal.FlipSell;

                result.Add(new TradingAction($"gemstone_{crystal.Id}_{crystal.Name}")
                {
                    MaxIn = (int)crystal.AdjustedBuyVelocity / 2,
                    MaxOut = (int)amal.AdjustedSellVelocity,
                    Description = $"Transmogrify {crystal.Name} and mystic forge to Amalgamated Gemstone",
                    Item = crystal,
                    CostPer = totalCost / 75,
                    IncomePer = (int)(totalIncome / 75).AfterTP(),
                    SafeProfitPercentage = Settings.SafeMinimumMargin,
                    Inventory = (int)(cache.CurrentSells[amal.Id] * 75 / 11.5)
                });

                var instant = cache.AccumulateBuyListings(amal, (int)(totalCost / 11.5), (int)(crystal.AdjustedBuyVelocity / 2 / 75 * 11.5));
                if (instant.Valid)
                {
                    result.Add(new TradingAction($"gemstone_{crystal.Id}_{crystal.Name}_instant")
                    {
                        MaxIn = Settings.MaxSaneAmount,
                        MaxOut = instant.Amount,
                        Description = $"Mystic Forge {crystal.Name}x75 + 5 Ecto, Sell instantly to {instant.MinPrice.GoldFormat()}",
                        Item = crystal,
                        CostPer = (int)(totalCost / 11.5),
                        IncomePer = instant.Sell / instant.Amount,
                        SafeProfitPercentage = Settings.SafeMinimumMargin,
                    });
                }
            }


            return result;
        }
    }
}
