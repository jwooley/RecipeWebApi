using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
