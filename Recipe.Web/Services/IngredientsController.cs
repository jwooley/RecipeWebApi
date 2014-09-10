using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using RecipeDal;
using System.Web.Http.OData;

namespace Recipe.Web.Services
{
    /// <summary>
    /// This is an accessor for the Ingredients
    /// </summary>
    public class IngredientsController : ApiController
    {
        private RecipeContext db = new RecipeContext();

        [EnableQuery(MaxTop = 50, PageSize = 20)]
        /// <summary>
        /// This is how we get the list of ingredients
        /// </summary>
        /// <returns></returns>
        // GET: api/Ingredients
        public IQueryable<Ingredient> GetIngredients()
        {
            return db.Ingredients;
        }

        // GET: api/Ingredients/5
        [ResponseType(typeof(Ingredient))]
        public async Task<IHttpActionResult> GetIngredient(long id)
        {
            Ingredient ingredient = await db.Ingredients.FindAsync(id);
            if (ingredient == null)
            {
                return NotFound();
            }

            return Ok(ingredient);
        }

        // PUT: api/Ingredients/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutIngredient(long id, Ingredient ingredient)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != ingredient.IngredientId)
            {
                return BadRequest();
            }

            db.Entry(ingredient).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IngredientExists(id))
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

        // POST: api/Ingredients
        [ResponseType(typeof(Ingredient))]
        public async Task<IHttpActionResult> PostIngredient(Ingredient ingredient)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Ingredients.Add(ingredient);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = ingredient.IngredientId }, ingredient);
        }

        // DELETE: api/Ingredients/5
        [ResponseType(typeof(Ingredient))]
        public async Task<IHttpActionResult> DeleteIngredient(long id)
        {
            Ingredient ingredient = await db.Ingredients.FindAsync(id);
            if (ingredient == null)
            {
                return NotFound();
            }

            db.Ingredients.Remove(ingredient);
            await db.SaveChangesAsync();

            return Ok(ingredient);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool IngredientExists(long id)
        {
            return db.Ingredients.Count(e => e.IngredientId == id) > 0;
        }
    }
}