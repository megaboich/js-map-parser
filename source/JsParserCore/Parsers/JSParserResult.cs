using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JsParserCore.Helpers;
using JsParserCore.Code;

namespace JsParserCore.Parsers
{
    public class JSParserResult
    {
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
