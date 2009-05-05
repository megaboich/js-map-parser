using System;
using System.Collections.Generic;
using System.Text;
using EnvDTE;
using System.IO;
using Microsoft.JScript.Compiler.ParseTree;
using Microsoft.JScript.Compiler;

namespace JS_addin.Addin
{
	public class CodeService
	{
		private Document _doc;

		public CodeService(Document doc)
		{
			this._doc = doc;
		}

		public TextSelection Selection
		{
			get
			{
				if (_doc == null)
					return null;
				else
				{
					TextDocument textDocument = (TextDocument)_doc.Object("TextDocument");
					return textDocument.Selection;
				}
			}
		}

		public string LoadCode()
		{
			if (_doc == null) return string.Empty;
			using (StreamReader reader = new StreamReader(_doc.FullName))
			{
				return reader.ReadToEnd();
			}
		}
	}
}
