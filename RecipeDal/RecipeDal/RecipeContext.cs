using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace RecipeDal
{
    public class RecipeContext : DbContext
    {
        #region config
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Recipe>()
                .HasMany(r => r.Categories)
                .WithMany(c => c.Recipes)
                .UsingEntity<Dictionary<string, object>>(
                    "RecipeCategory",
                    j => j
                        .HasOne<Category>()
                        .WithMany()
                        .HasForeignKey("CategoryId"),
                    j => j
                        .HasOne<Recipe>()
                        .WithMany()
                        .HasForeignKey("RecipeId"),
                    j =>
                    {
                        j.ToTable("RecipeCategory");
                        j.HasKey("RecipeId", "CategoryId");
                    });

            base.OnModelCreating(modelBuilder);
        }

        // Constructors
        public RecipeContext()
        {
            // EF Core doesn't use Configuration property
        }

        public RecipeContext(DbContextOptions<RecipeContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var config = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json", optional: true)
                    .Build();
                
                optionsBuilder.UseSqlServer(config.GetConnectionString("RecipeContext"))
                    .LogTo(message => Trace.WriteLine(message));
            }
            
            base.OnConfiguring(optionsBuilder);
        }

        // Factory
        public static RecipeContext ContextFactory([CallerMemberName] string memberName = "")
        {
            var context = new RecipeContext();

            // The following EF6 specific code is removed or updated for EF Core
            // Database.SetInitializer<RecipeContext>(null);
            // context.Configuration.LazyLoadingEnabled = false;
            // context.Database.Log = val => Trace.WriteLine(val);
            // context.Configuration.AutoDetectChangesEnabled = false;
            // context.Configuration.UseDatabaseNullSemantics = true;

            context.CallingMethod = memberName;
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
            // Using FromSqlInterpolated for better SQL injection protection
            return await Recipes.FromSqlInterpolated($"EXEC sRecipeSearch {searchText}").ToListAsync();
        }

        /// <summary>
        /// Name of the method that created the request.
        /// Used for logging purposes.
        /// </summary>
        /// <returns></returns>
        public string CallingMethod { get; private set; }
    }
}
