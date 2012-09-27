using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Shell;
using JsParser.Core.UI;
using System.Windows.Controls;
using System.Windows.Forms.Integration;
using System.Reflection;
using EnvDTE;
using System.Diagnostics;
using JsParser.Core.Helpers;
using JsParser.Core.UI.Xaml;

namespace JsParser.Package
{
	/// <summary>
	/// This class implements the tool window exposed by this package and hosts a user control.
	///
	/// In Visual Studio tool windows are composed of a frame (implemented by the shell) and a pane, 
	/// usually implemented by the package implementer.
	///
	/// This class derives from the ToolWindowPane class provided from the MPF in order to use its 
	/// implementation of the IVsUIElementPane interface.
	/// </summary>
    [Guid(GuidList.guidJsParserTreeToolWindow)]
	public class JsParserTreeToolWindow : ToolWindowPane
	{
		private DocumentEvents _documentEvents;
		private SolutionEvents _solutionEvents;
		private WindowEvents _windowEvents;
		private VS2010UIThemeProvider _uiVS2010UIThemeProvider;

		/// <summary>
		/// Standard constructor for the tool window.
		/// </summary>
        public JsParserTreeToolWindow() :
			base(null)
		{
			// Set the window title reading it from the resources.
			this.Caption = Resources.ToolWindowTitle;
			// Set the image that will appear on the tab of the window frame
			// when docked with an other window
			// The resource ID correspond to the one defined in the resx file
			// while the Index is the offset in the bitmap strip. Each image in
			// the strip being 16x16.
			this.BitmapResourceID = 301;
			this.BitmapIndex = 0;

            // This is the user control hosted by the tool window; Note that, even if this class implements IDisposable,
			// we are not calling Dispose on this object. This is because ToolWindowPane calls Dispose on 
			// the object returned by the Content property.

            base.Content = new JsParserToolWindow();

			_uiVS2010UIThemeProvider = new VS2010UIThemeProvider(GetService);
		}

		public override void OnToolWindowCreated()
		{
			Trace.WriteLine("js addin: OnToolWindowCreated");
			var _dte = (DTE)GetService(typeof(DTE));

			try
			{
				
			}
			catch (Exception ex)
			{
				ErrorHandler.WriteExceptionDetailsToTrace("OnToolWindowCreated", ex);
			}

			Events events = _dte.Events;
			_documentEvents = events.get_DocumentEvents(null);
			_solutionEvents = events.SolutionEvents;
			_windowEvents = events.get_WindowEvents(null);

			_documentEvents.DocumentClosing += documentEvents_DocumentClosing;
			_documentEvents.DocumentSaved += documentEvents_DocumentSaved;
			_windowEvents.WindowActivated += windowEvents_WindowActivated;

			base.OnToolWindowCreated();
		}

		/// <summary>
		/// The document events_ document closing.
		/// </summary>
		/// <param name="doc">
		/// The doc.
		/// </param>
		private void documentEvents_DocumentClosing(Document doc)
		{
			Trace.WriteLine("js addin: documentEvents_DocumentClosing");
		}

		/// <summary>
		/// The document events_ document saved.
		/// </summary>
		/// <param name="doc">
		/// The doc.
		/// </param>
		private void documentEvents_DocumentSaved(Document doc)
		{
			Trace.WriteLine("js addin: documentEvents_DocumentSaved");
		}

		/// <summary>
		/// The window events_ window activated.
		/// </summary>
		/// <param name="gotFocus">
		/// The got focus.
		/// </param>
		/// <param name="LostFocus">
		/// The lost focus.
		/// </param>
		private void windowEvents_WindowActivated(EnvDTE.Window gotFocus, EnvDTE.Window lostFocus)
		{
			Trace.WriteLine("js addin: windowEvents_WindowActivated");
		}
	}
}
