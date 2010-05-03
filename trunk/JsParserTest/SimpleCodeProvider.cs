using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JsParserCore.Code;
using System.Windows.Forms;

namespace JsParserTest
{
	public class SimpleCodeProvider :ICodeProvider
	{
		private string _name;
		private string _path;
		private RichTextBox _textBox;

		public SimpleCodeProvider(RichTextBox textBox, string path, string name)
		{
			_textBox = textBox;
			_name = name;
			_path = path;
		}

		#region ICodeProvider Members

		public string LoadCode()
		{
			return _textBox.Text;
		}

		public string Path
		{
			get { return _path; }
		}

		public string Name
		{
			get { return _name; }
		}

		public void SelectionMoveToLineAndOffset(int startLine, int startColumn)
		{
			var textLen = _textBox.Lines.Take(startLine - 1).Select(l => l.Length + 1).Aggregate((t, l) => t += l);
			_textBox.Select(textLen + startColumn - 1, 0);
		}

		public void SetFocus()
		{
			_textBox.Focus();
		}

		#endregion
	}
}
