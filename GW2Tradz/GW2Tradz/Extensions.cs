using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GW2Tradz
{
    static class Extensions
    {
        public static IEnumerable<IEnumerable<T>> Chunk<T>(this IEnumerable<T> source, int chunksize)
        {
            while (source.Any())
            {
                yield return source.Take(chunksize);
                source = source.Skip(chunksize);
            }
        }

        public static int Gold(this int i) => i * 100 * 100;

        public static int Silver(this int i) => i * 100;

        public static double Percent(this int i) => ((double)i).Percent();

        public static double Percent(this double d) => d / 100d;

        public static double AfterTP(this double d) => d * 0.85d;

        public static int AfterTP(this int i) => (int)(i * 0.85d);

        public static string GoldFormat(this int value)
        {
            var coins = (int)value;
            if (coins < 0)
            {
                return "-" + GoldFormat(-coins);
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
    }
}
