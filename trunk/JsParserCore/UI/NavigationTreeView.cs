using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using JsParserCore.Code;
using JsParserCore.Helpers;
using JsParserCore.Parsers;

namespace JsParserCore.UI
{
	/// <summary>
	/// The tree for code.
	/// </summary>
	public partial class NavigationTreeView : UserControl
	{
		private string _loadedDocName = string.Empty;
		private bool _canExpand = true;
		private List<string> _bookmarkedItems = new List<string>();
		private List<TreeNode> _tempTreeNodes = new List<TreeNode>();
		private static bool _versionChecked = false;

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
			_loadedDocName = Code.Path + Code.Name;
			lbDocName.Text = Code.Name;
			lbDocName.ToolTipText = Code.Path + Code.Name;

			treeView1.BeginUpdate();
			treeView1.Nodes.Clear();
			_tempTreeNodes.Clear();
			_canExpand = true;

			var isSort = btnSortToggle.Checked;
			var isHierarchy = btnTreeToggle.Checked;

			var nodes = (new JavascriptParser()).Parse(Code.LoadCode());
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

				TreeNode treeNode = new TreeNode(caption);
				treeNode.Tag = node;
				treeNode.ToolTipText = node.Comment;
				treeNode.StateImageIndex = GetImageIndex(node.Opcode);
				if (_bookmarkedItems.Contains(caption))
				{
					SelectNode(treeNode, true);
				}
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
				this.Dock = DockStyle.Fill;
				Refresh();
				_loadedDocName = string.Empty;
				LoadFunctionList();
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
				CodeNode codeNode = (CodeNode)treeView1.SelectedNode.Tag;
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

		private void SelectNode(TreeNode node, bool select)
		{
			if (select)
			{
				node.BackColor = Color.Aqua;
			}
			else
			{
				node.BackColor = SystemColors.Window;
			}
		}

		private void resetLabelToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SelectNode(treeView1.SelectedNode, false);
			_bookmarkedItems.Remove(treeView1.SelectedNode.Text);
		}

		private void setLabelToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SelectNode(treeView1.SelectedNode, true);
			_bookmarkedItems.Add(treeView1.SelectedNode.Text);
		}

		private void resetAllLabelsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			_bookmarkedItems.Clear();
			RefreshTree();
		}

		private void NavigationTreeView_Load(object sender, EventArgs e)
		{
			if (!_versionChecked)
			{
				VersionChecker.CheckVersion();
				_versionChecked = true;
			}
		}
	}
}
