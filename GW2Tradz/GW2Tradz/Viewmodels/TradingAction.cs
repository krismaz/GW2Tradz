using GW2Tradz.Networking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Math;

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
        public int MaxIn { get; set;  }
        public int MaxOut { get; set; }
        public int CostPer { get; set; }
        public double SafeProfitPercentage { get; set; }


        public int Amount => Max(0, Min(MaxIn, Min((Settings.TotalCoins / Settings.Spread) / Max(CostPer, 1), MaxOut - Inventory)));
        public int TotalIncome => Amount * IncomePer;
        public int TotalCost => Amount * CostPer ;
        public int Profit => TotalIncome - TotalCost;
        public double ProfitPercentage => Amount > 0 ? (double)Profit / (double)TotalCost : 0;
        public bool Safe => ProfitPercentage > SafeProfitPercentage;

    }
}
