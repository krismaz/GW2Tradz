using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GW2Tradz.Util
{
    public class DefaultDictionary<TKey, TValue> : Dictionary<TKey, TValue>
    {
        public DefaultDictionary(Dictionary<TKey, TValue> d) : base(d)
        {
        }

        public new TValue this[TKey key]
        {
            get
            {
                TValue val;
                if (!TryGetValue(key, out val))
                {
                    val = default(TValue);
                    Add(key, val);
                }
                return val;
            }
            set { base[key] = value; }
        }
    }
}
