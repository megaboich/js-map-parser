using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.JScript.Compiler;
using Microsoft.JScript.Compiler.ParseTree;
using JsParserCore.Code;
using JsParserCore.Helpers;
using System.Reflection;
using System.Diagnostics;

namespace JsParserCore.Parsers
{
	/// <summary>
	/// The js parser settings.
	/// </summary>
	public class JavascriptParserSettings
	{
		public JavascriptParserSettings()
		{
			MaxParametersLengthInFunctionChain = 25;
			MaxParametersLength = 25;
			ProcessHierarchy = true;
		}

		public int MaxParametersLengthInFunctionChain { get; set; }

		public int MaxParametersLength { get; set; }

		/// <summary>
		/// Gets or sets Flag indicating that parser will additionally process hierarchy.
		/// </summary>
		public bool ProcessHierarchy { get; set; }
	}
}