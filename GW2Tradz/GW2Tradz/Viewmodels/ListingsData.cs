using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GW2Tradz.Viewmodels
{
    public class ListingsData
    {
        public int Id { get; set; }
        public List<Listing> Buys { get; set; }
        public List<Listing> Sells { get; set; }
    }
}
