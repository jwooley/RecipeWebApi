using System.ComponentModel.DataAnnotations;

namespace RecipeDal
{
    public class Ingredient
    {
        // Properties
        public long IngredientId { get; set; }
        public int? SortOrder { get; set; }
        [MaxLength(50)]
        public string Units { get;set; }
        [MaxLength(50)]
        public string UnitType { get; set; }
        [MaxLength(50)]
        public string Description { get; set; }

        // Parent
        //public Recipe Recipe { get; set; }
        
    }
}