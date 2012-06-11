using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace JsParserCore.Helpers
{
    public static class ErrorHandler
    {
        public static void WriteExceptionDetailsToTrace(string subj, Exception ex)
        {
            Trace.TraceError("{0}\r\nMessage: {1}\r\nSource: {2}\r\nStack Trace: {3}", subj, ex.Message, ex.Source, ex.StackTrace);
            if (ex.InnerException != null)
            {
                WriteExceptionDetailsToTrace("Inner exception:", ex.InnerException);
            }
        }
    }
}
