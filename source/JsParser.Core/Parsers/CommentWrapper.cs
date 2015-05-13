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

        public CommentWrapper(Jint.Parser.Comment orig)
        {
            Spelling = orig.Value;
            StartLine = orig.Location.Start.Line;
            EndLine = orig.Location.End.Line;
        }
    }
}
