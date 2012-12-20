using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using JsParser.Core.Code;
using JsParser.UI.Helpers;
using JsParser.Core.Parsers;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using JsParser.UI.Properties;
using System.Linq;
using System.Threading;
using System.Diagnostics;
using JsParser.Core.Helpers;

namespace JsParser.UI.UI
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
		private ExpandedNodesManager _expandedNodesManager = new ExpandedNodesManager();
		private List<TreeNode> _tempTreeNodes = new List<TreeNode>();
		private int _lastCodeLine = -1;
		private List<CodeNode> _functions;
		private int _lastActiveLine;
		private int _lastActiveColumn;
		private bool _treeRefreshing = false;
		private bool _userWantsUpdateSplitterPosition = false;
		private ColorTable _colorTable = ColorTable.Default;
		private Palette _palette = new Palette();
		private JSParserResult _lastParserResult;
		private ICodeProvider _codeProvider;

		
		/// <summary>
		/// Initializes a new instance of the <see cref="NavigationTreeView"/> class.
		/// </summary>
		public NavigationTreeView()
		{
			InitializeComponent();
			this.Disposed += OnDisposed;

			treeView1.Nodes.Clear();
			treeView1.LostFocus += LostFocusHandler;

			sortItemsAlphabeticallyToolStripMenuItem.Checked = Settings.SortingEnabled;
			showHierarhyToolStripMenuItem.Checked = Settings.HierarchyEnabled;
			showLineNumbersToolStripMenuItem.Checked = Settings.ShowLineNumbersEnabled;
			filterByMarksToolStripMenuItem.Checked = Settings.FilterByMarksEnabled;
			expandAllByDefaultToolStripMenuItem.Checked = Settings.AutoExpandAll;
			hideAnonymousFunctionsToolStripMenuItem.Checked = Settings.HideAnonymousFunctions;
		}

		public void InitColors(IUIThemeProvider uiThemeProvider)
		{
			if (Settings.Default.UseVSColorTheme)
			{
				_colorTable = uiThemeProvider.GetColors();
			}
			else
			{
				_colorTable = ColorTable.Default;
			}

			BackColor = _colorTable.ControlBackground;
			ForeColor = _colorTable.ControlText;
			treeView1.BackColor = _colorTable.WindowBackground;
			treeView1.ForeColor = _colorTable.WindowText;
			lbTaskList.ForeColor = _colorTable.TabText;
			taskListDataGrid.BackColor = _colorTable.WindowBackground;
			taskListDataGrid.BackgroundColor = _colorTable.WindowBackground;
			taskListDataGrid.ForeColor = _colorTable.WindowText;
			taskListDataGrid.GridColor = _colorTable.GridLines;
			taskListDataGrid.RowsDefaultCellStyle.BackColor = _colorTable.WindowBackground;
			taskListDataGrid.RowsDefaultCellStyle.ForeColor = _colorTable.WindowText;

			toolStrip2.BackColor = _colorTable.ControlBackground;

			treeView1.Indent = Settings.Default.UseVSColorTheme ? 10 : 16;
			treeView1.DrawMode = Settings.Default.UseVSColorTheme
				? TreeViewDrawMode.OwnerDrawAll
				: TreeViewDrawMode.OwnerDrawText;
			treeView1.ShowLines = !Settings.Default.UseVSColorTheme;

		}

		public void OnDisposed(object sender, EventArgs args)
		{
			_palette.Dispose();
		}

		public void Init(ICodeProvider codeProvider)
		{
			
			_codeProvider = codeProvider;
			StatisticsManager.Instance.Statistics.Container = _codeProvider.ContainerName;
			StatisticsManager.Instance.Statistics.UpdateStatisticsFromSettings();

			PerformNetworkActivity();
		}

		/// <summary>
		/// Clears the tree and other UI.
		/// </summary>
		public void Clear()
		{
			_loadedDocName = string.Empty;
			treeView1.BeginUpdate();
			treeView1.Nodes.Clear();
			treeView1.EndUpdate();
			splitContainer1.Panel2Collapsed = true;
			_functions = new List<CodeNode>();
			OnResize(null);
			panelLinesNumbers.Refresh();
			btnErrorDiagnosis.Visible = false;
			btnErrorSeparator.Visible = false;
			lbTaskList.Text = "      Task List";
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
		public bool UpdateTree(JSParserResult result, ICodeProvider codeProvider)
		{
			if (result == null)
			{
				Clear();
				return false;
			}
			if(string.IsNullOrEmpty(result.FileName))
			{
				return false;
			}

			_lastParserResult = result;
			_codeProvider = codeProvider;

			_loadedDocName = _lastParserResult.FileName;
			_expandedNodesManager.SetFile(_loadedDocName);
			_marksManager.SetFile(_loadedDocName);

			var isSort = Settings.SortingEnabled;
			var isHierarchy = Settings.HierarchyEnabled;

			_treeRefreshing = true;
			treeView1.BeginUpdate();
			treeView1.Nodes.Clear();
			_tempTreeNodes.Clear();
			_canExpand = true;

			var nodes = result.Nodes;

			if (result.Errors.Count > 0)
			{
				btnErrorDiagnosis.Visible = true;
				btnErrorSeparator.Visible = true;
				btnErrorDiagnosis.DropDownItems.Clear();
				result.Errors.ForEach(er =>
				{
					var item = btnErrorDiagnosis.DropDownItems.Add(er.Message.SplitWordsByCamelCase() + ".\r\nLine: " + er.StartLine, null, ErrorDiagnosisClick);
					item.Tag = er;
				});
			}
			else
			{
				btnErrorDiagnosis.Visible = false;
				btnErrorSeparator.Visible = false;
			}

			var tasksDataSource = new List<object>();
			if (result.TaskList.Count > 0)
			{
				lbTaskList.Text = string.Format("      Task List: {0} items", result.TaskList.Count);
				int i = 0;
				result.TaskList.ForEach(t =>
				{
					++i;
					var item = new {
						No = i.ToString(),
						Desc = t.Description,
						Line = t.StartLine.ToString(),
					};

					tasksDataSource.Add(item);
				});

				splitContainer1.Panel2Collapsed = false;

				SetToDoSplitterPosition();
			}
			else
			{
				splitContainer1.Panel2Collapsed = true;
				tasksDataSource.Add(new {No = "", Desc="", Line="" }); //add fake row to workaround column sizing bug
			}

			taskListDataGrid.DataSource = tasksDataSource;
			taskListDataGrid.CurrentCell = null;
			

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

			if (filterByMarksToolStripMenuItem.Checked)
			{
				HideUnmarkedNodes(treeView1.Nodes);
			}

			treeView1.EndUpdate();
			_treeRefreshing = false;
			AdjustLineNumbersPanelSize();
			panelLinesNumbers.Refresh();
			return treeView1.Nodes.Count > 0;
		}

		public void RefreshTree()
		{
			UpdateTree(_lastParserResult, _codeProvider);
		}

		private void PerformNetworkActivity()
		{
			VersionChecker.CheckVersion();
			StatisticsSender.Send();
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
				case "Variable":
					return 2;
				default:
					return 3;
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

				if (node.StartLine > _lastCodeLine)
				{
					_lastCodeLine = node.StartLine;
				}

				CustomTreeNode treeNode = new CustomTreeNode(node.Alias);
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

				var isExpanded = _expandedNodesManager.IsNoteExpanded(treeNode);
				if (isExpanded.HasValue)
				{
					if (isExpanded.Value)
					{
						treeNode.Expand();
					}
				}
				else
				{
					if (Settings.AutoExpandAll)
					{
						treeNode.Expand();
					}
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
					_codeProvider.SelectionMoveToLineAndOffset(codeNode.StartLine, codeNode.StartColumn + 1);
					_codeProvider.SetFocus();
					++StatisticsManager.Instance.Statistics.NavigateFromFunctionsTreeCount;
				}
				catch { }
			}
		}

		private Image GetTagImage(char mark)
		{
			switch (mark)
			{
				case 'W':
					return JsParser.UI.Properties.Resources.flag_white;
				case 'B':
					return JsParser.UI.Properties.Resources.flag_blue;
				case 'G':
					return JsParser.UI.Properties.Resources.flag_green;
				case 'O':
					return JsParser.UI.Properties.Resources.flag_orange;
				case 'R':
					return JsParser.UI.Properties.Resources.flag_red;
				case '!':
					return JsParser.UI.Properties.Resources.Active;
				case 'S':
				default:
					return JsParser.UI.Properties.Resources.icon_favourites;
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
					_codeProvider.SelectionMoveToLineAndOffset(codeNode.StartLine, codeNode.StartColumn + 1);
					_codeProvider.SetFocus();
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

		private void SaveSettings()
		{
			Settings.SortingEnabled = sortItemsAlphabeticallyToolStripMenuItem.Checked;
			Settings.HierarchyEnabled = showHierarhyToolStripMenuItem.Checked;
			Settings.ShowLineNumbersEnabled = showLineNumbersToolStripMenuItem.Checked;
			Settings.FilterByMarksEnabled = filterByMarksToolStripMenuItem.Checked;
			Settings.AutoExpandAll = expandAllByDefaultToolStripMenuItem.Checked;
			Settings.HideAnonymousFunctions = hideAnonymousFunctionsToolStripMenuItem.Checked;
			StatisticsManager.Instance.Statistics.UpdateStatisticsFromSettings();
			StatisticsManager.Instance.UpdateSettingsWithStatistics();
			Settings.Save();
		}

		private void AdjustLineNumbersPanelSize()
		{
			var tw = 0;
			using (var g = this.CreateGraphics())
			{
				tw = Convert.ToInt32(Math.Round(g.MeasureString(_lastCodeLine.ToString(), Font).Width)) + 2;
			}

			treeView1.Left = Settings.ShowLineNumbersEnabled ? tw : 0;
			treeView1.Top = 0;
			treeView1.Width = splitContainer1.Panel1.ClientSize.Width - treeView1.Left;
			treeView1.Height = splitContainer1.Panel1.ClientSize.Height - treeView1.Top;
			panelLinesNumbers.Left = 0;
			panelLinesNumbers.Width = tw;
			panelLinesNumbers.Top = 0;
			panelLinesNumbers.Height = splitContainer1.Panel1.ClientSize.Height;
			panelLinesNumbers.Visible = Settings.ShowLineNumbersEnabled;
		}

		#region Event handlers

		private void treeView1_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == 13)
			{
				GotoSelected();
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

		private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
		{
			_canExpand = !e.Node.Bounds.Contains(e.X, e.Y);

			treeView1.SelectedNode = e.Node;

			if (e.Button == MouseButtons.Right)
			{
				//Apply fonts and colors to menu items
				var nodeTags = ((CustomTreeNode)e.Node).Tags ?? string.Empty;
				resetLabelToolStripMenuItem.Enabled = !string.IsNullOrEmpty(nodeTags);
				
				var menuItems = new[] { contextMenuMarks0Item, contextMenuMarks1Item, contextMenuMarks2Item, contextMenuMarks3Item, contextMenuMarks4Item, contextMenuMarks5Item };
				var menuFonts = new[] { Settings.taggedFunction1Font, Settings.taggedFunction2Font, Settings.taggedFunction3Font, Settings.taggedFunction4Font, Settings.taggedFunction5Font, Settings.taggedFunction6Font };
				var menuColors = new[] { Settings.taggedFunction1Color, Settings.taggedFunction2Color, Settings.taggedFunction3Color, Settings.taggedFunction4Color, Settings.taggedFunction5Color, Settings.taggedFunction6Color };

				for (int i = 0; i < menuItems.Length; ++i)
				{
					menuItems[i].Font = menuFonts[i];
					menuItems[i].ForeColor = menuColors[i];
					menuItems[i].Checked = nodeTags.Contains((string)menuItems[i].Tag);
				}

				contextMenuStrip1.Show((Control)sender, e.X, e.Y);

				++StatisticsManager.Instance.Statistics.TreeContextMenuExecutedCount;
			}
		}

		private void contextMenuMarks0Item_Click(object sender, EventArgs e)
		{
			var menuItem = (ToolStripMenuItem)sender;
			_marksManager.SetMark((string)menuItem.Tag, (CustomTreeNode)treeView1.SelectedNode);
			treeView1.Refresh();
			++StatisticsManager.Instance.Statistics.SetMarkExecutedCount;
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

			//workaround for a bug when nodes draws twice - first attempt on zero boundaries
			if (node.Bounds.Width == 0 && node.Bounds.Height == 0)
			{
				return;
			}

			var tags = node.Tags;

			if (Settings.Default.UseVSColorTheme)
			{

				// Retrieve the node font. If the node font has not been set,
				// use the TreeView font.
				Font nodeFont = e.Node.NodeFont;
				if (nodeFont == null) nodeFont = ((TreeView)sender).Font;

				var hasExpand = (e.Node.Nodes != null && e.Node.Nodes.Count > 0);
				var hasImage = (e.Node.StateImageIndex >= 0);
				var hasTags = !string.IsNullOrEmpty(tags);
				var isSelected = ((e.State & TreeNodeStates.Selected) != 0);

				var tagsShift = 0;

				// Draw background
				var bgBrush = isSelected
					? (treeView1.Focused)
						? _palette.GetSolidBrush(_colorTable.HighlightBackground)
						: _palette.GetSolidBrush(_colorTable.HighlightInactiveBackground)
					: _palette.GetSolidBrush(_colorTable.WindowBackground);

				e.Graphics.FillRectangle(bgBrush, 0, e.Bounds.Top, e.Bounds.Right, e.Bounds.Height);

				//Draw tags
				if (hasTags)
				{
					foreach (char mark in tags)
					{
						e.Graphics.DrawImageUnscaled(GetTagImage(mark), e.Node.Bounds.Left + tagsShift, e.Bounds.Top);
						tagsShift += 18;
					}
				}

				var nodeLeftShift = 0;
				// Draw image before node
				if (hasImage)
				{
					nodeLeftShift += 16;
					e.Graphics.DrawImageUnscaled(imageList1.Images[e.Node.StateImageIndex],
						Point.Subtract(e.Node.Bounds.Location, new Size(16, 0)));
				}

				// Draw + - sign before node
				if (hasExpand)
				{
					var img = e.Node.IsExpanded ? Resources.treeleaf_expanded : Resources.treeleaf_collapsed;

					e.Graphics.DrawImageUnscaled(img, e.Node.Bounds.Location.X - 11 - nodeLeftShift, e.Node.Bounds.Location.Y + 3, 9, 9);
				}

				var textColor = (e.Node.ForeColor.ToArgb() == 0)
					? _colorTable.WindowText
					: e.Node.ForeColor;

				// Draw the node text.
				var textColorToUse = ((e.State & TreeNodeStates.Selected) != 0)
					? (treeView1.Focused)
						? _colorTable.HighlightText
						: _colorTable.HighlightInactiveText
					: textColor;

				var textBrush = _palette.GetSolidBrush(textColorToUse);

				TextRenderer.DrawText(e.Graphics, e.Node.Text, nodeFont,
					Point.Add(e.Node.Bounds.Location, new Size(2 + tagsShift, 0)),
					textColorToUse);
				/*
				e.Graphics.DrawString(e.Node.Text, nodeFont, textBrush,
					Point.Add(e.Node.Bounds.Location, new Size(2 + tagsShift, 0)));
				*/
				e.DrawDefault = false;
			}
			else
			{
				//Draw tags
				if (!string.IsNullOrEmpty(tags))
				{
					var tagsShift = 2;
					foreach (char mark in tags)
					{
						e.Graphics.DrawImageUnscaled(GetTagImage(mark), e.Bounds.Right + tagsShift, e.Bounds.Top);
						tagsShift += 18;
					}
				}

				e.DrawDefault = true;
			}
		}

		private void ErrorDiagnosisClick(object sender, EventArgs e)
		{
			var errorMessage = (ErrorMessage)((ToolStripItem)sender).Tag;
			try
			{
				_codeProvider.SelectionMoveToLineAndOffset(errorMessage.StartLine, errorMessage.StartColumn + 1);
				_codeProvider.SetFocus();

				++StatisticsManager.Instance.Statistics.NavigateFromErrorListCount;
			}
			catch { }
		}

		private void taskListDataGrid_CellClick(object sender, DataGridViewCellEventArgs e)
		{
			var item = _lastParserResult.TaskList[e.RowIndex];
			try
			{
				_codeProvider.SelectionMoveToLineAndOffset(item.StartLine, item.StartColumn + 1);
				_codeProvider.SetFocus();
				++StatisticsManager.Instance.Statistics.NavigateFromToDoListCount;
			}
			catch { }
		}

		private void showLineNumbersToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SaveSettings();
			AdjustLineNumbersPanelSize();
			panelLinesNumbers.Refresh();
			++StatisticsManager.Instance.Statistics.ToggleShowLineNumbersCount;
			StatisticsManager.Instance.Statistics.UpdateStatisticsFromSettings();
		}

		private void NavigationTreeView_Resize(object sender, EventArgs e)
		{
			splitContainer1.Left = 0;
			splitContainer1.Top = 27;
			splitContainer1.Width = this.ClientSize.Width;
			splitContainer1.Height = this.ClientSize.Height - 27;
		}

		private void splitContainer1_Panel1_Resize(object sender, EventArgs e)
		{
			AdjustLineNumbersPanelSize();
		}

		private void treeView1_OnScroll(object sender, CustomTreeView.ScrollEventArgs e)
		{
			if (e.ScrollType == CustomTreeView.ScrollType.Vertical)
			{
				panelLinesNumbers.Refresh();
			}
		}

		private void panelLinesNumbers_Paint(object sender, PaintEventArgs e)
		{
			e.Graphics.FillRectangle(_palette.GetSolidBrush(_colorTable.ControlBackground), panelLinesNumbers.ClientRectangle);

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
				if (_codeProvider != null && Settings.TrackActiveItem)
				{
					int line;
					int column;
					_codeProvider.GetCursorPos(out line, out column);
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
			SettingsForm sf = new SettingsForm(treeView1.Font);
			sf.ShowDialog();
			RefreshTree();
			++StatisticsManager.Instance.Statistics.SettingsDialogShowedCount;
			StatisticsManager.Instance.Statistics.UpdateStatisticsFromSettings();
		}

		private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
		{
			panelLinesNumbers.Refresh();
		}

		private void showHierarhyToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SaveSettings();
			RefreshTree();
			++StatisticsManager.Instance.Statistics.ToggleHierachyOptionCount;
			StatisticsManager.Instance.Statistics.UpdateStatisticsFromSettings();
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
			++StatisticsManager.Instance.Statistics.ExpandAllCommandExecutedCount;
		}

		private void collapseAllNodesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			_treeRefreshing = true;
			treeView1.BeginUpdate();
			treeView1.CollapseAll();
			treeView1.EndUpdate();
			_treeRefreshing = false;
			panelLinesNumbers.Refresh();
			++StatisticsManager.Instance.Statistics.CollapseAllCommandExecutedCount;
		}

		private void treeView1_AfterExpand(object sender, TreeViewEventArgs e)
		{
			_expandedNodesManager.SetExpandedState((CustomTreeNode)e.Node);
			if (Settings.ShowLineNumbersEnabled && !_treeRefreshing)
			{
				panelLinesNumbers.Refresh();
			}
		}

		private void treeView1_AfterCollapse(object sender, TreeViewEventArgs e)
		{
			_expandedNodesManager.SetExpandedState((CustomTreeNode)e.Node);
			if (Settings.ShowLineNumbersEnabled && !_treeRefreshing)
			{
				panelLinesNumbers.Refresh();
			}
		}

		private void sortItemsAlphabeticallyToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SaveSettings();
			RefreshTree();
			++StatisticsManager.Instance.Statistics.SortingUsedCount;
			StatisticsManager.Instance.Statistics.UpdateStatisticsFromSettings();
		}

		private void filterByMarksToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SaveSettings();
			RefreshTree();
			++StatisticsManager.Instance.Statistics.FilterByMarksUsedCount;
			StatisticsManager.Instance.Statistics.UpdateStatisticsFromSettings();
		}

		private void hideAnonymousFunctionsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SaveSettings();
			RefreshTree();
			++StatisticsManager.Instance.Statistics.HideAnonymousFunctionsUsedCount;
			StatisticsManager.Instance.Statistics.UpdateStatisticsFromSettings();
		}

		private void UpdateToDoListToggleImage()
		{
			btnToDoListToggle.Image = Settings.ToDoListCollapsed
				? JsParser.UI.Properties.Resources.navCollapseArrow
				: JsParser.UI.Properties.Resources.navExpandArrow;
		}

		private void UpdateToDoListSettings() 
		{
			if (_userWantsUpdateSplitterPosition)
			{
				if (!Settings.ToDoListCollapsed)
				{
					Settings.ToDoListLastHeight = splitContainer1.Height - splitContainer1.SplitterDistance;
				}
				Settings.ToDoListCollapsed = (splitContainer1.Panel2.Height <= 25);
			}
			UpdateToDoListToggleImage();
		}

		private void SetToDoSplitterPosition()
		{
			if (!Settings.ToDoListCollapsed)
			{
				splitContainer1.SplitterDistance = splitContainer1.Height - Math.Max(100, Settings.ToDoListLastHeight);
			}
			else
			{
				splitContainer1.SplitterDistance = splitContainer1.Height;
			}
			UpdateToDoListToggleImage();
		}

		private void btnToDoListToggle_Click(object sender, EventArgs e)
		{
			_userWantsUpdateSplitterPosition = true;
			UpdateToDoListSettings();   //need to save old position
			Settings.ToDoListCollapsed = !Settings.ToDoListCollapsed;   //update toggle flag
			SetToDoSplitterPosition();  //update ui
		}

		private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
		{
			UpdateToDoListSettings();
			_userWantsUpdateSplitterPosition = false;
		}

		private void splitContainer1_SplitterMoving(object sender, SplitterCancelEventArgs e)
		{
			_userWantsUpdateSplitterPosition = true;
		}

		#endregion

		private void taskListDataGrid_Leave(object sender, EventArgs e)
		{
			taskListDataGrid.CurrentCell = null;
		}
	}
}
