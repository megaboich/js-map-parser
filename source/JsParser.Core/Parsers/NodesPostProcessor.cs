namespace JsParser.Core.Parsers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using JsParser.Core.Helpers;
    using JsParser.Core.Code;

    /// <summary>
    /// Process nodes hierarchy after parsing to improve structure
    /// </summary>
    public static class NodesPostProcessor
    {
        public static bool HideAnonymousFunctions(Hierarchy<CodeNode> hierachy)
        {
            if (hierachy.HasChildren)
            {
                hierachy.Children.RemoveAll(child => HideAnonymousFunctions(child));
            }

            if (hierachy.HasChildren)
            {
                return false;
            }

            if (hierachy.Item.NodeType == CodeNodeType.AnonymousFunction)
            {
                return true;
            }

            return false;
        }
    }
}
