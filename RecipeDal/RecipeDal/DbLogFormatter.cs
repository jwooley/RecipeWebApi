using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Interception;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeDal
{
    public class DbLogFormatter : DatabaseLogFormatter
    {

        public DbLogFormatter(DbContext context, Action<string> writeAction) 
            : base( context, writeAction) {}

        public override void LogCommand<TResult>(DbCommand command, DbCommandInterceptionContext<TResult> interceptionContext)
        {
            //base.LogCommand<TResult>(command, interceptionContext);
        }

        public override void LogResult<TResult>(DbCommand command, DbCommandInterceptionContext<TResult> interceptionContext)
        {
            var context = interceptionContext.DbContexts.OfType<RecipeContext>().FirstOrDefault();
            if (context != null)
            {
                Trace.WriteLine(context.CallingMethod + " Completed in " + GetStopwatch(interceptionContext).ElapsedMilliseconds.ToString());
            }
            base.LogResult<TResult>(command, interceptionContext);
        }
    }

    public class LogConfiguration //: DbConfiguration
    {
        public LogConfiguration()
        {
            //SetDatabaseLogFormatter((context, action) => new DbLogFormatter(context, action));
        }
    }
}
