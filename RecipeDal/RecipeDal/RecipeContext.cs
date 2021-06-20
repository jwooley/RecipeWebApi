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
        #region config
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Recipe>()
                .HasMany(r => r.Categories)
                .WithMany(c => c.Recipes)
                .Map(m =>
                {
                    m.MapLeftKey("RecipeId");
                    m.MapRightKey("CategoryId");
                    m.ToTable("RecipeCategory");
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

            //context.Configuration.LazyLoadingEnabled = false;
            context.CallingMethod = memberName;
            context.Database.Log = val => Trace.WriteLine(val);
            context.Configuration.AutoDetectChangesEnabled = false;
            context.Configuration.UseDatabaseNullSemantics = true;

            return context;
        }
        #endregion

        // DBSets
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Direction> Directions { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<Log> Logs { get; set; }

        public async Task<IEnumerable<Recipe>> SearchRecipeAsync(string searchText)
        {
            return await Database.SqlQuery<Recipe>("sRecipeSearch @searchText",
                new SqlParameter(nameof(searchText), searchText)).ToListAsync();
        }

        /// <summary>
        /// Name of the method that created the request.
        /// Used for logging purposes.
        /// </summary>
        /// <returns></returns>
        public string CallingMethod { get; private set; }
    }
}
