using System;
using System.Windows.Forms;
using EnvDTE;
using JS_addin.Addin.Code;
using JS_addin.Addin.Helpers;
using JS_addin.Addin.Parsers;

namespace JS_addin.Addin.UI
{
	/// <summary>
	/// The tree for code.
	/// </summary>
	public partial class NavigationTreeView : UserControl
	{
		private string _loadedDocName = string.Empty;
		private DTE _dte;
		private Document _doc;
		private bool _canExpand = true;

		/// <summary>
		/// Initializes a new instance of the <see cref="NavigationTreeView"/> class.
		/// </summary>
		public NavigationTreeView()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Sets DTE object.
		/// </summary>
		public DTE DTE
		{
			set { _dte = value; }
		}

		/// <summary>
		/// Gets Document.
		/// </summary>
		public Document Doc
		{
			get
			{
				if (_doc == null)
				{
					return _dte.ActiveDocument;
				}

				return _doc;
			}
		}

		/// <summary>
		/// Gets Selection.
		/// </summary>
		public TextSelection Selection
		{
			get
			{
				return (TextSelection)Doc.Selection;
			}
		}

		/// <summary>
		/// Gets Code.
		/// </summary>
		public string Code { get; private set; }

		/// <summary>
		/// Initialize method.
		/// </summary>
		/// <param name="dte">
		/// The dte param.
		/// </param>
		/// <param name="doc">
		/// The doc param.
		/// </param>
		/// <param name="debugActive">
		/// The debug active.
		/// </param>
		public void Init(DTE dte, Document doc)
		{
			this._dte = dte;
			this._doc = doc;
			Code = new CodeService(Doc).LoadCode();
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
			if (Doc == null || _loadedDocName == Doc.Path + Doc.Name)
			{
				return;
			}

			_loadedDocName = Doc.Path + Doc.Name;
			lbDocName.Text = Doc.Name;

			treeView1.BeginUpdate();
			treeView1.Nodes.Clear();
			_canExpand = true;
			
			var nodes = (new JavascriptParser()).Parse(Code);
			FillNodes(nodes, treeView1.Nodes);

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

			foreach (var item in source.Childrens)
			{
				CodeNode node = item.Item;
				var caption = !string.IsNullOrEmpty(node.Alias)
					? node.Alias
					: string.Format("Anonymous function at line {0}", node.StartLine);

				TreeNode treeNode = new TreeNode(caption);
				treeNode.Tag = node;
				treeNode.ToolTipText = node.Comment;
				treeNode.StateImageIndex = GetImageIndex(node.Opcode);
				dest.Add(treeNode);

				if (item.HasChildrens)
				{
					FillNodes(item, treeNode.Nodes);
				}

				treeNode.Expand();
			}
		}

		private void btnRefresh_Click(object sender, EventArgs e)
		{
			try
			{
				this.Dock = DockStyle.Fill;
				Refresh();
				Code = new CodeService(Doc).LoadCode();
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
					// Selection.GotoLine(codeNode.StartLine, false);
					Selection.MoveToLineAndOffset(codeNode.StartLine, codeNode.StartColumn, false);
					Doc.Activate();
					_dte.ActiveWindow.SetFocus();
				}
				catch { }
			}
		}

		private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
		{
			_canExpand = !e.Node.Bounds.Contains(e.X, e.Y);
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
	}
}
