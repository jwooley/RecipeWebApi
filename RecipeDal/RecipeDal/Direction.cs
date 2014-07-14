namespace RecipeDal
{
    public class Direction
    {
        // Properties
        public long DirectionId { get; set; }
        public long LineNumber { get; set; }
        public string Description { get; set; }

        // Parent
        public Recipe Recipe { get; set; }

    }
}