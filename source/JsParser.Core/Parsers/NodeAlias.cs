using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace JsParserCore.Parsers
{
    public enum NodeAliasType
    {
        Unknown = 0,
        AnonymousFunction = 1,
        AnonymousFunctionInParameter = 2,
        Variable = 3,
    }

    public class NodeAlias
    {
        public string Text { get; set; }

        public NodeAlias Prev { get; set; }

        public NodeAliasType Type {get; set;}

        public NodeAlias(string text, NodeAliasType type = NodeAliasType.Unknown)
        {
            Text = text;
            Type = type;
            Prev = null;
        }

        public void AppendPrev(NodeAlias prev, int level = 0)
        {
            if (Prev == null)
            {
                Prev = prev;
            }
            else
            {
                //if (level > 25)
                //{
                //    return;
                //}
                Prev.AppendPrev(prev, ++level);
            }
        }

        public string GetFullText()
        {
            string t = Text;
            var type = Type;
            var current = Prev;
            //int counter = 0;
            while (current != null /*&& ++counter <= 20*/)
            {
                var concatenator = (type == NodeAliasType.AnonymousFunctionInParameter) ? ">" : ".";
                t = current.Text + concatenator + t;
                type = current.Type;
                current = current.Prev;
            }

            //if (counter >= 20)
            //{
            //    t = "ERROR" + t;
            //}

            return t;
        }

        public override string ToString()
        {
            return GetFullText();
        }

        public NodeAlias Clone()
        {
            return new NodeAlias(this.Text, this.Type)
            {
                Prev = this.Prev == null ? null : Prev.Clone()
            };
        }
    }

    public static class NodeAliasExtension
    {
        public static NodeAlias Concat(this NodeAlias nodeAlias, NodeAlias prev)
        {
            if (nodeAlias == null)
            {
                return prev;
            }

            if (prev == null)
            {
                return nodeAlias;
            }

            // Cloning objects to avoid circular dependecies
            prev.AppendPrev(nodeAlias.Clone());
            return prev;
        }
    }
}
