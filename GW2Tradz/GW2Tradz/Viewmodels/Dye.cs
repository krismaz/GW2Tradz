using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GW2Tradz.Viewmodels
{
    public class Dye
    {
        public static Dictionary<string, List<int>> Salvages = new Dictionary<string, List<int>>
        {
            ["Brown"] = new List<int> { 74982 }, //Do not remove List<int>, on pain of runtime errors
            ["White"] = new List<int> { 75862 },
            ["Blue"] = new List<int> { 75694 },
            ["Black"] = new List<int> { 70426 },
            ["Gray"] = new List<int> { 75862, 70426 },
            ["Red"] = new List<int> { 71692 },
            ["Orange"] = new List<int> { 75270 },
            ["Purple"] = new List<int> { 77112 },
            ["Yellow"] = new List<int> { 71952 },
            ["Green"] = new List<int> { 76799 }
        };

        public static Dictionary<string, double> SalvageRates = new Dictionary<string, double>
        {
            ["Fine"] = 3,
            ["Masterwork"] = 6.5,
            ["Rare"] = 10.4
        };


        public int? Item { get; set; }
        public List<string> Categories { get; set; }
        public string Hue => Categories.FirstOrDefault();
        public Item ItemData { get; set; }
    }
}
