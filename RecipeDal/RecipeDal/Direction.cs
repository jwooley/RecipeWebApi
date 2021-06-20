using System.ComponentModel.DataAnnotations.Schema;

namespace RecipeDal
{
    [Table("Direction")]
    public class Direction
    {
        [Column("Id")]
        public long DirectionId { get; set; }
        public long LineNumber { get; set; }
        public string Description { get; set; }

        // Parent
        [Column("RecipeId")]
        public long RecipeId { get; set; }
        [ForeignKey("RecipeId")]
        public Recipe Recipe { get; set; }

    }
}