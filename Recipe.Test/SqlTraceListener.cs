using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecipeDal;

namespace Recipe.Test
{
    public class SqlTraceListener : TraceListener
    {
        private RecipeContext context = new RecipeContext();

        protected override void Dispose(bool disposing)
        {
            context.Dispose();
            base.Dispose(disposing);
        }

        public override void Write(string message)
        {
            context.Logs.Add(new Log
            {
                LogText = message
            });
            context.SaveChanges();
        }

        public override void WriteLine(string message)
        {
            Write(message);
        }
    }
}
