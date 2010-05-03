using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using UnitTests;

namespace JsParserTest
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
			InitTree(richTextBox1);
		}

		private void InitTree(RichTextBox textBox)
		{
			navigationTreeView1.Init(new SimpleCodeProvider(textBox, "D:\\FakePath\\", tabControl1.SelectedTab.Text));
			navigationTreeView1.LoadFunctionList();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			var tests = Assembly.GetAssembly(GetType()).GetManifestResourceNames().Where(name => name.EndsWith(".js"));
			foreach (var testname in tests)
			{
				var nameparts = testname.Split(new[] { '.' });
				var name = nameparts.Skip(nameparts.Count() - 2).Aggregate((a, i) => a += "." + i);
				tabControl1.TabPages.Add(testname, name);
			}
		}

		private void richTextBox1_TextChanged(object sender, EventArgs e)
		{
			navigationTreeView1.LoadFunctionList();
		}

		private void tabControl1_TabIndexChanged(object sender, EventArgs e)
		{
			var tab = tabControl1.SelectedTab;
			var textBox = tab.Controls.Find("richTextBox1", false).FirstOrDefault();

			if (textBox != null)
			{
				InitTree((RichTextBox)textBox);
			}
			else
			{
				// Create new text box and load text from resource
				var resname = tab.Name;
				var newTextBox = new RichTextBox();
				newTextBox.Name = "richTextBox1";
				newTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
				newTextBox.AcceptsTab = true;
				newTextBox.WordWrap = false;
				newTextBox.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
				newTextBox.Text = AutoTester.GetEmbeddedText(resname);
				newTextBox.TextChanged += richTextBox1_TextChanged;
				tab.Controls.Add(newTextBox);
				InitTree((RichTextBox)newTextBox);
			}
		}

		private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
		{

		}
	}
}
