using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using JsParser.Core.Code;
using JsParser.Core.Properties;
using JsParser.Core.Helpers;

namespace JsParser.Core.UI
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
			if ((!string.IsNullOrEmpty(edTextInput.Text.Trim())) && edTextInput.Text.Trim() != "*")
			{
				GetMatches(edTextInput.Text).ToList().ForEach(item =>
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

		private IEnumerable<CodeNode> GetMatches(string input)
		{
			var pattern = SplitFunctionName(input).ToList();
			return _autocompletesource
				.Where(item => CompareEntities(pattern, SplitFunctionName(item.Alias).ToList()));
		}

		public static bool CompareEntities(IList<string> pattern, IList<string> fname)
		{
			if (pattern.Count > fname.Count)
			{
				return false;
			}

			bool isMask = false;
			int pIndex = 0, fIndex = 0;
			for (; pIndex < pattern.Count && fIndex < fname.Count; ++fIndex, ++pIndex)
			{
				if (isMask && !fname[fIndex].StartsWith(pattern[pIndex], StringComparison.InvariantCultureIgnoreCase))
				{
					--pIndex;
					continue;
				}

				if (fname[fIndex].StartsWith(pattern[pIndex], StringComparison.InvariantCultureIgnoreCase))
				{
					isMask = false;
					continue;
				}

				if (pattern[pIndex] == "*")
				{
					isMask = true;
					continue;
				}

				return false;
			}

			return pIndex == pattern.Count;
		}

		public static string GetFunctionNameTestTransform(string originalFName)
		{
			return String.Join(" | ", SplitFunctionName(originalFName).ToArray());
		}

		private static IEnumerable<string> SplitByUpperCaseWording(string fname)
		{
			var lastUpperIndex = 0;
			var counter = 0;
			for (int index = 0; index < fname.Length; ++index)
			{
				if (char.IsUpper(fname[index]) ||
					char.IsDigit(fname[index]) ||
					fname[index] == '*' ||
					(index > 0 && fname[index - 1] == '*'))
				{
					var tl = lastUpperIndex;
					lastUpperIndex = index;
					++counter;
					yield return fname.Substring(tl, index - tl);
				}
			}

			yield return fname.Substring(lastUpperIndex, fname.Length - lastUpperIndex);
		}

		public static IEnumerable<string> SplitFunctionName(string fname)
		{
			var openBracket = fname.IndexOf('('); //Extract function name without parameters
			if (openBracket > 0 && openBracket < fname.Length)
			{
				fname = fname.Substring(0, openBracket);
			}

			var res = fname.Split(new[] { '.', '|', '^', '!', '$' }, StringSplitOptions.RemoveEmptyEntries);
			return res
				.SelectMany(r => SplitByUpperCaseWording(r))
				.Where(s => !string.IsNullOrEmpty(s.Trim()));
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
