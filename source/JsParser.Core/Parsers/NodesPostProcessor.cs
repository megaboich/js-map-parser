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
        public static bool HideAnonymousFunctions(Hierachy<CodeNode> hierachy, JavascriptParserSettings settings)
        {
            if (settings.SkipAnonymousFuntions)
            {
                if (hierachy.Childrens != null && hierachy.Childrens.Any())
                {
                    hierachy.Childrens.RemoveAll(child => HideAnonymousFunctions(child, settings));
                }

                if (hierachy.Childrens != null && hierachy.Childrens.Any())
                {
                    return false;
                }

                if (hierachy.Item.AliasType == NodeAliasType.Anonymous)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
