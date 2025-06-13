using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Recipe.Web.Models
{
    public class RecipeResultModel
    {
        public string Title { get; set; }
        public string ServingMeasure { get; set; }
        public decimal? ServingQuantity { get; set; }
        public string Categories { get; set; }
        public string Directions { get; set; }
        public IngredientResultModel[] Ingredients { get; set; }
    }
    public class IngredientResultModel
    {
        public string Description { get; set; }
        public string UnitType { get; set; }
        public string Units { get; set; }
    }
}