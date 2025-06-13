using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Query;
using RecipeDal;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;

namespace Recipe.Web.Services
{
    public class DirectionsController : ApiController
    {
        private RecipeContext db = RecipeContext.ContextFactory();

        // GET: api/Directions
        public IQueryable<Direction> GetDirections()
        {
            return db.Directions;
        }

        // GET: api/Directions/5
        [ResponseType(typeof(Direction))]
        public IHttpActionResult GetDirection(long id)
        {
            Direction direction = db.Directions.Find(id);
            if (direction == null)
            {
                return NotFound();
            }

            return Ok(direction);
        }

        // PUT: api/Directions/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutDirection(long id, Direction direction)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != direction.DirectionId)
            {
                return BadRequest();
            }

            db.Entry(direction).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DirectionExists(id))
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

        // POST: api/Directions
        [ResponseType(typeof(Direction))]
        public IHttpActionResult PostDirection(Direction direction)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Directions.Add(direction);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = direction.DirectionId }, direction);
        }

        // DELETE: api/Directions/5
        [ResponseType(typeof(Direction))]
        public IHttpActionResult DeleteDirection(long id)
        {
            Direction direction = db.Directions.Find(id);
            if (direction == null)
            {
                return NotFound();
            }

            db.Directions.Remove(direction);
            db.SaveChanges();

            return Ok(direction);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DirectionExists(long id)
        {
            return db.Directions.Count(e => e.DirectionId == id) > 0;
        }

        [HttpGet]
        public IQueryable<Direction> GetDirectionsForRecipe(long id)
        {
            return db.Directions
                .Where(d => d.Recipe.Id == id)
                .OrderBy(d => d.LineNumber);
        }

        [HttpGet]
        public PageResult GetPagedDirections(ODataQueryOptions<Direction> options)
        {
            return db.Directions.ToPageResult(options, base.Request);
        }


    }
}