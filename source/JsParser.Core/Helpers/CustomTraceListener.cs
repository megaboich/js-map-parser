using System;
using System.Diagnostics;
using System.IO;

namespace JsParser.Core.Helpers
{
    public class CustomTraceListener: TraceListener
    {
        private static object _lock = new object();

        public override void Write(string message)
        {
            lock (_lock)
            {
                var logFilePath = "c:\\jsparser.log";
                using (var f = File.AppendText(logFilePath))
                {
                    f.Write(string.Format("{0}:{1}", DateTime.Now, message));
                }
            }
        }

        public override void WriteLine(string message)
        {
            Write(message + Environment.NewLine);
        }
    }
}
