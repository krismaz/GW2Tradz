using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GW2Tradz
{
    public static class Settings
    {
        public static int TotalGold = -1;
        public static int Spread = 10;
        public static int EasyTaskCost = 10.Silver();
        public static int MediumTaskCost = 20.Silver();
        public static int HardTaskCost = 100.Silver();
        public static double SafeMinimumMargin = 10.Percent();
        public static double UnsafeMinumumMargin = 50.Percent();
        public static int VelocityUncertainty = 0;
        public static float VelocityFactor = 5f;
        public static string ApiKey = "1070D853-612C-6042-AB29-69C9E2D06ACE7FE1C8F2-7AEE-4C3C-9646-F65BEC5E1F13";

    }
}
