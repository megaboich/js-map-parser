using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JsParser.Core.Parsers
{
	public class CustomComment
	{
		public int Start { get; set; }
		public int End { get; set; }
		public string Text { get; set; }
		public bool Solid { get; set; }
		public bool Processed { get; set; }
	}
}
