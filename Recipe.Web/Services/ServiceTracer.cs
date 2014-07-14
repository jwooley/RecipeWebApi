using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Net.Http;
using System.Web;
using System.Web.Http.Tracing;

namespace Recipe.Web.Services
{
    public class ServiceTracer : ITraceWriter
    {
        List<TraceRecord> items = new List<TraceRecord>();

        public void Trace(HttpRequestMessage request,
            string category,
            System.Web.Http.Tracing.TraceLevel level,
            Action<TraceRecord> traceAction)
        {
            var record = new TraceRecord(request, category, level);
            traceAction(record);

            System.Diagnostics.Trace.WriteLine(string.Format("{0} - {1}  {2} - {3}", category, level, record.Kind, record.Operation));
            //WriteTrace(record);
        }

        private void WriteTrace(TraceRecord record)
        {
            if (record.Kind == TraceKind.Begin && record.Operation == "SelectAction")
            {
                items.Add(record);
            }
            if (record.Kind == TraceKind.End && record.Operation == "Dispose")
            {
                WriteServiceEnd(record);
            }
        }

        private void WriteServiceEnd(TraceRecord record)
        {
            var start = items.Find(r => r.RequestId == record.RequestId);
            if (record.Request != null && start != null)
            {
                System.Diagnostics.Trace.WriteLine(string.Format("{0} - {1}: elapsedTime={2}",
                    DateTime.Now,
                    record.Request.RequestUri.PathAndQuery,
                    (record.Timestamp - start.Timestamp).TotalMilliseconds));

                items.Remove(start);
            }
        }
    }
}