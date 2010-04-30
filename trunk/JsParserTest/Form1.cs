using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace JsParserTest
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
			var codeProvider = new SimpleCodeProvider(richTextBox1);
			navigationTreeView1.Init(codeProvider);
		}

		private void Form1_Load(object sender, EventArgs e)
		{

		}
	}
}
