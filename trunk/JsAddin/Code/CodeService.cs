using System.IO;
using EnvDTE;

namespace JS_addin.Addin.Code
{
	/// <summary>
	/// The code service.
	/// </summary>
	public class CodeService
	{
		private readonly Document _doc;

		/// <summary>
		/// Initializes a new instance of the <see cref="CodeService"/> class.
		/// </summary>
		/// <param name="doc">
		/// The doc parameter.
		/// </param>
		public CodeService(Document doc)
		{
			_doc = doc;
		}

		/// <summary>
		/// Gets Selection.
		/// </summary>
		public TextSelection Selection
		{
			get
			{
				if (_doc == null)
				{
					return null;
				}
				
				var textDocument = (TextDocument)_doc.Object("TextDocument");
				return textDocument.Selection;
			}
		}

		/// <summary>
		/// The load code method.
		/// </summary>
		/// <returns>
		/// The load code.
		/// </returns>
		public string LoadCode()
		{
			if (_doc == null)
			{
				return string.Empty;
			}

			using (var reader = new StreamReader(_doc.FullName))
			{
				return reader.ReadToEnd();
			}
		}
	}
}