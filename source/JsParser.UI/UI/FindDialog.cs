using JsParser.Core.Code;
using JsParser.Core.Search;
using JsParser.UI.Helpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace JsParser.UI.UI
{
    public partial class FindDialog : Form
	{
		public string Result { get; set; }
		Func<CodeNode, bool> _callback;
		IEnumerable<CodeNode> _autocompletesource;
		int _activateCounter = 0;
		bool _wantClose = false;

		public FindDialog(IEnumerable<CodeNode> autocompletesource, Func<CodeNode, bool> callback)
		{
			InitializeComponent();
			_callback = callback;
			_autocompletesource = autocompletesource;

			++StatisticsManager.Instance.Statistics.FindFeatureShowCount;
		}

		protected override void WndProc(ref Message m)
		{
			var messageCode = m.Msg;
			if (messageCode == 0x86)
			{
				_activateCounter++;
			}

			if (_activateCounter > 1 && !_wantClose && messageCode != 16)
			{
				Close();
			}

			base.WndProc(ref m);
		}

		private void FindDialog_Deactivate(object sender, EventArgs e)
		{
			Close();
		}

		private void FindDialog_Leave(object sender, EventArgs e)
		{
			Close();
		}

		private void menuItemOnClick(object sender, EventArgs e)
		{
			ToolStripMenuItem mi = (ToolStripMenuItem)sender;
			edTextInput.TextChanged -= edTextInput_TextChanged;
			edTextInput.Text = mi.Text;
			_callback((CodeNode)mi.Tag);
			++StatisticsManager.Instance.Statistics.FindFeatureUsedCount;
			Close();
		}

		private void edTextInput_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == 13)
			{
				var selected = contextMenuStrip1.Items.OfType<ToolStripMenuItem>().FirstOrDefault(i => i.Selected);
				if (selected != null)
				{
					selected.PerformClick();
				}
			} 
			else if (e.KeyChar == 27) 
			{
				Close();
			}
		}

		private void edTextInput_KeyDown(object sender, KeyEventArgs e)
		{
			switch (e.KeyCode)
			{
				case Keys.Down:
				case Keys.Up:
					{
						//get current selected index
						var selIndex = -1;
						for (int i = 0; i < contextMenuStrip1.Items.Count; ++i)
						{
							if (contextMenuStrip1.Items[i].Selected)
							{
								selIndex = i;
								break;
							}
						}

						switch (e.KeyCode)
						{
							case Keys.Down:
								selIndex++;
								break;
							case Keys.Up:
								selIndex--;
								break;
						}

						if (selIndex < 0)
						{
							selIndex = 0;
						}

						if (selIndex >= contextMenuStrip1.Items.Count)
						{
							selIndex = contextMenuStrip1.Items.Count - 1;
						}

						contextMenuStrip1.Items[selIndex].Select();
						e.SuppressKeyPress = true;
					}
					break;
				case Keys.Right:
				case Keys.Left:
					break;
				default:
					break;
			}
		}

		private void edTextInput_TextChanged(object sender, EventArgs e)
		{
			contextMenuStrip1.Items.Clear();
			if (edTextInput.Text.Length >= 2)
			{
				var matches = SearchHelper.GetMatches(_autocompletesource, item => item.Alias, edTextInput.Text)
					.ToList();

				if (matches.Count > 30) 
				{
					matches = matches.Take(30).ToList();
				}

				matches.ForEach(item =>
					{
						ToolStripMenuItem mi = new ToolStripMenuItem(item.Alias
							+ "                 "
							//+ GetFunctionNameTestTransform(item.Alias)
						);
						mi.Click += menuItemOnClick;
						mi.Tag = item;
						mi.ShortcutKeyDisplayString = item.StartLine.ToString();
						contextMenuStrip1.Items.Add(mi);
					});

				if (contextMenuStrip1.Items.Count > 0)
				{
					edTextInput.ForeColor = SystemColors.WindowText;
					contextMenuStrip1.Show(panel1, 0, panel1.ClientSize.Height);
					contextMenuStrip1.Items[0].Select();
					edTextInput.Focus();
				}
				else
				{
					edTextInput.ForeColor = Color.Red;
					contextMenuStrip1.Hide();
				}
			}
		}

		private void FindDialog_FormClosing(object sender, FormClosingEventArgs e)
		{
			_wantClose = true;
		}

		private void label2_Click(object sender, EventArgs e)
		{
			Close();
		}
	}
}
