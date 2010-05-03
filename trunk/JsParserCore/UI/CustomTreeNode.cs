using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using JsParserCore.Code;

namespace JsParserCore.UI
{
	public class CustomTreeNode : TreeNode
	{
		public CustomTreeNode(string text) : base(text) { }

		public CodeNode CodeNode { get; set; }

		public string Tags { get; set; }
	}
}
