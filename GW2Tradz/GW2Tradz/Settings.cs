using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GW2Tradz
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Settings
    {
        [JsonProperty]
        public static int TotalCoins = -1; // Total amoint of coins available, -1 means pulling the data online

        [JsonProperty]
        public static int Spread = 10; // Use at most 1/Spread of budget per action

        [JsonProperty]
        public static int EasyTaskCost = 0; // Task cost will assume your time is worth money

        [JsonProperty]
        public static int MediumTaskCost = 0; // Different tasks are assigned different amounts of 'work'

        [JsonProperty]
        public static int HardTaskCost = 0; // Mostly usefulle when sorting by profit margin

        [JsonProperty]
        public static double SafeMinimumMargin = 15.Percent(); // Safety margins hide risky transactions

        [JsonProperty]
        public static double UnsafeMinumumMargin = 40.Percent(); // They are only checked in the combined analyzer

        [JsonProperty]
        public static int VelocityUncertainty = 0; // Amount subtracted from daily velocities, can be used to hide slow items

        [JsonProperty]
        public static float VelocityFactor = 4f; // Assume we can move 1/Factor - Uncertainty of the daily velocity

        [JsonProperty]
        public static string ApiKey = "1070D853-612C-6042-AB29-69C9E2D06ACE7FE1C8F2-7AEE-4C3C-9646-F65BEC5E1F13"; // This is my API key, get your own

        [JsonProperty]
        public static int MaxSaneAmount = 15000; // Maximuum sell/buy velocity

    }
}
