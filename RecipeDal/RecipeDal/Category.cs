using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RecipeDal
{
    public class Category
    {
        // Properties
        public long CategoryId { get; set; }
        [StringLength(50)]
        public string Description { get; set; }

        // Children
        public virtual ICollection<Recipe> Recipes { get; set; }
    }
}
