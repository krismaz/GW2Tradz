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
    class ClickingAnalyser : IAnalyzer
    {
        public List<TradingAction> Analyse(Cache cache)
        {

            var sources = cache.Lookup.ToDictionary(kv => kv.Key, kv => kv.Value.SellPrice);
            sources[-1] = 1;


            var validRecipes = cache.Recipes
                    .Where(r =>r.Disciplines.Contains("Double Click") && r.Ingredients.All(i => sources.ContainsKey(i.ItemId)) && cache.Lookup.ContainsKey(r.OutputItemId));

            var result = new List<TradingAction> { };
            var instantRecipes = new List<Recipe> { };

            foreach (var recipe in validRecipes)
            {
                var input = cache.Lookup[recipe.Ingredients[0].ItemId];
                var output = cache.Lookup[recipe.OutputItemId];
                var income = (int)(output.SellPrice * recipe.OutputItemCount).AfterTP();
                result.Add(new TradingAction($"clicking_{input.Id}_{input.Name}_{recipe.Id}")
                {
                    MaxIn = (int)input.AdjustedBuyVelocity,
                    MaxOut = (int)(output.AdjustedSellVelocity / recipe.OutputItemCount),
                    Description = $"{input.Name} -> {recipe.OutputItemCount} x {output.Name}",
                    Item = input,
                    CostPer = input.FlipBuy,
                    IncomePer = income,
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
                var listings = cache.AccumulateSellListings(input, income, max);

                if (!listings.Valid)
                {
                    result.Add(new TradingAction($"clicking_{input.Id}_{input.Name}_{recipe.Id}_2")
                    {
                        MaxOut = max,
                        MaxIn = listings.Amount,
                        Description = $"Instabuy {input.Name} @ {listings.MaxPrice.GoldFormat()} -> {recipe.OutputItemCount} x {output.Name}",
                        Item = input,
                        CostPer = listings.Cost / listings.Amount,
                        IncomePer = income,
                        SafeProfitPercentage = Settings.SafeMinimumMargin,
                        Inventory = cache.CurrentSells[output.Id] / (int)recipe.OutputItemCount
                    });
                }
            }
            return result;
        }
    }
}
