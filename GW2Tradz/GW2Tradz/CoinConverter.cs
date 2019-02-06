using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace GW2Tradz
{
    [ValueConversion(typeof(int), typeof(String))]
    class CoinConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var coins = (int)value;
            if(coins<0)
            {
                return "-" + Convert(-coins, targetType, parameter, culture);
            }
            var copper = coins % 100;
            var silver = (coins % 10000) / 100;
            var gold = coins / 10000;

            if (coins < 100)
            {
                return $"{copper}c";
            }
            if (coins < 10000)
            {
                return $"{silver}s{copper}c";
            }
            return $"{gold}g{silver}s{copper}c";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
