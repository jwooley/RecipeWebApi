using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace RecipeDal
{
    public class RecipeContext : DbContext
    {
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Recipe>()
                .HasMany(r => r.Categories)
                .WithMany(c => c.Recipes)
                .Map(m =>
                {
                    m.MapLeftKey("Recipe_RecipeId");
                    m.MapRightKey("Category_CategoryId");
                    m.ToTable("RecipeCategories");
                });

            base.OnModelCreating(modelBuilder);
        }
        // Constructors
        public RecipeContext()
        {
            //this.Configuration.LazyLoadingEnabled = false;
        }

        public RecipeContext(string connString) : base(connString)
        {
        }

        // Factory
        public static RecipeContext ContextFactory([CallerMemberName] string memberName = "")
        {
            var context = new RecipeContext();
            // Don't use migrations. Just accept the structure and manage database schema manually.
            Database.SetInitializer<RecipeContext>(null);

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
        public DbSet<Log> Logs { get; set; }

        public async Task<IEnumerable<Recipe>> SearchRecipeAsync(string searchText)
        {
            return await Database.SqlQuery<Recipe>("sRecipeSearch @searchText", 
                new SqlParameter("searchText", searchText)).ToListAsync();
        }

        /// <summary>
        /// Name of the method that created the request.
        /// Used for logging purposes.
        /// </summary>
        /// <returns></returns>
        public string CallingMethod { get; private set; }
    }
}
