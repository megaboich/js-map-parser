using System;
using System.Collections.Generic;
using System.Text;

namespace JS_addin.Addin.Parsers
{
	public class CodeNode
	{
		public string Alias { get; set; }
		public int StartLine { get; set; }
		public string Opcode { get; set; }
	}
}
