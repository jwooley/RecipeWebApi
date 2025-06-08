using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace RecipeDal
{
    public class RecipeContext : DbContext
    {
        private static IConfiguration _appConfiguration;

        private static IConfiguration AppConfiguration
        {
            get
            {
                if (_appConfiguration == null)
                {
                    var builder = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

                    _appConfiguration = builder.Build();
                }

                return _appConfiguration;
            }
        }

        #region config
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = AppConfiguration["ConnectionStrings:RecipeContext"];
            optionsBuilder.UseSqlServer(connectionString,
            options =>
                {
                    options.EnableRetryOnFailure(maxRetryCount: 3);
                    options.MaxBatchSize(30);
                })
                .LogTo(val => Trace.WriteLine(val));

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure the many-to-many relationship between Recipe and Category
            modelBuilder.Entity<Recipe>()
                .HasMany(r => r.Categories)
                .WithMany(c => c.Recipes)
                .UsingEntity<Dictionary<string, object>>(
                    "RecipeCategory",
                    j => j.HasOne<Category>().WithMany().HasForeignKey("CategoryId"),
                    j => j.HasOne<Recipe>().WithMany().HasForeignKey("RecipeId"),
                    j => 
                    {
                        j.ToTable("RecipeCategory");
                        j.HasKey("RecipeId", "CategoryId");
                    }
                );

            base.OnModelCreating(modelBuilder);
        }

        // Factory
        public static RecipeContext ContextFactory([CallerMemberName] string memberName = "")
        {
            var context = new RecipeContext();
             context.CallingMethod = memberName;
            
            // Configure these settings using the new configuration API
            //context.Configuration.AutoDetectChangesEnabled = false;
            //context.Configuration.UseDatabaseNullSemantics = true;

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
            return await Set<Recipe>().FromSqlInterpolated($"sRecipeSearch {@searchText}").ToListAsync();
        }

        /// <summary>
        /// Name of the method that created the request.
        /// Used for logging purposes.
        /// </summary>
        /// <returns></returns>
        public string CallingMethod { get; private set; }
    }
}
