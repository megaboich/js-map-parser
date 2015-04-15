using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JsParser.Core.Parsers
{
    public interface ICommentWrapper
    {
        string Spelling { get; }
        int StartLine { get; }
        int EndLine { get; }
    }

    public class MsJsParserCommentWrapper:ICommentWrapper
    {
        private Microsoft.JScript.Compiler.Comment _orig;
        public MsJsParserCommentWrapper(Microsoft.JScript.Compiler.Comment orig)
        {
            _orig = orig;
        }

        public string Spelling
        {
            get { return _orig.Spelling; }
        }

        public int StartLine
        {
            get { return _orig.Location.StartLine; }
        }

        public int EndLine
        {
            get { return _orig.Location.EndLine; }
        }
    }

    public class JintCommentWrapper : ICommentWrapper
    {
        private Jint.Parser.Comment _orig;

        public JintCommentWrapper(Jint.Parser.Comment orig)
        {
            _orig = orig;
        }

        public string Spelling
        {
            get { return _orig.Value; }
        }

        public int StartLine
        {
            get { return _orig.Location.Start.Line; }
        }

        public int EndLine
        {
            get { return _orig.Location.End.Line; }
        }
    }
}
