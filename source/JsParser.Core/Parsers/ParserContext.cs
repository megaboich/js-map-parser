using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JsParser.Core.Code;
using JsParser.Core.Helpers;

namespace JsParser.Core.Parsers
{
    public class ParserContext
    {
        public ParserContext(ParserContext context, bool copyNames = false)
            :this(context.Nodes)
        {
            if (copyNames)
            {
                NameStack.AddRange(context.NameStack);
            }
        }

        public ParserContext(Hierarchy<CodeNode> nodes)
        {
            Nodes = nodes;
            NameStack = new List<string>();
        }

        public Hierarchy<CodeNode> Nodes { get; set; }

        public List<string> NameStack { get; set; }

        public string GetNameFromStack()
        {
            if (NameStack != null && NameStack.Count > 0)
            {
                if (NameStack.Count == 1)
                {
                    return NameStack[0];
                }

                var sb = new StringBuilder();
                for (var i = NameStack.Count - 1; i > 0; i--)
                {
                    sb.Append(NameStack[i]);
                    if (NameStack[i - 1] == "?")
                    {
                        sb.Append(">");
                    }
                    else
                    {
                        sb.Append(".");
                    }
                }

                sb.Append(NameStack[0]);
                return sb.ToString();
            }
            else
            {
                return "?";
            }
        }
    }
}
