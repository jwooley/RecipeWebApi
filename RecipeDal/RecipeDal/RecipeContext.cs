using System.Data.Entity;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace RecipeDal
{
    public class RecipeContext : DbContext
    {
        // Constructors
        public RecipeContext()
        {
            this.Configuration.LazyLoadingEnabled = false;
        }

        public RecipeContext(string connString) : base(connString)
        {
        }

        // Factory
        public static RecipeContext ContextFactory([CallerMemberName] string memberName = "")
        {
            var context = new RecipeContext();
            context.Configuration.LazyLoadingEnabled = false;
            context.CallingMethod = memberName;
            context.Database.Log = val => Trace.WriteLine(val);
            return context;
        }

        // DBSets
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Direction> Directions { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }

        /// <summary>
        /// Name of the method that created the request.
        /// Used for logging purposes.
        /// </summary>
        /// <returns></returns>
        public string CallingMethod { get; private set; }
    }
}
