using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Shell;
using JsParserCore.UI;
using System.Windows.Controls;
using System.Windows.Forms.Integration;
using System.Reflection;
using EnvDTE;

namespace AlexanderBoyko.JsParser_package
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
	[Guid("5c1947b9-a2ea-42cd-8299-f2603a9c033d")]
	public class MyToolWindow : ToolWindowPane
	{
		private DocumentEvents _documentEvents;
		private SolutionEvents _solutionEvents;
		private WindowEvents _windowEvents;
		private DTE _dte;

		public bool TreeLoaded
		{
			get
			{
				if (NavigationTreeView != null)
				{
					return NavigationTreeView.TreeLoaded;
				}
				return false;
			}
		}

		public NavigationTreeView NavigationTreeView
		{
			get
			{
				return (NavigationTreeView)((WindowsFormsHost)(((Panel)(((JsParserHolder)Content).Content)).Children[0])).Child;
			}
		}

		public void FindCommand()
		{
			if (NavigationTreeView != null)
			{
				NavigationTreeView.Find();
			}
		}

		/// <summary>
		/// Standard constructor for the tool window.
		/// </summary>
		public MyToolWindow() :
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

			//Force load of a referenced assembly - this is workaround of bug when sometimes WPF cant load it and throws an exception.
			Assembly.Load(typeof(NavigationTreeView).Assembly.FullName);

			// This is the user control hosted by the tool window; Note that, even if this class implements IDisposable,
			// we are not calling Dispose on this object. This is because ToolWindowPane calls Dispose on 
			// the object returned by the Content property.

			base.Content = new JsParserHolder();
		}

		public override void OnToolWindowCreated()
		{
			var _dte = (DTE)GetService(typeof(DTE));

			try
			{
				if (_dte.ActiveDocument != null)
				{
					var codeProvider = new VS2010CodeProvider(_dte.ActiveDocument);
					NavigationTreeView.Init(codeProvider);
					NavigationTreeView.LoadFunctionList();
				}
			}
			catch
			{
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
			//MessageBox.Show("documentEvents_DocumentClosing");
			if (NavigationTreeView != null)
				NavigationTreeView.Clear();
		}

		/// <summary>
		/// The document events_ document saved.
		/// </summary>
		/// <param name="doc">
		/// The doc.
		/// </param>
		private void documentEvents_DocumentSaved(Document doc)
		{
			//MessageBox.Show("documentEvents_DocumentSaved");
			if (NavigationTreeView != null)
				NavigationTreeView.LoadFunctionList();
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
			//MessageBox.Show("windowEvents_WindowActivated");
			if (gotFocus == null
				|| gotFocus.Kind != "Document"
				|| NavigationTreeView == null
				|| gotFocus.Document == null)
			{
				return;
			}

			try
			{
				var codeProvider = new VS2010CodeProvider(gotFocus.Document);
				NavigationTreeView.Init(codeProvider);
				var treeExist = NavigationTreeView.LoadFunctionList();
				IVsWindowFrame windowFrame = (IVsWindowFrame)Frame;

				if (NavigationTreeView.Settings.ShowHideAutomatically)
				{
					if (!treeExist)
					{
						windowFrame.Hide();
					}
					else
					{
						windowFrame.ShowNoActivate();
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message + Environment.NewLine + ex.Source);
			}
		}
	}
}
