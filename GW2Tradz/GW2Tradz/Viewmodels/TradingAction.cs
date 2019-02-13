using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GW2Tradz.Viewmodels
{
    public class TradingAction
    {
        public string Description{get; set; }
        public Item Item { get; set; }
        public int Profit { get; set; }
        public float ProfitPercentage { get; set; }
        public int Amount { get; set; }
    }
}
