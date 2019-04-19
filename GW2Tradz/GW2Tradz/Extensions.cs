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
    }
}
