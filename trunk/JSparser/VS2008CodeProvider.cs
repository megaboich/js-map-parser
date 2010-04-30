using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JsParcerCore.Code;
using EnvDTE80;
using EnvDTE;

namespace JSparser
{
	public class VS2008CodeProvider : ICodeProvider
	{
		private DTE2 _applicationObject;
		private Document _doc;

		public VS2008CodeProvider(DTE2 applicationObject, Document doc)
		{
			_applicationObject = applicationObject;
			_doc = doc;
		}

		#region ICodeProvider Members

		public string LoadCode()
		{
			var textDocument = (TextDocument)_doc.Object("TextDocument");
			return textDocument.CreateEditPoint(textDocument.StartPoint).GetText(textDocument.EndPoint);
		}

		public string Path
		{
			get { return _applicationObject.ActiveDocument.Path; }
		}

		public string Name
		{
			get { return _applicationObject.ActiveDocument.Name; }
		}

		public void SelectionMoveToLineAndOffset(int StartLine, int StartColumn)
		{
			throw new NotImplementedException();

			var textDocument = (TextDocument)_doc.Object("TextDocument");
			textDocument.Selection.MoveToLineAndOffset(StartLine, StartColumn + 1, false);
		}

		public void SetFocus()
		{
			_doc.Activate();
			_applicationObject.ActiveWindow.SetFocus();
		}

		#endregion
	}
}
