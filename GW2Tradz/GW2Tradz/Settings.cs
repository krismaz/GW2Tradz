using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GW2Tradz
{
    public static class Settings
    {
        public static int TotalGold = -1; // Total amoint of coins available, -1 means pulling the data online
        public static int Spread = 10; // Use at most 1/Spread of budget per action 
        public static int EasyTaskCost = 0; // Task cost will assume your time is worth money
        public static int MediumTaskCost = 0; // Different tasks are assigned different amounts of 'work'
        public static int HardTaskCost = 0; // Mostly usefulle when sorting by profit margin
        public static double SafeMinimumMargin = 15.Percent(); // Safety margins hide risky transactions
        public static double UnsafeMinumumMargin = 40.Percent(); // They are only checked in the combined analyzer
        public static int VelocityUncertainty = 0; // Amount subtracted from daily velocities, can be used to hide slow items
        public static float VelocityFactor = 7.5f; // Assume we can move 1/Factor - Uncertainty of the daily velocity
        public static string ApiKey = "1070D853-612C-6042-AB29-69C9E2D06ACE7FE1C8F2-7AEE-4C3C-9646-F65BEC5E1F13"; // This is my API key, get your own

    }
}
