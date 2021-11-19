﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JsParser.Core.Code;
using EnvDTE80;
using EnvDTE;
using System.Threading;

namespace JsParser.VsExtension.Infrastructure
{
	public class VS2010CodeProvider : ICodeProvider
	{
		private readonly Document _activeDocument;

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

		public string FullName
		{
			get
			{
				return System.IO.Path.Combine(Path, Name);
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
			//ThreadPool.QueueUserWorkItem((state) =>
			//{
			//    try
			//    {
			//        Doc.Activate();
			//    }
			//    catch { }
			//}, null);
		}

		public void GetCursorPos(out int line, out int column)
		{
			line = -1;
			column = -1;
			try
			{
				var textDocument = (TextDocument)Doc.Object("TextDocument");
				if (textDocument != null)
				{
					line = textDocument.Selection.ActivePoint.Line;
					column = textDocument.Selection.ActivePoint.DisplayColumn;
				}
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
