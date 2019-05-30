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
                    .Where(r => r.Ingredients.All(i => sources.ContainsKey(i.ItemId) && cache.Lookup.ContainsKey(r.OutputItemId)) && !r.Disciplines.Contains("Double Click"));

            var result = new List<TradingAction> { };

            foreach (var recipe in validRecipes)
            {

                var item = cache.Lookup[recipe.OutputItemId];

                if (recipe.Disciplines.Contains("Mystic Forge") && (item.Name.Contains("Rune") || item.Name.Contains("Sigil")))
                {
                    continue;
                }

                var maxAmount = (int)((item.AdjustedSellVelocity - cache.CurrentSells[recipe.OutputItemId])/recipe.OutputItemCount);

                if (dailyRecipes.Contains(recipe.Id))
                {
                    maxAmount = 1;
                }

                var cost = recipe.Ingredients.Sum(i => i.Count * sources[i.ItemId]);
                result.Add(new TradingAction
                {
                    MaxAmount = maxAmount,
                    Description = $"{item.Name} - {string.Join(", ", recipe.Disciplines)} ({string.Join(", ", recipe.Ingredients.Select(i => cache.Lookup.ContainsKey(i.ItemId) ? cache.Lookup[i.ItemId].Name : "?"))})",
                    Item = item,
                    CostPer = cost,
                    IncomePer = (int)((float)(item.SellPrice) * recipe.OutputItemCount).AfterTP(),
                    BaseCost = Settings.HardTaskCost,
                    SafeProfitPercentage = Settings.UnsafeMinumumMargin
                });
            }
            return result;
        }

    }
}
