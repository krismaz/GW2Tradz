using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GW2Tradz
{
    public static class Settings
    {
        public static int TotalGold = 700.Gold();
        public static int EasyTaskCost = 50.Silver();
        public static int MediumTaskCost = 2.Gold();
        public static int HardTaskCost = 10.Gold();
        public static double SafeMinimumMargin = 10.Percent();
        public static double UnsafeMinumumMargin = 50.Percent();
        public static int VelocityUncertainty = 5;

    }
}
