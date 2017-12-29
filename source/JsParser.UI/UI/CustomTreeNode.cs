using JsParser.Core.Code;
using System.Windows.Forms;

namespace JsParser.UI.UI
{
	public class CustomTreeNode : TreeNode
	{
		public CustomTreeNode(string text) : base(text) { }

		public CodeNode CodeNode { get; set; }

		public string Tags { get; set; }
	}
}
