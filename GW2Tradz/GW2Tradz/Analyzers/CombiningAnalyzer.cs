using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GW2Tradz.Networking;
using GW2Tradz.Viewmodels;

namespace GW2Tradz.Analyzers
{
    class CombiningAnalyzer : IAnalyzer
    {
        public bool Filtering { get; init; } = true;

        public List<IAnalyzer> Analyzers { get; set; }

        public List<TradingAction> Analyse(Cache cache)
        {
            return Analyzers.SelectMany(a => a.Analyse(cache)).Where(t => !Filtering ||  t.Safe).ToList();
        }
    }
}
