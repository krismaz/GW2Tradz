using GW2Tradz.Networking.viewmodels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GW2Tradz.Networking
{
    class Cache
    {
        Dictionary<int, Item> _lookup = new Dictionary<int, Item> { };

        public void Update(List<Item> items)
        {
            foreach(var item in items)
            {
                //linq dictionary merge?
                if(_lookup.TryGetValue(item.Id, out Item cached))
                {
                    cached.Update(item);
                }
                else
                {
                    _lookup[item.Id] = item;
                }
            }
        }

        public void UpdateHistory(List<History> items)
        {
            var grouped = items.GroupBy(h => h.Id);
            foreach(var entry in grouped)
            {
                _lookup[entry.Key].History = entry.ToList();
            }
        }

        public IEnumerable<Item> Resolve()
        {
            return _lookup.Values;
        }
    }
}
