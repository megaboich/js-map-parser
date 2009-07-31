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

			treeView1.Nodes.Clear();
			if (Doc.Name.EndsWith(".js"))
			{
				var nodes = JavascriptParser.Parse(Code);
				FillNodes(nodes, treeView1.Nodes);
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
				treeNode.Tag = node.StartLine;
				dest.Add(treeNode);

				if (item.HasChildrens)
				{
					FillNodes(item, treeNode.Nodes);
				}
			}

			treeView1.ExpandAll();
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

		private void treeView1_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			if (treeView1.SelectedNode != null)
			{
				int line = (int)treeView1.SelectedNode.Tag;
				try
				{
					Selection.GotoLine(line, false);
					Doc.Activate();
					_dte.ActiveWindow.SetFocus();

				}
				catch { }
			}
		}

		private void button1_Click(object sender, EventArgs e)
		{
			System.Diagnostics.Debugger.Break();
		}
	}
}
