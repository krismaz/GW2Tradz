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
            var items = cache.Lookup;
            var validRecipes = cache.Recipes
                    .Where(r => r.Ingredients.All(i => i.ItemId == -1 || items.ContainsKey(i.ItemId)) & items.ContainsKey(r.OutputItemId));

            var result = new List<TradingAction> { };

            foreach (var recipe in validRecipes)
            {
                var item = items[recipe.OutputItemId];
                var income = (int)(item.SellPrice * 0.85);

                var cost = recipe.Ingredients.Sum(i => i.Count * (i.ItemId == -1 ? 1 : items[i.ItemId].SellPrice))/recipe.OutputItemCount;
                var totalvelocity = item.WeekSellVelocity ?? 0;
                result.Add(new TradingAction
                {
                    MaxAmount = (int)totalvelocity - Settings.VelocityUncertainty - cache.CurrentSells[recipe.OutputItemId],
                    Description = $"{item.Name} - {string.Join(", ", recipe.Disciplines)} ({string.Join(", ", recipe.Ingredients.Select(i => i.ItemId == -1 ? "Coin" : items[i.ItemId].Name))})",
                    Item = item,
                    CostPer = (int)cost,
                    IncomePer = income,
                    BaseCost = Settings.HardTaskCost,
                    SafeProfitPercentage = Settings.UnsafeMinumumMargin
                });
            }
            return result;
        }

    }
}
