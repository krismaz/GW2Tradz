using GW2Tradz.Networking;
using GW2Tradz.Viewmodels;
using System;
using System.Collections.Generic;
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


            foreach (var gemstone in gemstones)
            {
                var totalCost = 5 * ecto.FlipBuy + 75 * gemstone.FlipBuy;
                var totalIncome = 11.5 * amal.FlipSell;

                result.Add(new TradingAction
                {
                    MaxAmount = (int)gemstone.AdjustedBuyVelocity,
                    Description = $"Mystic Forge {gemstone.Name}x75 + 5 Ecto",
                    Item = gemstone,
                    CostPer = totalCost / 75,
                    IncomePer = (int)(totalIncome / 75).AfterTP(),
                    BaseCost = Settings.EasyTaskCost,
                    SafeProfitPercentage = Settings.SafeMinimumMargin
                });
            }

            return result;
        }
    }
}
