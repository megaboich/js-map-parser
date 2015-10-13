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

        public Hierarchy<CodeNode> Nodes { get; set; }

        public List<ErrorMessage> Errors { get; set; }

        public List<ErrorMessage> InternalErrors { get; set; }

        public List<TaskListItem> TaskList { get; set; }

        public JSParserResult()
        {
            Nodes = new Hierarchy<CodeNode>(new CodeNode(){Alias = "All"});
            Errors = new List<ErrorMessage>();
            InternalErrors = new List<ErrorMessage>();
            TaskList = new List<TaskListItem>();
        }

        /// <summary>
        /// Get the IsEmpty flag. If so then result is empty - probably service returned empty result when there is not need to update previous.
        /// </summary>
        public bool IsEmpty
        {
            get { return string.IsNullOrEmpty(FileName); }
        }
    }

    public class ErrorMessage
    {
        public string Message { get; set; }

        public int StartLine { get; set; }

        public int StartColumn { get; set; }
    }
}
