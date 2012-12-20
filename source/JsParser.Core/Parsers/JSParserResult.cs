using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JsParser.Core.Helpers;
using JsParser.Core.Code;

namespace JsParser.Core.Parsers
{
    public class JSParserResult
    {
        public string FileName { get; set; }

        public Hierachy<CodeNode> Nodes { get; set; }

        public List<ErrorMessage> Errors { get; set; }

        public List<ErrorMessage> InternalErrors { get; set; }

        public List<TaskListItem> TaskList { get; set; }
    }

    public class ErrorMessage
    {
        public string Message { get; set; }

        public int StartLine { get; set; }

        public int StartColumn { get; set; }
    }
}
