using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using EnvDTE;
using System.IO;
using EnvDTE80;
using System.Resources;
using System.Reflection;
using System.Diagnostics;
using System.Threading;
using System.Globalization;
using JS_addin.Addin.Parsers.JSParser;

namespace JS_addin.Addin
{
	public partial class NavigationTreeView : UserControl
	{
		private string _loadedDocName = string.Empty;

		private DTE m_dte = null;
		public DTE DTE
		{
			set { m_dte = value; }
		}

		public NavigationTreeView()
		{
			InitializeComponent();
		}

		public void Init(DTE dte, Document doc)
		{
			this.m_dte = dte;
			this._doc = doc;
			_code = new CodeService(Doc).LoadCode();
		}

		private Document _doc;

		private Document Doc
		{
			get
			{
				if (_doc == null)
					return m_dte.ActiveDocument;
				return _doc;
			}
		}

		public TextSelection Selection
		{
			get
			{
				return (TextSelection)Doc.Selection;
			}
		}
		private string _code;
		public string Code
		{
			get { return _code; }
		}

		public void LoadFunctionList()
		{
			if (Doc == null || _loadedDocName == Doc.Path + Doc.Name) return;
			_loadedDocName = Doc.Path + Doc.Name;
			lbDocName.Text = Doc.Name;

			listView1.Items.Clear();
			if (Doc.Name.EndsWith(".js"))
			{
				var nodes = JSParser.Parse(Code);
				foreach (var node in nodes)
				{
					ListViewItem it = new ListViewItem(new[] { node.Opcode.ToString(), node.Alias, node.StartLine.ToString() });
					it.Tag = node.StartLine;
					listView1.Items.Add(it);
				}
			}
		}

		public void Clear()
		{
			listView1.Items.Clear();
		}

		private void btnRefresh_Click(object sender, EventArgs e)
		{
			try
			{
				this.Dock = DockStyle.Fill;
				Refresh();
				_code = new CodeService(Doc).LoadCode();
				_loadedDocName = string.Empty;
				LoadFunctionList();
			}
			catch  (Exception ex)
			{
				MessageBox.Show(ex.Message + Environment.NewLine + ex.Source);
			}
		}

		private void listView1_SelectedIndexChanged(object sender, EventArgs e)
		{
			
		}

		private void listView1_DoubleClick(object sender, EventArgs e)
		{
			if (listView1.SelectedItems.Count > 0)
			{
				int line = (int)listView1.SelectedItems[0].Tag;
				try
				{
					Selection.GotoLine(line, false);
					Doc.Activate();
					m_dte.ActiveWindow.SetFocus();
					
				}
				catch { }
			}
		}
	}
}
