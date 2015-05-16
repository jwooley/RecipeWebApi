using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecipeDal
{
    public class Recipe
    {
        // properties
        public long RecipeId { get; set; }
        public string Title { get; set; }
        public decimal? ServingQuantity { get; set; }
        public string ServingMeasure { get; set; }

        // Child Collections
        [ForeignKey("CategoryId")]
        public virtual ICollection<Category> Categories { get; set; }
        public virtual ICollection<Ingredient> Ingredients { get; set; }
        public virtual ICollection<Direction> Directions { get; set; }
    }
}