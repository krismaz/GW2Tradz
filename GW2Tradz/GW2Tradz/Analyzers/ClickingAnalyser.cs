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
            var instantRecipes = new List<Recipe> { };

            foreach (var recipe in validRecipes)
            {
                var input = cache.Lookup[recipe.Ingredients[0].ItemId];
                var output = cache.Lookup[recipe.OutputItemId];
                var income = (int)(output.SellPrice * recipe.OutputItemCount).AfterTP();
                result.Add(new TradingAction($"clicking_{input.Id}_{input.Name}_{recipe.Id}")
                {
                    MaxAmount = (int)Math.Min(input.AdjustedBuyVelocity, output.AdjustedSellVelocity / (int)recipe.OutputItemCount),
                    Description = $"{input.Name} -> {recipe.OutputItemCount} x {output.Name}",
                    Item = input,
                    CostPer = input.FlipBuy,
                    IncomePer = income,
                    BaseCost = Settings.EasyTaskCost,
                    SafeProfitPercentage = Settings.SafeMinimumMargin,
                    Inventory = cache.CurrentSells[output.Id] / (int)recipe.OutputItemCount
                });

                if (input.SellPrice < income)
                {
                    instantRecipes.Add(recipe);
                }
            }

            cache.LoadListings(instantRecipes.Select(r => r.Ingredients[0].ItemId));

            foreach (var recipe in instantRecipes)
            {
                var input = cache.Lookup[recipe.Ingredients[0].ItemId];
                var output = cache.Lookup[recipe.OutputItemId];
                var income = (int)(output.SellPrice * recipe.OutputItemCount).AfterTP();
                var max = (int)(output.AdjustedSellVelocity / (int)recipe.OutputItemCount);


                var acc = 0;
                var listings = cache.SellListings[input.Id].Where(l => l.Price < income).TakeWhile(l =>
                {
                    var acc0 = acc;
                    acc += l.Quantity;
                    return acc0 < max;
                }).ToList(); ;
                var quantity = listings.Sum(l => l.Quantity);
                var totalcost = listings.Sum(l => l.Quantity * l.Price);

                result.Add(new TradingAction($"clicking_{input.Id}_{input.Name}_{recipe.Id}")
                {
                    MaxAmount = max,
                    Description = $"Instabuy {input.Name} @ {listings.Max(l=>l.Price).GoldFormat()} -> {recipe.OutputItemCount} x {output.Name}",
                    Item = input,
                    CostPer = totalcost / quantity,
                    IncomePer = income,
                    BaseCost = Settings.EasyTaskCost,
                    SafeProfitPercentage = Settings.SafeMinimumMargin,
                    Inventory = cache.CurrentSells[output.Id] / (int)recipe.OutputItemCount
                });
            }
            return result;
        }
    }
}
