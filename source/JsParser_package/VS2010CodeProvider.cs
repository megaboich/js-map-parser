using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JsParserCore.Code;
using EnvDTE80;
using EnvDTE;

namespace AlexanderBoyko.JsParser_package
{
	public class VS2010CodeProvider: ICodeProvider
	{
		private Document _activeDocument;

		public VS2010CodeProvider(Document activeDocument)
		{
			_activeDocument = activeDocument;

			ContainerName = "Visual Studio " + activeDocument.DTE.Version;
		}

		private Document Doc
		{
			get
			{
				return _activeDocument;
			}
		}

		#region ICodeProvider Members

		public string LoadCode()
		{
			try
			{
				var textDocument = (TextDocument)Doc.Object("TextDocument");
				var docContent = textDocument.CreateEditPoint(textDocument.StartPoint).GetText(textDocument.EndPoint);
				return docContent;
			}
			catch
			{
				return "function Error_Loading_Document(){}";
			}
		}

		public string Path
		{
			get { return Doc != null ? Doc.Path : string.Empty; }
		}

		public string Name
		{
			get { return Doc != null ? Doc.Name : string.Empty; }
		}

		public void SelectionMoveToLineAndOffset(int StartLine, int StartColumn)
		{
			try
			{
				var textDocument = (TextDocument)Doc.Object("TextDocument");
				textDocument.Selection.MoveToLineAndOffset(StartLine, StartColumn, false);
			}
			catch
			{
			}
		}

		public void SetFocus()
		{
			if (Doc == null)
			{
				return;
			}

			Doc.Activate();
		}

		public void GetCursorPos(out int line, out int column)
		{
			try
			{
				var textDocument = (TextDocument)Doc.Object("TextDocument");
				line = textDocument.Selection.ActivePoint.Line;
				column = textDocument.Selection.ActivePoint.DisplayColumn;
			}
			catch
			{
				line = -1;
				column = -1;
			}
		}

		#endregion


		public string ContainerName {get; set;}
	}
}
