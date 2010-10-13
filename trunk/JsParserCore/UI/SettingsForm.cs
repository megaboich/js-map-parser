using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using JsParserCore.Properties;

namespace JsParserCore.UI
{
	public partial class SettingsForm : Form
	{
		public SettingsForm()
		{
			InitializeComponent();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			Settings.Default.TrackActiveItem = chTrackActiveItem.Checked;
			Settings.Default.Extensions = new System.Collections.Specialized.StringCollection();
			Settings.Default.Extensions.AddRange(edExtensions.Lines);
			Settings.Default.Save();
			Close();
		}

		private void SettingsForm_Load(object sender, EventArgs e)
		{
			chTrackActiveItem.Checked = Settings.Default.TrackActiveItem;
			edExtensions.Lines = Settings.Default.Extensions.OfType<string>().ToArray();
		}
	}
}
