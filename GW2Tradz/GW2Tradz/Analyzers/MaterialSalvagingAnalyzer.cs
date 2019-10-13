using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GW2Tradz.Networking;
using GW2Tradz.Viewmodels;

namespace GW2Tradz.Analyzers
{
    class MaterialSalvagingAnalyzer : IAnalyzer
    {

        public List<TradingAction> Analyse(Cache cache)
        {
            List<TradingAction> result = new List<TradingAction> { };

            void salvage(Dictionary<int, double> results, int salvageCost, int itemId)
            {
                var item = cache.Lookup[itemId];
                var salvager = "???";
                if(salvageCost == 3)
                {
                    salvager = "Copperfed";
                }
                if (salvageCost == 60)
                {
                    salvager = "Silverfed";
                }

                var salvageSell = results.Select(kv => kv.Value * cache.Lookup[kv.Key].FlipSell).Sum().AfterTP();
                var salvageUse = results.Select(kv => kv.Value * cache.Lookup[kv.Key].FlipBuy).Sum();

                result.Add(new TradingAction($"salvage_{salvageCost}_{itemId}_{item.Name}_sell")
                {
                    Description = $"Salvage ({salvager}) and Sell",
                    Item = item,
                    MaxAmount = (int)(item.AdjustedBuyVelocity),
                    BaseCost = Settings.MediumTaskCost,
                    CostPer = item.FlipBuy + salvageCost,
                    IncomePer = (int)salvageSell,
                    SafeProfitPercentage = Settings.SafeMinimumMargin,
                    Inventory = 0
                });

                result.Add(new TradingAction($"salvage_{salvageCost}_{itemId}_{item.Name}_use")
                {
                    Description = $"Salvage ({salvager}) and Use",
                    Item = item,
                    MaxAmount = (int)(item.AdjustedBuyVelocity),
                    BaseCost = Settings.MediumTaskCost,
                    CostPer = item.FlipBuy + salvageCost,
                    IncomePer = (int)salvageUse,
                    SafeProfitPercentage = double.PositiveInfinity,
                    Inventory = 0
                });
            }

            //Bloodstone hide
            salvage(new Dictionary<int, double>
            {
                [19719] = 0.044,
                [19728] = 0.044,
                [19730] = 0.045,
                [19731] = 0.042,
                [19729] = 0.442,
                [19732] = 0.49,
            }, 3, 80681);

            salvage(new Dictionary<int, double>
            {
                [19719] = 0.05,
                [19728] = 0.049,
                [19730] = 0.048,
                [19731] = 0.051,
                [19729] = 0.506,
                [19732] = 0.553,
            }, 60, 80681);


            //Valuable metal scrap
            salvage(new Dictionary<int, double>
            {
                [19700] = 1.20,
                [19701] = 0.21
            }, 3, 21683);

            //Salvageable Intact Forged Scrap
            salvage(new Dictionary<int, double>
            {
                [19700] = 4.22,
                [19701] = 0.3,
                [82582] = 0.26
            }, 3, 82488);

            //Lump of Raw Ambrite
            salvage(new Dictionary<int, double>
            {
                [66637] = 1.99
            }, 3, 66670);

            //Discarded Garment
            salvage(new Dictionary<int, double>
            {
                [19748] = 1.6,
                [19745] = 0.09
            }, 3, 21675);

            //Unstable Metal Chunk
            salvage(new Dictionary<int, double>
            {
                [19697] = 0.16,
                [19699] = 0.95,
                [19702] = 0.47,
                [19700] = 0.15,
                [19701] = 0.32
            }, 3, 79079);

            //Hard Leather Strap
            salvage(new Dictionary<int, double>
            {
                [19729] = 1.26,
                [19732] = 0.096,
            }, 60, 21689);
            return result;
        }
    }
}
