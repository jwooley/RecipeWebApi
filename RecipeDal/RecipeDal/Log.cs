using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeDal
{
    public class Log
    {
        public int Id { get; set; }
        public string LogText { get; set; }
        public DateTime EventTime { get; set; } = DateTime.Now;
    }
}
