using GW2Tradz.Networking;
using GW2Tradz.Viewmodels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GW2Tradz.Analyzers
{
    class ClickingAnalyser : IAnalyzer
    {
        public List<TradingAction> Analyse(Cache cache)
        {

            var sources = cache.Lookup.ToDictionary(kv => kv.Key, kv => kv.Value.SellPrice);
            sources[-1] = 1;


            var validRecipes = cache.Recipes
                    .Where(r => r.Ingredients.All(i => sources.ContainsKey(i.ItemId) && cache.Lookup.ContainsKey(r.OutputItemId)) && r.Disciplines.Contains("Double Click"));

            var result = new List<TradingAction> { };

            foreach (var recipe in validRecipes)
            {
                var input = cache.Lookup[recipe.Ingredients[0].ItemId];
                var output = cache.Lookup[recipe.OutputItemId];
                result.Add(new TradingAction($"clicking_{input.Id}_{input.Name}_{recipe.Id}")
                {
                    MaxAmount = (int)Math.Min(input.AdjustedBuyVelocity, output.AdjustedSellVelocity / (int)recipe.OutputItemCount),
                    Description = $"{input.Name} -> {recipe.OutputItemCount} x {output.Name}",
                    Item = input,
                    CostPer = input.FlipBuy,
                    IncomePer = (int)(output.SellPrice * recipe.OutputItemCount).AfterTP(),
                    BaseCost = Settings.EasyTaskCost,
                    SafeProfitPercentage = Settings.SafeMinimumMargin,
                    Inventory = cache.CurrentSells[output.Id] / (int)recipe.OutputItemCount
                });
            }
            return result;
        }
    }
}
