using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JsParcerCore.Code;
using System.Windows.Forms;

namespace JsParserTest
{
	public class SimpleCodeProvider :ICodeProvider
	{
		private RichTextBox _textBox;

		public SimpleCodeProvider(RichTextBox textBox)
		{
			_textBox = textBox;
		}

		#region ICodeProvider Members

		public string LoadCode()
		{
			return _textBox.Text;
		}

		public string Path
		{
			get { return "Test path"; }
		}

		public string Name
		{
			get { return "Test doc"; }
		}

		public void SelectionMoveToLineAndOffset(int startLine, int startColumn)
		{

			var textLen = _textBox.Lines.Take(startLine - 1).Select(l => l.Length).Aggregate((t, l) => t += l);
			_textBox.Select(textLen + startColumn, 0);
		}

		public void SetFocus()
		{
			_textBox.Focus();
		}

		#endregion
	}
}
