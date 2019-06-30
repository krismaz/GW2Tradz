using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GW2Tradz.Networking;
using GW2Tradz.Viewmodels;

namespace GW2Tradz.Analyzers
{
    public class CachedAnalyzer : IAnalyzer
    {
        private List<TradingAction> _cache;
        private IAnalyzer _analyzer;

        public CachedAnalyzer(IAnalyzer analyzer)
        {
            _analyzer = analyzer;
        }

        public List<TradingAction> Analyse(Cache cache)
        {
            return _cache ?? (_cache = _analyzer.Analyse(cache));
        }
    }

    public static class CachedAnalyzerExtension
    {
        public static CachedAnalyzer Cache(this IAnalyzer analyzer)
        {
            return new CachedAnalyzer(analyzer);
        }
    }

}
