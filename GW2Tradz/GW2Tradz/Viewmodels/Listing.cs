using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GW2Tradz.Viewmodels
{
    public class Listing
    {
        public int ItemId { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }
        public int UnitPrice
        {
            get
            {
                return Price;
            }
            set
            {
                Price = value;
            }
        }
    }
}
