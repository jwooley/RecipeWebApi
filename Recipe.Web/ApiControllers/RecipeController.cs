using Recipe.Utils;
using Recipe.Web.Models;
using RecipeDal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Recipe.Web.ApiControllers
{
    public class RecipeController : ApiController
    {

        // GET api/recipe
        [HttpGet]
        [Route("api/recipe/search")]
        public IEnumerable<RecipeSearchResult> GetRecipes(string searchstring)
        {
            var context = RecipeContext.ContextFactory();
            var query = context.Recipes
                .Where(r => r.Title.Contains(searchstring) ||
                    r.Ingredients.Any(i => i.Description.Contains(searchstring)))
                .Select(r => new RecipeSearchResult{ Id = r.Id, Title = r.Title })
                .ToList();
            return query;
        }

        [HttpGet]
        [Route("api/recipe/{id}")]
        public RecipeResultModel GetRecipe(long id)
        {
            var dc = RecipeContext.ContextFactory();
            var recipe = dc.Recipes
                .Where(r => r.Id == id)
                .Select(r => new
                {
                    r.Title,
                    r.ServingMeasure,
                    r.ServingQuantity,
                    Categories = r.Categories.Select(c => c.Description),
                    Directions = r.Directions.OrderBy(d => d.LineNumber).Select(d => d.Description),
                    Ingredients = r.Ingredients.Select(i => new IngredientResultModel
                    {
                        Description = i.Description,
                        UnitType = i.UnitType,
                        Units = i.Units
                    })
                })
                .FirstOrDefault();
            if (recipe == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            foreach (var ingredient in recipe.Ingredients)
            {
                ingredient.Units = ingredient.Units.ToFractionString();
            }
            return new RecipeResultModel
            {
                Title = recipe.Title,
                ServingMeasure = recipe.ServingMeasure,
                ServingQuantity = recipe.ServingQuantity,
                Categories = string.Join(", ", recipe.Categories.Distinct().ToArray()),
                Directions = string.Join("<br />", recipe.Directions),
                Ingredients = recipe.Ingredients.ToArray() ?? Array.Empty<IngredientResultModel>()
            };
        }
    }
}
