using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JsParser.Core.Parsers
{
    public class CommentWrapper
    {
        public string Spelling { get; set; }
        public int StartLine { get; set; }
        public int EndLine { get; set; }
        public bool Processed { get; set; }

        public CommentWrapper(Microsoft.JScript.Compiler.Comment orig)
        {
            Spelling = orig.Spelling;
            StartLine = orig.Location.StartLine;
            EndLine = orig.Location.EndLine;
        }

        public CommentWrapper(Jint.Parser.Comment orig)
        {
            Spelling = orig.Value;
            StartLine = orig.Location.Start.Line;
            EndLine = orig.Location.End.Line;
        }
    }
}
