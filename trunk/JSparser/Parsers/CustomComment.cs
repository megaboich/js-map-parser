﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JS_addin.Addin.Parsers
{
	internal class CustomComment
	{
		public int Start { get; set; }
		public int End { get; set; }
		public string Text { get; set; }
		public bool Solid { get; set; }
		public bool Processed { get; set; }
	}
}