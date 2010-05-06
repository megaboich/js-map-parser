using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using JsParserCore.Code;
using JsParserCore.Helpers;
using JsParserCore.Parsers;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using JsParserCore.Properties;

namespace JsParserCore.UI
{
	/// <summary>
	/// The tree for code.
	/// </summary>
	[ComVisibleAttribute(true)]
	public partial class NavigationTreeView : UserControl
	{
		private string _loadedDocName = string.Empty;
		private bool _canExpand = true;
		private MarksManager _marksManager = new MarksManager();
		private List<TreeNode> _tempTreeNodes = new List<TreeNode>();
		private static bool _versionChecked = false;
		private string _hash;

		/// <summary>
		/// Initializes a new instance of the <see cref="NavigationTreeView"/> class.
		/// </summary>
		public NavigationTreeView()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Gets Code.
		/// </summary>
		public ICodeProvider Code { get; private set; }

		/// <summary>
		/// Initialize method.
		/// </summary>
		public void Init(ICodeProvider codeProvider)
		{
			Code = codeProvider;
		}

		/// <summary>
		/// Clears the tree.
		/// </summary>
		public void Clear()
		{
			treeView1.Nodes.Clear();
		}

		/// <summary>
		/// Build the tree.
		/// </summary>
		public void LoadFunctionList()
		{
			_loadedDocName = Path.Combine(Code.Path, Code.Name);
			lbDocName.Text = Code.Name;
			lbDocName.ToolTipText = _loadedDocName;

			var code = Code.LoadCode();
			var hash = Convert.ToBase64String(SHA1.Create().ComputeHash(Encoding.Default.GetBytes(code)));
			if (_hash == hash)
			{
				return;
			}

			_hash = hash;

			treeView1.BeginUpdate();
			treeView1.Nodes.Clear();
			_tempTreeNodes.Clear();
			_canExpand = true;

			var isSort = btnSortToggle.Checked;
			var isHierarchy = btnTreeToggle.Checked;

			if (!Code.Name.ToLower().EndsWith(".js"))
			{
				code = CodeTransformer.KillNonJavascript(code);
			}

			code = CodeTransformer.KillAspNetTags(code);
			_marksManager.SetFile(_loadedDocName);
			var nodes = (new JavascriptParser()).Parse(code);
			FillNodes(nodes, treeView1.Nodes);

			if (!isHierarchy)
			{
				if (isSort)
				{
					_tempTreeNodes.Sort((n1, n2) => string.Compare(n1.Text, n2.Text));
				}

				foreach (TreeNode node in _tempTreeNodes)
				{
					treeView1.Nodes.Add(node);
				}
			}

			treeView1.EndUpdate();
		}

		private int GetImageIndex(string opCode)
		{
			switch (opCode)
			{
				case "Function":
					return -1;
				case "ObjectLiteral":
					return 1;
				default:
					return 2;
			}
		}

		private void FillNodes(Hierachy<CodeNode> source, TreeNodeCollection dest)
		{
			if (source.Childrens == null)
			{
				return;
			}

			var isSort = btnSortToggle.Checked;
			var isHierarchy = btnTreeToggle.Checked;
			var childrens = source.Childrens;
			if (isSort)
			{
				childrens.Sort((a1, a2) => string.Compare(a1.Item.Alias, a2.Item.Alias));
			}

			foreach (var item in childrens)
			{
				CodeNode node = item.Item;
				var caption = !string.IsNullOrEmpty(node.Alias)
					? node.Alias
					: string.Format("Anonymous function at line {0}", node.StartLine);

				CustomTreeNode treeNode = new CustomTreeNode(caption);
				treeNode.CodeNode = node;
				treeNode.ToolTipText = node.Comment;
				treeNode.StateImageIndex = GetImageIndex(node.Opcode);
				_marksManager.RestoreMark(treeNode);
				if (isHierarchy)
				{
					dest.Add(treeNode);
				}
				else
				{
					_tempTreeNodes.Add(treeNode);
				}

				if (item.HasChildrens)
				{
					FillNodes(item, treeNode.Nodes);
				}

				treeNode.Expand();
			}
		}

		private void btnRefresh_Click(object sender, EventArgs e)
		{
			RefreshTree();
		}

		public void RefreshTree()
		{
			try
			{
				if (Code != null)
				{
					_loadedDocName = string.Empty;
					_hash = string.Empty;
					LoadFunctionList();
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message + Environment.NewLine + ex.Source);
			}
		}

		private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
		{
			if (treeView1.SelectedNode != null)
			{
				CodeNode codeNode = ((CustomTreeNode)treeView1.SelectedNode).CodeNode;
				try
				{
					Code.SelectionMoveToLineAndOffset(codeNode.StartLine, codeNode.StartColumn + 1);
					Code.SetFocus();
				}
				catch { }
			}
		}

		private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
		{
			_canExpand = !e.Node.Bounds.Contains(e.X, e.Y);

			treeView1.SelectedNode = e.Node;

			if (e.Button == MouseButtons.Right)
			{
				resetLabelToolStripMenuItem.Enabled = !string.IsNullOrEmpty(((CustomTreeNode)e.Node).Tags);
				contextMenuStrip1.Show((Control) sender, e.X, e.Y);
			}
		}

		private void treeView1_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
		{
			if (!_canExpand)
			{
				e.Cancel = true;
			}
		}

		private void treeView1_BeforeExpand(object sender, TreeViewCancelEventArgs e)
		{
			if (!_canExpand)
			{
				e.Cancel = true;
			}
		}

		private void resetLabelToolStripMenuItem_Click(object sender, EventArgs e)
		{
			_marksManager.SetMark(null, (CustomTreeNode) treeView1.SelectedNode);
			treeView1.Refresh();
		}

		private void resetAllLabelsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			_marksManager.ResetMarks();
			RefreshTree();
		}

		private void NavigationTreeView_Load(object sender, EventArgs e)
		{
			
		}

		private void contextMenuStrip1_Opening(object sender, System.ComponentModel.CancelEventArgs e)
		{

		}

		private Image GetTagImage(char mark)
		{
			switch (mark)
			{
				case 'W':
					return JsParserCore.Properties.Resources.flag_white;
				case 'B':
					return JsParserCore.Properties.Resources.flag_blue;
				case 'G':
					return JsParserCore.Properties.Resources.flag_green;
				case 'O':
					return JsParserCore.Properties.Resources.flag_orange;
				case 'R':
					return JsParserCore.Properties.Resources.flag_red;
				default:
					return JsParserCore.Properties.Resources.icon_favourites;
			}
		}

		private void treeView1_DrawNode(object sender, DrawTreeNodeEventArgs e)
		{
			var node = (CustomTreeNode)e.Node;
			if (!string.IsNullOrEmpty(node.Tags))
			{
				var x = e.Bounds.Right + 2;
				foreach (char mark in node.Tags)
				{
					e.Graphics.DrawImageUnscaled(GetTagImage(mark), x, e.Bounds.Top - 1);
					x += 18;
				}
			}

			e.DrawDefault = true;
		}

		private void toolStripMenuItem6_Click(object sender, EventArgs e)
		{
			_marksManager.SetMark("W", (CustomTreeNode)treeView1.SelectedNode);
			treeView1.Refresh();
		}

		private void toolStripMenuItem5_Click(object sender, EventArgs e)
		{
			_marksManager.SetMark("G", (CustomTreeNode)treeView1.SelectedNode);
			treeView1.Refresh();
		}

		private void setLabelToolStripMenuItem_Click(object sender, EventArgs e)
		{
			_marksManager.SetMark("S", (CustomTreeNode)treeView1.SelectedNode);
			treeView1.Refresh();
		}

		private void toolStripMenuItem4_Click(object sender, EventArgs e)
		{
			_marksManager.SetMark("B", (CustomTreeNode)treeView1.SelectedNode);
			treeView1.Refresh();
		}

		private void toolStripMenuItem3_Click(object sender, EventArgs e)
		{
			_marksManager.SetMark("O", (CustomTreeNode)treeView1.SelectedNode);
			treeView1.Refresh();
		}

		private void toolStripMenuItem2_Click(object sender, EventArgs e)
		{
			_marksManager.SetMark("R", (CustomTreeNode)treeView1.SelectedNode);
			treeView1.Refresh();
		}

		private void timer1_Tick(object sender, EventArgs e)
		{
			timer1.Enabled = false;
			if (!_versionChecked)
			{
				VersionChecker.CheckVersion();
				_versionChecked = true;
			}
		}
	}
}
