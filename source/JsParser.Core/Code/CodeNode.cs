using System;
using System.Collections.Generic;

namespace JsParser.Core.Code
{
    /// <summary>
    /// The code node.
    /// </summary>
    public class CodeNode : IEquatable<CodeNode>
    {
        /// <summary>
        /// Gets or sets Alias.
        /// </summary>
        public string Alias { get; set; }

        /// <summary>
        /// Gets or sets StartLine.
        /// </summary>
        public int StartLine { get; set; }

        /// <summary>
        /// Gets or sets EndLine.
        /// </summary>
        public int EndLine { get; set; }

        /// <summary>
        /// Gets or sets EndColumn.
        /// </summary>
        public int EndColumn { get; set; }

        /// <summary>
        /// Gets or sets StartPosition.
        /// </summary>
        public int StartColumn { get; set; }

        /// <summary>
        /// Gets or sets type of node
        /// </summary>
        public CodeNodeType NodeType { get; set; }

        /// <summary>
        /// Gets or sets The Comment.
        /// </summary>
        public string Comment { get; set; }

        private static IComparer<CodeNode> _comparer = new CodeNodeComparer();

        public CodeNode()
        {
        }

        public CodeNode(string alias, int startLine, int endLine, string comment = null, CodeNodeType type = CodeNodeType.Function)
        {
            Alias = alias;
            StartLine = startLine;
            EndLine = endLine;
            Comment = comment;
            NodeType = type;
        }

        public bool Equals(CodeNode other)
        {
            return _comparer.Compare(this, other) == 0;
        }

        public override bool Equals(Object obj)
        {
            return Equals(obj as CodeNode);
        }

        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hash = 17;
                // Suitable nullity checks etc, of course :)
                hash = hash * 23 + StartLine.GetHashCode();
                hash = hash * 23 + EndLine.GetHashCode();
                hash = hash * 23 + (Alias ?? string.Empty).GetHashCode();
                hash = hash * 23 + (Comment ?? string.Empty).GetHashCode();
                hash = hash * 23 + NodeType.GetHashCode();
                return hash;
            }
        }

        public static IComparer<CodeNode> GetDefaultComparer()
        {
            return _comparer;
        }
    }
}