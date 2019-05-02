using GW2Tradz.Util;
using GW2Tradz.Viewmodels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GW2Tradz.Networking
{
    public class Cache
    {
        public Dictionary<int, Item> Lookup = new Dictionary<int, Item> { };
        public List<Dye> Dyes { get; private set; }
        public List<Recipe> Recipes { get; private set; }
        public DefaultDictionary<int, int> CurrentSells { get; private set; }
        public DefaultDictionary<int, int> CurrentBuys { get; private set; }
        public int WalletGold { get; private set; }
        public DeliveryBox DeliveryBox { get; private set; }

        public void Update(List<Item> items)
        {
            foreach (var item in items)
            {
                //linq dictionary merge?
                if (Lookup.TryGetValue(item.Id, out Item cached))
                {
                    cached.Update(item);
                }
                else
                {
                    Lookup[item.Id] = item;
                }
            }
        }

        public IEnumerable<Item> Items => Lookup.Values;

        public void Load()
        {
            var silver = new Silveress();
            var gw2 = new GW2();
            var gw2Profits = new GW2Profits();
            Update(silver.FetchBasicInfo());
            Dyes = gw2.FetchDyes();
            foreach (var dye in Dyes.Where(d => d.Item.HasValue))
            {
                Lookup.TryGetValue(dye.Item.Value, out var item);
                dye.ItemData = item;
            }
            Recipes = gw2.FetchRecipes();
            Recipes.AddRange(gw2Profits.FetchRecipes().Where(r => r.Id < 0));
            CurrentSells = new DefaultDictionary<int, int>(gw2.FetchCurrentSells());
            CurrentBuys = new DefaultDictionary<int, int>(gw2.FetchCurrentBuys());
            WalletGold = gw2.WalletGold();
            DeliveryBox = gw2.FetchDeliveryBox();
        }
    }
}
