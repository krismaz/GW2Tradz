using GW2Tradz.Util;
using GW2Tradz.Viewmodels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GW2Tradz.Networking
{
    public class Cache
    {
        public Dictionary<int, Item> Lookup = new Dictionary<int, Item> { };
        public List<Dye> Dyes { get; private set; }
        public List<Recipe> Recipes { get; private set; }
        public List<Item> Materials { get; private set; }
        public DefaultDictionary<int, int> CurrentSells { get; private set; }
        public DefaultDictionary<int, int> CurrentBuys { get; private set; }
        public DefaultDictionary<int, List<Listing>> BuyListings { get; private set; } = new DefaultDictionary<int, List<Listing>> { };
        public DefaultDictionary<int, List<Listing>> SellListings { get; private set; } = new DefaultDictionary<int, List<Listing>> { };
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



        public void UpdateHistory(List<History> items)
        {
            var grouped = items.GroupBy(h => h.ItemId);
            foreach (var entry in grouped)
            {
                if(!Lookup.ContainsKey(entry.Key))
                {
                    

                    continue;
                }
                Lookup[entry.Key].History = entry.ToList();
            }
        }

        public IEnumerable<Item> Items => Lookup.Values;

        private GW2 _gw2 = new GW2();
        private Silveress _silver = new Silveress();
        private GW2Profits _gw2Profits = new GW2Profits();
        private GW2BLTC _gW2BLTC = new GW2BLTC();


        public void Load()
        {
            Update(_silver.FetchBasicInfo());
            Dyes = _gw2.FetchDyes();
            foreach (var dye in Dyes.Where(d => d.Item.HasValue))
            {
                Lookup.TryGetValue(dye.Item.Value, out var item);
                dye.ItemData = item;
            }
            Recipes = _gw2.FetchRecipes();
            Recipes.AddRange(_gw2Profits.FetchRecipes().Where(r => r.Id < 0));
            CurrentSells = new DefaultDictionary<int, int>(_gw2.FetchCurrentSells());
            CurrentBuys = new DefaultDictionary<int, int>(_gw2.FetchCurrentBuys());
            WalletGold = _gw2.WalletGold();
            DeliveryBox = _gw2.FetchDeliveryBox();
            Materials = _gw2.FetchMaterials().SelectMany(c => c.Items.Where(i => Lookup.ContainsKey(i)).Select(i => Lookup[i])).ToList();
            ;

            if (_silver.FetchHistory(new List<int> { 19721 }).Count()<6)
            {
                MessageBox.Show("Silver's data is broken!\n" +
                    "Scraping gw2bltc, this might be slow");
                ScrapeGW2BLTC();
            }
        }

        public void LoadListings(IEnumerable<int> ids)
        {
            var missing = ids.Except(BuyListings.Keys);
            foreach(var ld in _gw2.FetchListings(missing))
            {
                BuyListings[ld.Id] = ld.Buys;
                SellListings[ld.Id] = ld.Sells;
            }
        }


        public void LoadHistory(IEnumerable<int> ids)
        {
            var histories = _silver.FetchHistory(ids);
            
            UpdateHistory(histories);
        }

        private void ScrapeGW2BLTC()
        {
            foreach(var item in Lookup.Values)
            {
                item.WeekBuyVelocity = 0;
                item.WeekSellVelocity = 0;
            }
            foreach(var item in _gW2BLTC.ScrapeItems().Where(i=>Lookup.ContainsKey(i.Id)))
            {
                Lookup[item.Id].Update(item);
            }
        }
    }
}
