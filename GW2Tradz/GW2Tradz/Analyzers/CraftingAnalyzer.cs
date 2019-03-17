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
        public List<TradingAction> Analyse(int budget, Cache cache)
        {
            var items = cache.Lookup;
            var validRecipes = cache.Recipes
                    .Where(r => r.Ingredients.All(i => i.ItemId == -1 || items.ContainsKey(i.ItemId)) & items.ContainsKey(r.OutputItemId));

            var result = new List<TradingAction> { };

            foreach(var recipe in validRecipes)
            {
                var item = items[recipe.OutputItemId];
                var income = (int)(recipe.OutputItemCount * item.SellPrice * 0.85);

                var cost = recipe.Ingredients.Sum(i => i.Count * (i.ItemId == -1 ? 1 : items[i.ItemId].SellPrice));
                var totalvelocity = Math.Min((item.WeekSellVelocity ?? 0) / recipe.OutputItemCount, recipe.Ingredients.Min(i => (i.ItemId == -1 ? float.PositiveInfinity : items[i.ItemId].WeekBuyVelocity ?? 0) / i.Count));
                if(totalvelocity > Settings.VelocityUncertainty && (income-cost) * (int)totalvelocity > Settings.HardTaskCost && (float)(income - cost) / (float)cost > Settings.UnsafeMinumumMargin)
                {
                    result.Add(new TradingAction
                    {
                        Amount = (int)totalvelocity,
                        Description = $"{item.Name} - {string.Join(", ", recipe.Disciplines)} ({string.Join(", ", recipe.Ingredients.Select(i => i.ItemId == -1 ? "Coin" : items[i.ItemId].Name))})",
                        Item = item,
                        Profit = (int)((income - cost) * totalvelocity),
                        ProfitPercentage = (float)(income - cost) / (float)cost
                    });
                }
            }
            return result;
        }

    }
}
