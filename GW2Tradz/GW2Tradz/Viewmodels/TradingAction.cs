using GW2Tradz.Networking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GW2Tradz.Viewmodels
{
    public class TradingAction
    {
        public TradingAction(string identifier)
        {
            Identifier = identifier;
        }


        public string Identifier{get; private set;}

        public int Inventory { get; set; }

        public string Description { get; set; }
        public Item Item { get; set; }
        public int IncomePer { get; set; }
        private int _maxAmount;
        public int MaxAmount { get => _maxAmount; set { _maxAmount = Math.Max(0, value); } }
        public int CostPer { get; set; }
        public int BaseCost { get; set; }
        public double SafeProfitPercentage { get; set; }


        public int Amount => Math.Max(0, Math.Min(MaxAmount, (Settings.TotalCoins / Settings.Spread) / CostPer - Inventory));
        public int TotalIncome => Amount * IncomePer;
        public int TotalCost => Amount * CostPer + BaseCost;
        public int Profit => TotalIncome - TotalCost;
        public double ProfitPercentage => Amount > 0 ? (double)Profit / (double)TotalCost : 0;
        public bool Safe => ProfitPercentage > SafeProfitPercentage;

    }
}
