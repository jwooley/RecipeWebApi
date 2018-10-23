using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RecipeDal;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Recipe.Test
{
    [TestClass]
    public class RecipeTests
    {
        RecipeContext dc;

        [TestInitialize]
        public void Init()
        {
            Database.SetInitializer<RecipeContext>(null);
            dc = RecipeContext.ContextFactory();
        }
        [TestCleanup]
        public void Teardown()
        {
            dc.Dispose();
        }
        [TestMethod]
        public void Baseline()
        {
            // Force execution before tests to remove first use penalty.
            var recipe = dc.Recipes.First();
            Trace.WriteLine(recipe.Title);
        }
        [TestMethod]
        public void Recipe_BadCodePerformsPoorly()
        {
            var Appetizers = from cat in dc.Categories
                             where cat.Description == "Appetizers"
                             select cat;
            foreach (var category in Appetizers)
            {
                foreach (var recipe in category.Recipes)
                {
                    Trace.WriteLine(recipe.Title);
                    Trace.Write($"    Category: " + category.Description);

                    foreach (var ingredient in recipe.Ingredients.OrderBy(i => i.SortOrder))
                    {
                        Trace.Write(ingredient.Units);
                        Trace.Write($" {ingredient.UnitType} ");
                        Trace.WriteLine(ingredient.Units);
                    }

                    foreach (var directionLine in recipe.Directions.OrderBy(d => d.LineNumber))
                    {
                        Trace.WriteLine(directionLine.Description);
                    }
                }
            }
        }

        [TestMethod]
        public void Recipe_EagerLoading()
        {
            var brownies = from r in dc.Recipes
                                .Include("Categories")
                                .Include("Ingredients")
                                .Include("Directions")
                           where r.Title.Contains("brownie")
                           select r;

            foreach (var recipe in brownies.ToList())
            {
                Trace.WriteLine(recipe.Title);
                Trace.Write($"    Category: " + recipe.Categories.FirstOrDefault()?.Description);

                foreach (var ingredient in recipe.Ingredients.OrderBy(i => i.SortOrder))
                {
                    Trace.WriteLine($"{ingredient.Units} {ingredient.UnitType}: {ingredient.Description}");
                }

                foreach (var directionLine in recipe.Directions.OrderBy(d => d.LineNumber))
                {
                    Trace.WriteLine(directionLine.Description);
                }
            }
        }

        [TestMethod]
        public void Recipe_Projections()
        {
            var brownies = from r in dc.Recipes
                           where r.Title.Contains("Brownie")
                           select new
                           {
                               r.Title,
                               Category = r.Categories.FirstOrDefault().Description,
                               Ingredients = r.Ingredients.OrderBy(i => i.SortOrder),
                               Directions = r.Directions.OrderBy(d => d.LineNumber).Select(d => d.Description)
                           };

            foreach (var recipe in brownies.ToList())
            {
                Trace.WriteLine(recipe.Title);
                Trace.Write($"    Category: " + recipe.Category);

                foreach (var ingredient in recipe.Ingredients)
                {
                    Trace.WriteLine($"{ingredient.Units} {ingredient.UnitType}: {ingredient.Description}");
                }

                foreach (var directionLine in recipe.Directions)
                {
                    Trace.WriteLine(directionLine);
                }
            }
        }
        [TestMethod]
        public async Task Recipe_AsyncLoad()
        {
            var brownies = await GetRecipesAsync();

            foreach (var recipe in brownies)
            {
                Trace.WriteLine(recipe.Title);
                Trace.Write($"    Category: " + recipe.Category);

                foreach (var ingredient in recipe.Ingredients)
                {
                    Trace.WriteLine($"{ingredient.Units} {ingredient.UnitType}: {ingredient.Description}");
                }

                foreach (var directionLine in recipe.Directions)
                {
                    Trace.WriteLine(directionLine);
                }
            }
        }

        public async Task<IEnumerable<RecipeDto>> GetRecipesAsync()
        {
            var brownies = await (from r in dc.Recipes
                                  where r.Title.Contains("Brownie")
                                  select new RecipeDto { Title = r.Title, Id = r.RecipeId })
                           .ToListAsync();

            await Task.WhenAll(SetIngredientsAsync(brownies), SetDirectionsAsync(brownies));

            return brownies;
        }
        public async Task SetIngredientsAsync(IEnumerable<RecipeDto> recipes)
        {
            using (var dc1 = RecipeContext.ContextFactory())
            {
                var ingredients = await (from r in dc.Recipes
                                         where r.Title.Contains("Brownie")
                                         from i in r.Ingredients
                                         orderby i.SortOrder
                                         select i).ToListAsync();

                foreach (var i in ingredients)
                {
                    var recipe = recipes.First(r => r.Id == i.Recipe_RecipeId);
                    recipe.Ingredients.Add(i);
                }
            }
        }
        public async Task SetDirectionsAsync(IEnumerable<RecipeDto> recipes)
        {
            using (var dc1 = RecipeContext.ContextFactory())
            {
                var directions = await (from r in dc1.Recipes
                                        where r.Title.Contains("Brownie")
                                        from d in r.Directions
                                        orderby d.LineNumber
                                        select d).ToListAsync();
                foreach (var d in directions)
                {
                    var recipe = recipes.First(r => r.Id == d.Recipe_RecipeId);
                    recipe.Directions.Add(d.Description);
                }
            }
        }

        [TestMethod]
        public void FirstSingle()
        {
            var recipeId = dc.Recipes.First().RecipeId;

            var recipe1 = dc.Recipes.First(r => r.RecipeId == recipeId);
            var recipe2 = dc.Recipes.FirstOrDefault(r => r.RecipeId == recipeId);
            var recipe3 = dc.Recipes.Single(r => r.RecipeId == recipeId);
            var recipe4 = dc.Recipes.SingleOrDefault(r => r.RecipeId == recipeId);

            var recipe5 = dc.Recipes.Where(r => r.RecipeId == recipeId).First();

            Trace.WriteLine("Fetch from cache");
            var recipeCached = dc.Recipes.Find(recipeId);
            Assert.IsNotNull(recipeCached);
            Trace.WriteLine("Fetch from Local");
            var cachedAgain = dc.Recipes.Local.FirstOrDefault(r => r.RecipeId == recipeId);
            Assert.IsNotNull(cachedAgain);
        }

        [TestMethod]
        public void Recipe_LocalFetchesObjectsNotInDatabase()
        {
            var fakeRecipe = new RecipeDal.Recipe { RecipeId = -9999, Title = "NotInDatabase" };
            dc.Recipes.Add(fakeRecipe);
            var found = dc.Recipes.Local.First(r => r.Title == "NotInDatabase");
            Assert.AreEqual(-9999, found.RecipeId);
        }

        private IQueryable<RecipeDal.Recipe> GetConditional(RecipeConditions conditions)
        {
            IQueryable<RecipeDal.Recipe> recipes = dc.Recipes;

            if (conditions.Servings > 0)
            {
                recipes = recipes.Where(r => r.ServingQuantity == conditions.Servings);
            }

            if (!String.IsNullOrEmpty(conditions.IngredientName))
            {
                recipes = recipes.Where(r => r.Ingredients.Any(i => i.Description.Contains(conditions.IngredientName)));
            }

            if (!String.IsNullOrEmpty(conditions.Category))
            {
                recipes = recipes.Where(r => r.Categories.Any(c => c.Description == conditions.Category));
            }

            if (!string.IsNullOrEmpty(conditions.Title))
            {
                recipes = recipes.Where(r => r.Title.Contains(conditions.Title));
            }

            return recipes;
        }

        [TestMethod]
        public void Recipe_ConditionalSearchByTitleCategory()
        {
            var conditions = new RecipeConditions { Title = "Dip", Category = "Appetizers" };
            var recipes = GetConditional(conditions)
                .Take(10);
            Assert.AreEqual(10, recipes.Count());
        }
        [TestMethod]
        public void Recipe_ConditionalSearcyByIngredientServings()
        {
            var conditions = new RecipeConditions { IngredientName = "Chocolate", Servings = 8 };
            var recipes = GetConditional(conditions)
                .Take(10);
            Assert.AreEqual(10, recipes.Count());
        }
        [TestMethod]
        public void Recipe_ConditionalSearchByTitle()
        {
            var conditions = new RecipeConditions { Title = "Brownie" };
            var recipes = GetConditional(conditions)
                .Take(10);
            Assert.AreEqual(10, recipes.Count());
        }


        [TestMethod]
        public void Recipe_Paging()
        {
            // Orderby is required by EF. WebAPI orders by all columns if not specified.
            var recipes = dc.Recipes.OrderBy(r => r.RecipeId).Skip(10).Take(10);
            Assert.AreEqual(10, recipes.Count());
        }

        [TestMethod]
        public void Recipe_ContainsList()
        {
            string[] targetCategories = { "Cheese/eggs", "Chocolate", "Children" };
            var recipes = (from recipe in dc.Recipes
                           from category in recipe.Categories
                           where targetCategories.Contains(category.Description)
                           select recipe).ToList();

            Assert.IsTrue(recipes.Any());
        }

        [TestMethod]
        public void Recipe_ComposingSets()
        {
            var targetCategories = dc.Categories.Where(c => c.Description.StartsWith("ch"));

            var nutsRecipes = dc.Ingredients.Where(i => i.Description.Contains("nut")).Select(i => i.Recipe);

            var results = from recipe in nutsRecipes
                          from category in recipe.Categories
                          join tCat in targetCategories on category.CategoryId equals tCat.CategoryId
                          select recipe;
            Assert.AreEqual(10, results.Take(10).Count());
        }

        [TestMethod]
        public void Recipe_ManyToOnePoor()
        {
            var sw = new Stopwatch();
            sw.Start();
            var chocolates = (from i in dc.Ingredients
                              where i.Description.StartsWith("Chocolate")
                              select new
                              {
                                  i.Description,
                                  i.Recipe.Title,
                                  i.Recipe.ServingQuantity,
                                  i.Recipe.ServingMeasure
                              }).ToList();
            sw.Stop();
            Trace.WriteLine($"Fetched {chocolates.Count()} rows in {sw.ElapsedTicks} ticks");

            sw.Restart();
            var better = (from i in dc.Ingredients
                          where i.Description.StartsWith("Chocolate")
                          let r = i.Recipe
                          select new
                          {
                              i.Description,
                              r.Title,
                              r.ServingQuantity,
                              r.ServingMeasure
                          }).ToList();
            sw.Stop();
            Trace.WriteLine($"Better fetched {better.Count()} rows in {sw.ElapsedTicks} ticks");

            sw.Restart();
            var inverted = (from r in dc.Recipes
                            from i in r.Ingredients
                            where i.Description.StartsWith("Chocolate")
                            select new
                            {
                                i.Description,
                                r.Title,
                                r.ServingQuantity,
                                r.ServingMeasure
                            }).ToList();
            sw.Stop();
            Trace.WriteLine($"Inverted fetched {better.Count()} rows in {sw.ElapsedTicks} ticks");
        }

        [TestMethod]
        public async Task Recipe_SearchWithStoredProc()
        {
            var chocolates = await dc.SearchRecipeAsync("Chocolate");
            Assert.IsTrue(chocolates.Any());
        }
    }
}