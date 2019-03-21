using GW2Tradz.Networking;
using GW2Tradz.Viewmodels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GW2Tradz.Analyzers
{
    public interface IAnalyzer
    {
        List<TradingAction> Analyse(Cache cache);   
    }
}
