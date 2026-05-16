using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GW2Tradz.Viewmodels
{
    public class ListingGroup
    {
        public ListingGroup(List<Listing> listings)
        {
            Listings = listings;
        }
        public bool Valid => Listings.Any();

        public List<Listing> Listings { get; }
        public int Amount  => Listings.Sum(l => l.Quantity);
        public int Cost  => Listings.Sum(l => l.Quantity*l.Price);
        public int Sell  => Listings.Sum(l => l.Quantity * l.Price.AfterTP());

        public int MinPrice => Listings.Min(l => l.Price);
        public int MaxPrice => Listings.Max(l => l.Price);
    }
}
