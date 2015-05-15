using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using JsParser.Core.Helpers;
using JsParser.Core.Parsers;

namespace JsParser.Core.Code
{
    /// <summary>
    /// The comparer for code node.
    /// </summary>
    public class CodeNodeComparer : Comparer<CodeNode>
    {
        public override int Compare(CodeNode x, CodeNode y)
        {
            if (x == null || y == null)
            {
                return -1;
            }

            if (x.StartLine == y.StartLine
                && x.EndLine == y.EndLine
                && x.Alias == y.Alias
                && x.Comment == y.Comment
                && x.NodeType == y.NodeType)
            {
                return 0;
            }

            return -1;
        }
    }
}