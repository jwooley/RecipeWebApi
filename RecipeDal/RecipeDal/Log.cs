using System;

namespace RecipeDal
{
    public class Log
    {
        public int Id { get; set; }
        public string LogText { get; set; }
        public DateTime EventTime { get; set; } = DateTime.Now;
    }
}
