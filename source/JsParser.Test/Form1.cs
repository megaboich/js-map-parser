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
using JsParserTest.Helpers;
using System.IO;

namespace JsParser.Test
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		private void InitTree(RichTextBox textBox)
		{
			navigationTreeView1.Init(new SimpleCodeProvider(textBox, "D:\\FakePath\\", tabControl1.SelectedTab.Text));
			navigationTreeView1.LoadFunctionList();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			var tests = Assembly.GetAssembly(GetType()).GetManifestResourceNames()
                .Where(name => name.EndsWith(".js")
                    || name.EndsWith(".htm")
                    || name.EndsWith(".aspx")
                    )
                .OrderBy(n => n);

			foreach (var testname in tests)
			{
				var nameparts = testname.Split(new[] { '.' });
				var name = nameparts.Skip(nameparts.Count() - 2).Aggregate((a, i) => a += "." + i);
				tabControl1.TabPages.Add(testname, name);
			}

			tabControl1.SelectTab(0);
			tabControl1_TabIndexChanged(sender, e);
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
                var resname = tab.Name;
                var content = TestsHelper.GetEmbeddedText(resname);
                var newTextBox = CreateTextBox(tab, content);
			}
		}

        private RichTextBox CreateTextBox(TabPage tab, string content)
        {
            // Create new text box and load text from resource
            
            var newTextBox = new RichTextBox();
            newTextBox.Name = "richTextBox1";
            newTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            newTextBox.AcceptsTab = true;
            newTextBox.WordWrap = false;
            newTextBox.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            newTextBox.Text = content;
            newTextBox.TextChanged += richTextBox1_TextChanged;
            InitTree((RichTextBox)newTextBox);
            tab.Controls.Add(newTextBox);
            return newTextBox;
        }

		private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
		{

		}

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                var fileName = openFileDialog1.FileName;
                var fileContent = File.ReadAllText(fileName);
                tabControl1.TabPages.Add(Guid.NewGuid().ToString(), Path.GetFileName(fileName));
                var tab = tabControl1.TabPages[tabControl1.TabCount - 1];
                CreateTextBox(tab, fileContent);
                tabControl1.SelectTab(tabControl1.TabCount - 1);
            }
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void scanDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                var path = folderBrowserDialog1.SelectedPath;
                var ff = new Form_ScanDir_Results(path);
                ff.Show();
            }
        }
	}
}
