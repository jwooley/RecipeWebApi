using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecipeDal
{
    [Table("Ingredient")]
    /// <summary>
    /// CRUD operations for the Ingredient resource
    /// </summary>
    public class Ingredient
    {
        [Column("Id")]
        public long IngredientId { get; set; }
        public int? SortOrder { get; set; }
        [MaxLength(50)]
        public string Units { get;set; }
        [MaxLength(50)]
        public string UnitType { get; set; }
        [MaxLength(50)]
        public string Description { get; set; }

        public long RecipeId { get; set; }

        [ForeignKey("RecipeId")]
        public Recipe Recipe { get; set; }
        
    }
}