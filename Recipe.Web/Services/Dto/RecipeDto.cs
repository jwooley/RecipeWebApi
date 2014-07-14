using System.Collections.Generic;

namespace Recipe.Web.Services.DTO
{
    public class RecipeDto
    {
        public IEnumerable<string> Categories { get; internal set; }
        public IEnumerable<string> Directions { get; internal set; }
        public long Id { get; internal set; }
        public object Ingredients { get; internal set; }
        public string ServingMeasure { get; internal set; }
        public decimal? Servings { get; internal set; }
        public string Title { get; internal set; }
    }
}