using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using JsParser.Core.Properties;
using JsParser.Core.Helpers;

namespace JsParser.Core.UI
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
