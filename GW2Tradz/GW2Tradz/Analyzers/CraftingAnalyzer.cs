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


            var sources = cache.Lookup.ToDictionary(kv => kv.Key, kv => kv.Value.SellPrice);
            sources[-1] = 1;

            //Gobbler shit
            sources[46731] = 0;
            sources[46733] = 0;
            sources[46735] = 0;

            var validRecipes = cache.Recipes
                    .Where(r => r.Ingredients.All(i => sources.ContainsKey(i.ItemId) && cache.Lookup.ContainsKey(r.OutputItemId)));

            var result = new List<TradingAction> { };

            foreach (var recipe in validRecipes)
            {

                var item = cache.Lookup[recipe.OutputItemId];

                if (recipe.Disciplines.Contains("Mystic Forge") && (item.Name.Contains("Rune") || item.Name.Contains("Sigil")))
                {
                    continue;
                }


                var cost = recipe.Ingredients.Sum(i => i.Count * sources[i.ItemId]) / recipe.OutputItemCount;
                result.Add(new TradingAction
                {
                    MaxAmount = dailyRecipes.Contains(recipe.Id) ? 1 : (int)item.AdjustedSellVelocity - Settings.VelocityUncertainty - cache.CurrentSells[recipe.OutputItemId],
                    Description = $"{item.Name} - {string.Join(", ", recipe.Disciplines)} ({string.Join(", ", recipe.Ingredients.Select(i => cache.Lookup.ContainsKey(i.ItemId) ? cache.Lookup[i.ItemId].Name : "?"))})",
                    Item = item,
                    CostPer = (int)cost,
                    IncomePer = item.SellPrice.AfterTP(),
                    BaseCost = Settings.HardTaskCost,
                    SafeProfitPercentage = Settings.UnsafeMinumumMargin
                });
            }
            return result;
        }

    }
}
