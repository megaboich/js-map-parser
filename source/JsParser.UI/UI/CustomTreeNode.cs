using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using JsParser.Core.Code;

namespace JsParser.Core.UI
{
	public class CustomTreeNode : TreeNode
	{
		public CustomTreeNode(string text) : base(text) { }

		public CodeNode CodeNode { get; set; }

		public string Tags { get; set; }
	}
}
