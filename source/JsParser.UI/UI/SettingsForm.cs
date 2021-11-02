using System;
using System.Drawing;
using System.Windows.Forms;

namespace JsParser.UI.UI
{
	public partial class SettingsForm : Form
	{
		public SettingsForm(Font defaultTreeFont)
		{
			InitializeComponent();
			jsParserSettingsControl1.DefaultTreeFont = defaultTreeFont;
			jsParserSettingsControl1.InitSettings();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			jsParserSettingsControl1.SaveSettings();
			Close();
		}

		private void button2_Click(object sender, EventArgs e)
		{
			Close();
		}
	}
}
