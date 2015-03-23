using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using RecipeDal;
using Recipe.Web.Services.DTO;
using System.Threading.Tasks;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;

namespace Recipe.Web.Services
{
    public class RecipesController : ApiController
    {
        private RecipeContext db = RecipeContext.ContextFactory();

        // GET: api/Recipes
        public IQueryable<RecipeDal.Recipe> GetRecipes()
        {
            return db.Recipes;
        }

        // GET: api/Recipes/5
        [ResponseType(typeof(RecipeDal.Recipe))]
        public IHttpActionResult GetRecipe(long id)
        {
            var recipe = db.Recipes
                .Include(r => r.Directions)
                .Include(r => r.Ingredients)
                .FirstOrDefault(r => r.RecipeId == id);
            if (recipe == null)
            {
                return NotFound();
            }

            return Ok(recipe);
        }

        // PUT: api/Recipes/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutRecipe(long id, RecipeDal.Recipe recipe)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != recipe.RecipeId)
            {
                return BadRequest();
            }

            db.Entry(recipe).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RecipeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Recipes
        [ResponseType(typeof(RecipeDal.Recipe))]
        public IHttpActionResult PostRecipe(RecipeDal.Recipe recipe)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Recipes.Add(recipe);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = recipe.RecipeId }, recipe);
        }

        // DELETE: api/Recipes/5
        [ResponseType(typeof(RecipeDal.Recipe))]
        public IHttpActionResult DeleteRecipe(long id)
        {
            RecipeDal.Recipe recipe = db.Recipes.Find(id);
            if (recipe == null)
            {
                return NotFound();
            }

            db.Recipes.Remove(recipe);
            db.SaveChanges();

            return Ok(recipe);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool RecipeExists(long id)
        {
            return db.Recipes.Count(e => e.RecipeId == id) > 0;
        }

        [HttpGet]
        public PageResult GetPagedRecipes(ODataQueryOptions<RecipeDal.Recipe> options)
        {
            return db.Recipes.ToPageResult(options, base.Request);
        }


        /// <summary>
        /// This is an example of a custom attribute controller. This fetches the child
        /// records from a parent Id
        /// </summary>
        /// <param name="recipeId">Recipe ID. Notice the naming does not agree with
        /// the standard or controller/action routes.</param>
        /// <returns></returns>
        [Route("api/Recipe/{recipeId}/Ingredients")]
        public IEnumerable<Ingredient> GetRecipeIngredients(long recipeId)
        {
            var results = from r in db.Recipes
                          where r.RecipeId == recipeId
                          from i in r.Ingredients
                          select i;
            
            return results;
        }

        /// <summary>
        /// Gets a recipe with the associated child collections populated.
        /// This eagerly loads the children but avoids the circular reference
        /// that EF natively exposes and the JSON serializer throws an error on.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IQueryable<RecipeDto> RecipeDtos()
        {
            var recipes = db.Recipes
                .Select(r => new RecipeDto
            {
                Id = r.RecipeId,
                Title = r.Title,
                Servings = r.ServingQuantity,
                ServingMeasure = r.ServingMeasure,
                Ingredients = r.Ingredients.OrderBy(i => i.SortOrder).Select(i => new IngredientDto { Decription = i.Description, Amount = i.Units, AmountType = i.UnitType }),
                Directions = r.Directions.OrderBy(d => d.LineNumber).Select(d => d.Description),
                Categories = r.Categories.Select(c => c.Description)
            });

            return recipes;
        }

        /// <summary>
        /// Gets a recipe based on the supplied Id
        /// </summary>
        /// <param name="id">Recipe Id</param>
        /// <returns>Single recipe including the directions, ingredients, and categories</returns>
        [HttpGet]
        public RecipeDto RecipeById(long id)
        {
            return RecipeDtos().FirstOrDefault(r => r.Id == id);
        }

        /// <summary>
        /// Asyncronously gets a recipe based on the supplied Id
        /// </summary>
        /// <param name="id">Recipe Id</param>
        /// <returns>Gets the recipe and asyncronously loads the child objects</returns>
        [HttpGet]
        public async Task<RecipeDto> RecipeByIdAsync(long id)
        {
            var recipe = await db.Recipes
                .Where(r => r.RecipeId == id)
                .Select(r => new RecipeDto
                {
                    Id = r.RecipeId,
                    Title = r.Title,
                    Servings = r.ServingQuantity,
                    ServingMeasure = r.ServingMeasure,
                }).FirstOrDefaultAsync();

            await Task.WhenAll(
                    SetIngredients(recipe),
                    SetDirections(recipe),
                    SetCategories(recipe)
                );

            return recipe;

        }
        private async Task SetIngredients(RecipeDto recipe)
        {
            using (var dc = RecipeContext.ContextFactory())
            {
                recipe.Ingredients = await 
                    (from r in dc.Recipes
                     where r.RecipeId == recipe.Id
                     from i in r.Ingredients
                     orderby i.SortOrder
                     select new IngredientDto { Decription = i.Description, Amount = i.Units, AmountType = i.UnitType })
                    .ToListAsync();
            }  
        }
        private async Task SetDirections(RecipeDto recipe)
        {
            using (var dc = RecipeContext.ContextFactory())
            {
                recipe.Directions = await dc.Directions
                .Where(d => d.Recipe.RecipeId == recipe.Id)
                .OrderBy(d => d.LineNumber)
                .Select(d => d.Description)
                .ToListAsync();
            }
        }
        private async Task SetCategories(RecipeDto recipe)
        {
            using (var dc = RecipeContext.ContextFactory())
            {
                recipe.Categories = await dc.Categories
                .Where(c => c.Recipes.Any(r => r.RecipeId == recipe.Id))
                .Select(c => c.Description)
                .ToListAsync();
            }
        }
    }
}