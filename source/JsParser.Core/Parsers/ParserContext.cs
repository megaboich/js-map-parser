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

        public ParserContext(Hierachy<CodeNode> nodes)
        {
            Nodes = nodes;
            NameStack = new List<string>();
        }

        public Hierachy<CodeNode> Nodes { get; set; }

        public List<string> NameStack { get; set; }
    }
}
