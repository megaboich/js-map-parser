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
using System.Linq;

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
		private int _lastCodeLine = -1;
		private List<CodeNode> _functions;
		private int _lastActiveLine;
		private int _lastActiveColumn;
		private bool _treeRefreshing = false;
		private ExpandedNodesManager _expandedNodes = new ExpandedNodesManager();
		private bool _firstLoadOfDocument = false;

		/// <summary>
		/// Gets Code.
		/// </summary>
		public ICodeProvider Code { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="NavigationTreeView"/> class.
		/// </summary>
		public NavigationTreeView()
		{
			InitializeComponent();

			treeView1.Nodes.Clear();
			treeView1.LostFocus += LostFocusHandler;

			btnSortToggle.Checked = Settings.SortingEnabled;
			showHierarhyToolStripMenuItem.Checked = Settings.HierarchyEnabled;
			btnShowLineNumbers.Checked = Settings.ShowLineNumbersEnabled;
			btnFilterByMarks.Checked = Settings.FilterByMarksEnabled;
			expandAllByDefaultToolStripMenuItem.Checked = Settings.AutoExpandAll;
		}

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
		/// Gets status of the tree.
		/// </summary>
		public bool TreeLoaded
		{
			get
			{
				return treeView1.Nodes.Count > 0;
			}
		}

		/// <summary>
		/// Settings instance
		/// </summary>
		public Settings Settings
		{
			get { return Settings.Default; }
		}

		/// <summary>
		/// Build the tree.
		/// </summary>
		public bool LoadFunctionList()
		{
			//check extension
			if (!CheckExt(Code.Name))
			{
				return false;
			}

			var docName = Path.Combine(Code.Path, Code.Name);
			_firstLoadOfDocument = !_expandedNodes.HasDocumentInStorage(docName);
			if (_loadedDocName != docName)
			{
				_loadedDocName = docName;
				_expandedNodes.ActiveDocumentName = _loadedDocName;
				//We load the new document.
			}

			lbDocName.Text = Code.Name;
			lbDocName.ToolTipText = _loadedDocName;

			var code = Code.LoadCode();
			var hash = Convert.ToBase64String(SHA1.Create().ComputeHash(Encoding.Default.GetBytes(code)));
			if (_hash == hash)
			{
				return true;
			}

			_hash = hash;
			_treeRefreshing = true;
			treeView1.BeginUpdate();
			treeView1.Nodes.Clear();
			_tempTreeNodes.Clear();
			_canExpand = true;

			var isSort = Settings.SortingEnabled;
			var isHierarchy = Settings.HierarchyEnabled;

			_marksManager.SetFile(_loadedDocName);

			code = CodeTransformer.KillAspNetTags(code);
			var codeChunks = CodeTransformer.ExtractJsFromSource(code);
			var nodes = (new JavascriptParser()).Parse(codeChunks);

			_lastCodeLine = -1;
			_functions = new List<CodeNode>();
			FillNodes(nodes, treeView1.Nodes, 0, _functions);

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

			if (btnFilterByMarks.Checked)
			{
				HideUnmarkedNodes(treeView1.Nodes);
			}

			treeView1.EndUpdate();
			_treeRefreshing = false;
			OnResize(null);
			panelLinesNumbers.Refresh();
			return treeView1.Nodes.Count > 0;
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

		private bool CheckExt(string fileName)
		{
			if (Settings.Extensions.Count > 0)
			{
				foreach (var ext in Settings.Extensions)
				{
					if (Code.Name.ToLower().EndsWith(ext, StringComparison.InvariantCultureIgnoreCase))
					{
						return true;
					}
				}

				return false;
			}

			return true;
		}

		private bool HideUnmarkedNodes(TreeNodeCollection nodes)
		{
			bool hasMarks = false;
			var nodess = nodes.Cast<CustomTreeNode>().ToArray();
			foreach (TreeNode tnode in nodess)
			{
				if (tnode == null)
				{
					continue;
				}

				CustomTreeNode node = (CustomTreeNode)tnode;
				if (!string.IsNullOrEmpty(node.Tags))
				{
					hasMarks = true;
					continue;
				}

				if (node.Nodes.Count > 0)
				{
					var hasChildMarks = HideUnmarkedNodes(node.Nodes);
					if (hasChildMarks)
					{
						hasMarks = true;
					}
					else
					{
						node.Remove();
					}
				}

				if (node.Nodes.Count == 0 && string.IsNullOrEmpty(node.Tags))
				{
					node.Remove();
				}
			}

			return hasMarks;
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

		private void FillNodes(Hierachy<CodeNode> source, TreeNodeCollection dest, int level, IList<CodeNode> functions)
		{
			if (source.Childrens == null)
			{
				return;
			}

			var isSort = Settings.SortingEnabled;
			var isHierarchy = Settings.HierarchyEnabled;
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

				if (node.StartLine > _lastCodeLine)
				{
					_lastCodeLine = node.StartLine;
				}

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

				functions.Add(node);

				if (item.HasChildrens)
				{
					FillNodes(item, treeNode.Nodes, level + 1, functions);
				}

				if (_firstLoadOfDocument && Settings.AutoExpandAll)
				{
					treeNode.Expand();
				}

				if (_expandedNodes.IsNoteExpanded(treeNode))
				{
					treeNode.Expand();
				}
			}
		}

		private void GotoSelected()
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
				case '!':
					return JsParserCore.Properties.Resources.Active;
				case 'S':
				default:
					return JsParserCore.Properties.Resources.icon_favourites;
			}
		}

		public void Find()
		{
			if (_functions != null)
			{
				FindDialog fd = new FindDialog(_functions, FindCallBack);
				fd.ShowDialog();
			}
		}

		private bool FindCallBack(CodeNode codeNode)
		{
			try
			{
				var node = SearchNode(treeView1.Nodes, codeNode);
				if (node != null)
				{
					treeView1.SelectedNode = node;
					GotoSelected();
				}
				else
				{
					Code.SelectionMoveToLineAndOffset(codeNode.StartLine, codeNode.StartColumn + 1);
					Code.SetFocus();
				}
			}
			catch { }
			return true;
		}

		private bool ScanTreeView(Func<CustomTreeNode, bool> funcDelegate, TreeNodeCollection nodes, bool scanExpandedNodes = true)
		{
			bool continueScan = true;
			if (TreeLoaded)
			{
				foreach (CustomTreeNode node in nodes)
				{
					continueScan = funcDelegate(node);

					if (!continueScan)
					{
						return false;
					}

					if (node.IsExpanded || scanExpandedNodes)
					{
						if (node.Nodes.Count > 0)
						{
							continueScan = ScanTreeView(funcDelegate, node.Nodes, scanExpandedNodes);

							if (!continueScan)
							{
								return false;
							}
						}
					}
				}
			}

			return true;
		}

		private TreeNode SearchNode(TreeNodeCollection nodes, CodeNode cn)
		{
			foreach (CustomTreeNode node in nodes)
			{
				if (node.CodeNode == cn)
				{
					return node;
				}

				var inner = SearchNode(node.Nodes, cn);

				if (inner != null)
				{
					return inner;
				}
			}

			return null;
		}

		private string GetStringsHash(IEnumerable<string> col)
		{
			return col.Any() ? col.Aggregate((k, a) => a += k) : string.Empty;
		}

		private void SaveSettings()
		{
			Settings.SortingEnabled = btnSortToggle.Checked;
			Settings.HierarchyEnabled = showHierarhyToolStripMenuItem.Checked;
			Settings.ShowLineNumbersEnabled = btnShowLineNumbers.Checked;
			Settings.FilterByMarksEnabled = btnFilterByMarks.Checked;
			Settings.AutoExpandAll = expandAllByDefaultToolStripMenuItem.Checked;
			Settings.Save();
		}

		#region Event handlers

		private void treeView1_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == 13)
			{
				GotoSelected();
			}
		}

		private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
		{
			_canExpand = !e.Node.Bounds.Contains(e.X, e.Y);

			treeView1.SelectedNode = e.Node;

			if (e.Button == MouseButtons.Right)
			{
				resetLabelToolStripMenuItem.Enabled = !string.IsNullOrEmpty(((CustomTreeNode)e.Node).Tags);
				contextMenuStrip1.Show((Control)sender, e.X, e.Y);
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
			_marksManager.SetMark(null, (CustomTreeNode)treeView1.SelectedNode);
			treeView1.Refresh();
		}

		private void resetAllLabelsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			_marksManager.ResetMarks();
			RefreshTree();
		}

		private void btnRefresh_Click(object sender, EventArgs e)
		{
			SaveSettings();
			RefreshTree();
		}

		private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
		{
			GotoSelected();
		}

		private void treeView1_DrawNode(object sender, DrawTreeNodeEventArgs e)
		{
			var node = (CustomTreeNode)e.Node;
			var tags = node.Tags;

			if (!string.IsNullOrEmpty(tags))
			{
				var x = e.Bounds.Right + 2;
				foreach (char mark in tags)
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

		private void showLineNumbersToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SaveSettings();
			OnResize(null);
			panelLinesNumbers.Refresh();
		}

		private void btnFilterByMarks_Click(object sender, EventArgs e)
		{
			RefreshTree();
		}

		private void NavigationTreeView_Resize(object sender, EventArgs e)
		{
			var tw = Convert.ToInt32(Math.Round(this.CreateGraphics().MeasureString(_lastCodeLine.ToString(), Font).Width)) + 2;
			treeView1.Left = Settings.ShowLineNumbersEnabled ? tw : 0;
			treeView1.Top = 25;
			treeView1.Width = this.ClientSize.Width - treeView1.Left;
			treeView1.Height = this.ClientSize.Height - treeView1.Top;
			panelLinesNumbers.Left = 0;
			panelLinesNumbers.Width = tw;
			panelLinesNumbers.Top = 25;
			panelLinesNumbers.Height = treeView1.Height;
			panelLinesNumbers.Visible = Settings.ShowLineNumbersEnabled;
		}

		private void treeView1_OnScroll(object sender, EventArgs e)
		{
			panelLinesNumbers.Refresh();
		}

		private void panelLinesNumbers_Paint(object sender, PaintEventArgs e)
		{
			e.Graphics.FillRectangle(SystemBrushes.Control, panelLinesNumbers.ClientRectangle);
			if (Settings.ShowLineNumbersEnabled && treeView1.Nodes.Count > 0)
			{
				var gr = e.Graphics;
				ScanTreeView(node =>
				{
					int p = node.Bounds.Top + 2;
					if (p < 0)
					{
						return true;
					}

					if (p > panelLinesNumbers.Height)
					{
						return false; //This means stop scan anymore
					}

					var nodeHeight = node.Bounds.Height;
					var s = node.CodeNode.StartLine.ToString();
					gr.DrawString(s, Font, Brushes.Gray, new Rectangle(0, p, panelLinesNumbers.Width, nodeHeight));
					return true;
				}, treeView1.Nodes, false);
			}
		}

		private void toolFindButton_Click(object sender, EventArgs e)
		{
			Find();
		}

		private void LostFocusHandler(object sender, EventArgs e)
		{
			SaveSettings();
		}

		private void timer2_Tick_1(object sender, EventArgs e)
		{
			try
			{
				if (Code != null && Settings.TrackActiveItem)
				{
					int line;
					int column;
					Code.GetCursorPos(out line, out column);
					if (line >= 0 && (line != _lastActiveLine || column != _lastActiveColumn))
					{
						CustomTreeNode hightLightNode = null;
						ScanTreeView(node => 
							{
								bool sel = false;
								if (node.CodeNode.StartLine <= line && line <= node.CodeNode.EndLine)
								{
									if (node.CodeNode.StartLine == node.CodeNode.EndLine)
									{
										if (node.CodeNode.StartColumn <= column && column <= node.CodeNode.EndColumn)
										{
											sel = true;
										}
									}
									else
									{
										sel = true;
									}
								}

								if (sel)
								{
									if (hightLightNode != null && node.Level < hightLightNode.Level)
									{
										return true;	//Skip parent nodes
									}

									hightLightNode = node;
								}

								return true;
							}, treeView1.Nodes);
						_lastActiveLine = line;
						_lastActiveColumn = column;
						if (hightLightNode != null && treeView1.SelectedNode != hightLightNode)
						{
							treeView1.SelectedNode = hightLightNode;
						}
					}
				}
			}
			catch { }
		}

		private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SettingsForm sf = new SettingsForm();
			sf.ShowDialog();
		}

		private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
		{
			panelLinesNumbers.Refresh();
		}

		private void showHierarhyToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SaveSettings();
			RefreshTree();
		}

		private void expandAllByDefaultToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SaveSettings();
			RefreshTree();
		}

		private void expandAllToolStripMenuItem_Click(object sender, EventArgs e)
		{
			_treeRefreshing = true;
			treeView1.BeginUpdate();
			treeView1.ExpandAll();
			treeView1.EndUpdate();
			_treeRefreshing = false;
			panelLinesNumbers.Refresh();
		}

		private void collapseAllNodesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			_treeRefreshing = true;
			treeView1.BeginUpdate();
			treeView1.CollapseAll();
			treeView1.EndUpdate();
			_treeRefreshing = false;
			panelLinesNumbers.Refresh();
		}

		private void treeView1_AfterExpand(object sender, TreeViewEventArgs e)
		{
			_expandedNodes.SetExpandedState((CustomTreeNode)e.Node);
			if (Settings.ShowLineNumbersEnabled && !_treeRefreshing)
			{
				panelLinesNumbers.Refresh();
			}
		}

		private void treeView1_AfterCollapse(object sender, TreeViewEventArgs e)
		{
			_expandedNodes.SetExpandedState((CustomTreeNode)e.Node);
			if (Settings.ShowLineNumbersEnabled && !_treeRefreshing)
			{
				panelLinesNumbers.Refresh();
			}
		}

		#endregion
	}
}
