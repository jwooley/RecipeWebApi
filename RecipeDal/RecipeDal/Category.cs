using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecipeDal
{
    [Table("Category")]
    public class Category
    {
        [Column("Id")]
        // Properties
        public long CategoryId { get; set; }
        [StringLength(50)]
        public string Description { get; set; }

        // Children
        public virtual ICollection<Recipe> Recipes { get; set; }
    }
}
