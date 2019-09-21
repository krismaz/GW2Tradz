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
    class CraftingAnalyzer : IAnalyzer
    {
        public List<TradingAction> Analyse(Cache cache)
        {

            var dailyRecipes = new HashSet<int> { 9790, 9792 };


            var sources = cache.Lookup.ToDictionary(kv => kv.Key, kv => Math.Max(kv.Value.SellPrice, kv.Value.BuyPrice));
            sources[-1] = 1;

            foreach (var recipe in cache.Recipes.Where(r => r.Id < 0 && r.Disciplines[0] == "Merchant" && r.Ingredients.Count == 1 && r.Ingredients[0].ItemId == -1))
            {
                sources[recipe.OutputItemId] = recipe.Ingredients[0].Count / (int)recipe.OutputItemCount;
            }

            //Gobbler shit
            //sources[46731] = 0;
            //sources[46733] = 0;
            //sources[46735] = 0;

            var validRecipes = cache.Recipes
                    .Where(r => r.Type != "Bulk" && r.Type != "Feast" && r.Ingredients.All(i => sources.ContainsKey(i.ItemId) && cache.Lookup.ContainsKey(r.OutputItemId)) && r.Id > 0 && r.Id != 10456);

            var result = new List<TradingAction> { };

            List<Recipe> InstantRecipes = new List<Recipe>();

            foreach (var recipe in validRecipes)
            {

                var item = cache.Lookup[recipe.OutputItemId];
                var spike = item.SellPrice > item.YearSellAvg * 5;


                var cost = recipe.Ingredients.Sum(i => i.Count * sources[i.ItemId]);
                result.Add(new TradingAction($"crafting_{recipe.Id}_{item.Name}")
                {
                    MaxAmount = dailyRecipes.Contains(recipe.Id) ? 1 : (int)(item.AdjustedSellVelocity / recipe.OutputItemCount),
                    Description = (spike ? "(spike) " : "") + $"{item.Name} - {string.Join(", ", recipe.Disciplines)} ({string.Join(", ", recipe.Ingredients.Select(i => cache.Lookup.ContainsKey(i.ItemId) ? cache.Lookup[i.ItemId].Name : "?"))})",
                    Item = item,
                    CostPer = cost,
                    IncomePer = (int)((float)(item.SellPrice) * recipe.OutputItemCount).AfterTP(),
                    BaseCost = Settings.HardTaskCost,
                    SafeProfitPercentage = (spike && (item.Type == "Weapon" || item.Type == "Armor")) ? float.PositiveInfinity : Settings.UnsafeMinumumMargin,
                    Inventory = cache.CurrentSells[recipe.OutputItemId] / (int)recipe.OutputItemCount
                });

                if (cost < item.BuyPrice.AfterTP() * recipe.OutputItemCount)
                {
                    InstantRecipes.Add(recipe);
                }

            }

            cache.LoadListings(InstantRecipes.Select(r => r.OutputItemId).Distinct());
            foreach (var recipe in InstantRecipes)
            {
                var item = cache.Lookup[recipe.OutputItemId];
                var cost = recipe.Ingredients.Sum(i => i.Count * sources[i.ItemId]);

                var goodListings = cache.BuyListings[item.Id].Where(l => l.Price.AfterTP() > cost / recipe.OutputItemCount);

                if (!goodListings.Any())
                {
                    continue;
                }


                float totalCount = goodListings.Sum(l => l.Quantity);
                float totalIncome = goodListings.Sum(l => l.Quantity * l.Price).AfterTP();
                var minPrice = goodListings.Min(l => l.Price);

                result.Add(new TradingAction($"crafting_instant_{recipe.Id}_{item.Name}")
                {
                    MaxAmount = dailyRecipes.Contains(recipe.Id) ? 1 : (int)(totalCount / recipe.OutputItemCount),
                    Description = $"InstaSell ({minPrice.GoldFormat()}) <- { item.Name } - { string.Join(", ", recipe.Disciplines) } ({ string.Join(", ", recipe.Ingredients.Select(i => cache.Lookup.ContainsKey(i.ItemId) ? cache.Lookup[i.ItemId].Name : "?")) })",
                    Item = item,
                    CostPer = cost,
                    IncomePer = (int)(totalIncome / totalCount * recipe.OutputItemCount),
                    BaseCost = 0,
                    SafeProfitPercentage = Settings.SafeMinimumMargin
                });
            }
            return result;
        }

    }
}
