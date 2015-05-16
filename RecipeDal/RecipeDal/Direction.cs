using System.ComponentModel.DataAnnotations.Schema;

namespace RecipeDal
{
    public class Direction
    {
        // Properties
        public long DirectionId { get; set; }
        public long LineNumber { get; set; }
        public string Description { get; set; }

        // Parent
        [Column("Recipe_RecipeId")]
        public long Recipe_RecipeId { get; set; }
        [ForeignKey("Recipe_RecipeId")]
        public Recipe Recipe { get; set; }

    }
}