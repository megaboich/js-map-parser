using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using JsParserCore.Properties;
using JsParserCore.Helpers;

namespace JsParserCore.UI
{
	public partial class SettingsForm : Form
	{
		public SettingsForm(Font defaultTreeFont)
		{
			InitializeComponent();
			jsParserSettingsControl1.DefaultFont = defaultTreeFont;
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
