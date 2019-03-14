using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GW2Tradz.Viewmodels
{
    public class Recipe
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<string> Disciplines { get; set; }
        public int OutputItemId { get; set; }
        public float OutputItemCount { get; set; }
        public List<Ingredient> Ingredients { get; set; }

        public class Ingredient
        {
            public int ItemId { get; set; }
            public int Count { get; set; }
        }
    }
}
