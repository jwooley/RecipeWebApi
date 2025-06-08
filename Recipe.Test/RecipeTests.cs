using Microsoft.EntityFrameworkCore;
using Xunit;
using RecipeDal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recipe.Test;

public class RecipeTests
{
    private readonly RecipeContext dc;
    private readonly ITestOutputHelper Trace;

    public RecipeTests(ITestOutputHelper testOutputHelper)
    {
        Trace = testOutputHelper;
        dc = RecipeContext.ContextFactory();
    }

    [Fact]
    public void Baseline()
    {
        // Force execution before tests to remove first use penalty.
        var recipe = dc.Recipes.First();
        Trace.WriteLine(recipe.Title);
    }

    //[Fact]
    public void Recipe_BadCodePerformsPoorly()
    {
        foreach (var recipe in dc.Recipes.Where(r => r.Title.Contains("Brownie")))
        {
            Trace.WriteLine(recipe.Title);

            if (recipe.Categories.Any())
            {
                Trace.WriteLine($"    Category: " + recipe.Categories.First().Description);
            }

            if (recipe.Ingredients.Count > 0)
            {
                foreach (var ingredient in recipe.Ingredients.OrderBy(i => i.SortOrder))
                {
                    Trace.WriteLine(dc.Ingredients.SingleOrDefault(i => i.IngredientId == ingredient.IngredientId).Units);
                    Trace.WriteLine($" {dc.Ingredients.SingleOrDefault(i => i.IngredientId == ingredient.IngredientId).UnitType} ");
                    Trace.WriteLine(dc.Ingredients.SingleOrDefault(i => i.IngredientId == ingredient.IngredientId).Description);
                }
            }

            foreach (var directionLine in recipe.Directions.OrderBy(d => d.LineNumber))
            {
                Trace.WriteLine(directionLine.Description);
            }
        }
    }

    [Fact]
    public void Recipe_EagerLoading()
    {
        var brownies = from r in dc.Recipes
                            .Include(r => r.Categories)
                            .Include(r => r.Ingredients)
                            .Include(r => r.Directions)
                       where r.Title.Contains("brownie")
                       select r;

        foreach (var recipe in brownies.ToList())
        {
            Trace.WriteLine(recipe.Title);
            Trace.WriteLine($"    Category: " + recipe.Categories.FirstOrDefault()?.Description);

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

    [Fact]
    public void Recipe_Projections()
    {
        var brownies = from r in dc.Recipes
                       where r.Title.Contains("Brownie")
                       select new
                       {
                           r.Title,
                           Category = r.Categories.FirstOrDefault().Description,
                           Ingredients = r.Ingredients.OrderBy(i => i.SortOrder).ToList(),
                           Directions = r.Directions.OrderBy(d => d.LineNumber).Select(d => d.Description).ToList()
                       };

        foreach (var recipe in brownies.ToList())
        {
            Trace.WriteLine(recipe.Title);
            Trace.WriteLine($"    Category: " + recipe.Category);

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
    [Fact]
    public async Task Recipe_AsyncLoad()
    {
        var brownies = await GetRecipesAsync();

        foreach (var recipe in brownies)
        {
            Trace.WriteLine(recipe.Title);
            Trace.WriteLine($"    Category: " + recipe.Category);

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
                              select new RecipeDto { Title = r.Title, Id = r.Id })
                       .ToListAsync();

        await Task.WhenAll(SetIngredientsAsync(brownies), SetDirectionsAsync(brownies));

        return brownies;
    }    
    public async Task SetIngredientsAsync(IEnumerable<RecipeDto> recipes)
    {
        using var context = RecipeContext.ContextFactory();
        var ingredients = await (from r in dc.Recipes
                                 where r.Title.Contains("Brownie")
                                 from i in r.Ingredients
                                 orderby i.SortOrder
                                 select i).ToListAsync();

        foreach (var i in ingredients)
        {
            var recipe = recipes.First(r => r.Id == i.RecipeId);
            recipe.Ingredients.Add(i);
        }
    }
    
    public async Task SetDirectionsAsync(IEnumerable<RecipeDto> recipes)
    {
        using var context = RecipeContext.ContextFactory();
        var directions = await (from r in context.Recipes
                                where r.Title.Contains("Brownie")
                                from d in r.Directions
                                orderby d.LineNumber
                                select d).ToListAsync();
        foreach (var d in directions)
        {
            var recipe = recipes.First(r => r.Id == d.RecipeId);
            recipe.Directions.Add(d.Description);
        }
    }

    [Fact]
    public void FirstSingle()
    {
        var recipeId = dc.Recipes.First().Id;

        var recipe1 = dc.Recipes.First(r => r.Id == recipeId);
        var recipe2 = dc.Recipes.FirstOrDefault(r => r.Id == recipeId);
        var recipe3 = dc.Recipes.Single(r => r.Id == recipeId);
        var recipe4 = dc.Recipes.SingleOrDefault(r => r.Id == recipeId);

        var recipe5 = dc.Recipes.Where(r => r.Id == recipeId).First();

        Trace.WriteLine("Fetch from cache");
        var recipeCached = dc.Recipes.Find(recipeId);
        Assert.NotNull(recipeCached);
        Trace.WriteLine("Fetch from Local");
        var cachedAgain = dc.Recipes.Local.FirstOrDefault(r => r.Id == recipeId);
        Assert.NotNull(cachedAgain);
    }

    [Fact]
    public void Recipe_LocalFetchesObjectsNotInDatabase()
    {
        var fakeRecipe = new RecipeDal.Recipe { Id = -9999, Title = "NotInDatabase" };
        dc.Recipes.Add(fakeRecipe);
        var found = dc.Recipes.Local.First(r => r.Title == "NotInDatabase");
        Assert.Equal(-9999, found.Id);
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

    [Fact]
    public void Recipe_ConditionalSearchByTitleCategory()
    {
        var conditions = new RecipeConditions { Title = "Dip", Category = "Appetizers" };
        var recipes = GetConditional(conditions)
            .Take(10);
        Assert.Equal(10, recipes.Count());
    }    
    [Fact]
    public void Recipe_ConditionalSearcyByIngredientServings()
    {
        var conditions = new RecipeConditions { IngredientName = "Chocolate", Servings = 8 };
        var recipes = GetConditional(conditions)
            .Take(10);
        Assert.Equal(10, recipes.Count());
    }
    [Fact]
    public void Recipe_ConditionalSearchByTitle()
    {
        var conditions = new RecipeConditions { Title = "Brownie" };
        var recipes = GetConditional(conditions)
            .Take(10);
        Assert.Equal(10, recipes.Count());
    }
    
    
    [Fact]
    public void Recipe_Paging()
    {
        // Orderby is required by EF. WebAPI orders by all columns if not specified.
        var recipes = dc.Recipes.OrderBy(r => r.Id).Skip(10).Take(10);
        Assert.Equal(10, recipes.Count());
    }

    [Fact]
    public void Recipe_ContainsList()
    {
        string[] targetCategories = { "Cheese/eggs", "Chocolate", "Children" };
        var recipes = (from recipe in dc.Recipes
                       from category in recipe.Categories
                       where targetCategories.Contains(category.Description)
                       select recipe).ToList();

        Assert.True(recipes.Any());
    }

    [Fact]
    public void Recipe_ComposingSets()
    {
        var targetCategories = dc.Categories.Where(c => c.Description.StartsWith("ch"));

        var nutsRecipes = dc.Ingredients.Where(i => i.Description.Contains("nut")).Select(i => i.Recipe);

        var results = from recipe in nutsRecipes
                      from category in recipe.Categories
                      join tCat in targetCategories on category.CategoryId equals tCat.CategoryId
                      select recipe;
        Assert.Equal(10, results.Take(10).Count());
    }

    [Fact]
    public void Recipe_ManyToOnePoor()
    {
        var sw = new System.Diagnostics.Stopwatch();
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

    [Fact]
    public async Task Recipe_SearchWithStoredProc()
    {
        var chocolates = await dc.SearchRecipeAsync("Chocolate");
        Assert.True(chocolates.Any());
    }
}
