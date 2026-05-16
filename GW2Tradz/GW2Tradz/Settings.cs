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
        public static int Spread = 5; // Use at most 1/Spread of budget per action

        [JsonProperty]
        public static double SafeMinimumMargin = 15.Percent(); // Safety margins hide risky transactions

        [JsonProperty]
        public static double UnsafeMinimumMargin = 40.Percent(); // They are only checked in the combined analyzer

        [JsonProperty]
        public static int VelocityUncertainty = 0; // Amount subtracted from daily velocities, can be used to hide slow items

        [JsonProperty]
        public static float VelocityFactor = 4f; // Assume we can move 1/Factor - Uncertainty of the daily velocity

        [JsonProperty]
        public static string ApiKey = "D29B3875-547F-2B47-AE06-961A0E3AB053D9799A10-D18F-4D75-8603-6BC8176EEB87"; // This is my API key, get your own

        [JsonProperty]
        public static int MaxSaneAmount = 15000; // Maximuum sell/buy velocity

        [JsonProperty]
        //https://fast.farming-community.eu/farming/calculator?item=empyreal-fragment
        public static int EmpyrialShardValue = 27;

        //https://fast.farming-community.eu/farming/calculator?item=salvaging-costs-per-research-note
        [JsonProperty]
        public static int ResearchNoteCost = 70;

        //https://fast.farming-community.eu/conversions/fractal-relic
        [JsonProperty]
        public static int FractalValue = 200;
    }
}
