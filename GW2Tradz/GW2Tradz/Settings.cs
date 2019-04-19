using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GW2Tradz
{
    public static class Settings
    {
        public static int TotalGold = 500.Gold();
        public static int Spread = 10;
        public static int EasyTaskCost = 50.Silver();
        public static int MediumTaskCost = 2.Gold();
        public static int HardTaskCost = 10.Gold();
        public static double SafeMinimumMargin = 10.Percent();
        public static double UnsafeMinumumMargin = 50.Percent();
        public static int VelocityUncertainty = 0;
        public static string ApiKey = "1070D853-612C-6042-AB29-69C9E2D06ACE7FE1C8F2-7AEE-4C3C-9646-F65BEC5E1F13";

    }
}
