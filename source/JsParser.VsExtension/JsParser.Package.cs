﻿using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.ComponentModel.Design;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Shell;
using EnvDTE;
using JsParser.Core.Infrastructure;
using JsParser.UI.Properties;
using JsParser.UI.Infrastructure;
using JsParser.VsExtension.Infrastructure;
using JsParser.VsExtension.UI;

namespace JsParser.VsExtension
{
    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    ///
    /// The minimum requirement for a class to be considered a valid package for Visual Studio
    /// is to implement the IVsPackage interface and register itself with the shell.
    /// This package uses the helper classes defined inside the Managed Package Framework (MPF)
    /// to do it: it derives from the Package class that provides the implementation of the 
    /// IVsPackage interface and uses the registration attributes defined in the framework to 
    /// register itself and its components with the shell.
    /// </summary>
    // This attribute tells the PkgDef creation utility (CreatePkgDef.exe) that this class is
    // a package.
    [PackageRegistration(UseManagedResourcesOnly = true)]
    // This attribute is used to register the informations needed to show the this package
    // in the Help/About dialog of Visual Studio.
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    // This attribute is needed to let the shell know that this package exposes some menus.
    [ProvideMenuResource("Menus.ctmenu", 1)]

    // This attribute registers a tool window exposed by this package.
    [ProvideToolWindow(typeof (JsParserToolWindow))]

    // This attribute triggers load of extension
    [ProvideAutoLoad(VSConstants.UICONTEXT.SolutionExists_string)]
    [ProvideAutoLoad(VSConstants.UICONTEXT.NoSolution_string)]

    [ProvideOptionPage(typeof (OptionsPageCustom), "JavaScript Map Parser Extension", "General", 113, 114, true)]
    [Guid(GuidList.guidJsParserPackagePkgString)]
    public sealed class JsParserPackage : Microsoft.VisualStudio.Shell.Package
    {
        private JsParserService _jsParserService;
        private DocumentEvents _documentEvents;
        private WindowEvents _windowEvents;
        private static JsParserToolWindowManager _jsParserToolWindowManager;

        public static JsParserToolWindowManager JsParserToolWindowManager
        {
            get { return _jsParserToolWindowManager; }
        }

        /// <summary>
        /// Default constructor of the package.
        /// Inside this method you can place any initialization code that does not require 
        /// any Visual Studio service because at this point the package object is created but 
        /// not sited yet inside Visual Studio environment. The place to do all the other 
        /// initialization is the Initialize method.
        /// </summary>
        public JsParserPackage()
        {
            Trace.WriteLine(string.Format(CultureInfo.CurrentCulture, "Entering constructor for: {0}", this.ToString()));
        }

        /// <summary>
        /// This function is called when the user clicks the menu item that shows the 
        /// tool window. See the Initialize method to see how the menu item is associated to 
        /// this function using the OleMenuCommandService service and the MenuCommand class.
        /// </summary>
        private void JsParserMenuCmd(object sender, EventArgs e)
        {
            var wnd = CreateToolWindow<JsParserToolWindow>();
            ShowToolWindow(wnd);
        }

        /// <summary>
        /// This function is called when the user clicks the menu item that shows the 
        /// tool window. See the Initialize method to see how the menu item is associated to 
        /// this function using the OleMenuCommandService service and the MenuCommand class.
        /// </summary>
        private void JsParserMenuFindCmd(object sender, EventArgs e)
        {
            CreateToolWindow<JsParserToolWindow>().FindCommand();
        }

        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initialization code that rely on services provided by VisualStudio.
        /// </summary>
        protected override void Initialize()
        {
            Trace.WriteLine(string.Format(CultureInfo.CurrentCulture, "Entering Initialize() of: {0}", this.ToString()));

            //System.Diagnostics.Debugger.Break();

            // Add our command handlers for menu (commands must exist in the .vsct file)
            var mcs = (IMenuCommandService) GetService(typeof (IMenuCommandService));
            if (mcs != null)
            {
                // Create the command for the tool window
                var toolwndCommandID = new CommandID(GuidList.guidJsParserPackageCmdSet, (int) PkgCmdIDList.cmdJsParser);
                var menuToolWin = new MenuCommand(JsParserMenuCmd, toolwndCommandID);
                mcs.AddCommand(menuToolWin);

                // Create the command for the tool window
                toolwndCommandID = new CommandID(GuidList.guidJsParserPackageCmdSet, (int) PkgCmdIDList.cmdJsParserFind);
                menuToolWin = new MenuCommand(JsParserMenuFindCmd, toolwndCommandID);
                mcs.AddCommand(menuToolWin);
            }

            _jsParserService = new JsParserService(Settings.Default);

            var dte = (DTE) GetService(typeof (DTE));
            Events events = dte.Events;
            _documentEvents = events.get_DocumentEvents(null);
            _windowEvents = events.get_WindowEvents(null);
            _documentEvents.DocumentSaved += DocumentEvents_DocumentSaved;
            _documentEvents.DocumentOpened += DocumentEvents_DocumentOpened;
            _windowEvents.WindowActivated += WindowEvents_WindowActivated;

            _jsParserToolWindowManager = new JsParserToolWindowManager(_jsParserService, () =>
            {
                return FindToolWindow<JsParserToolWindow>();
            });

            base.Initialize();
        }

        private T CreateToolWindow<T>()
            where T : ToolWindowPane
        {
            // Get the instance number 0 of this tool window. This window is single instance so this instance
            // is actually the only one.
            // The last flag is set to true so that if the tool window does not exists it will be created.
            var window = (T) this.FindToolWindow(typeof (T), 0, true);
            if ((null == window) || (null == window.Frame))
            {
                throw new NotSupportedException(Resources.Resources.CanNotCreateWindow);
            }
            return window;
        }

        private T FindToolWindow<T>()
            where T : ToolWindowPane
        {
            var window = (T) this.FindToolWindow(typeof (T), 0, false);
            if ((null == window) || (null == window.Frame))
            {
                return null;
            }
            return window;
        }

        private void ShowToolWindow<T>(T window)
            where T : ToolWindowPane
        {
            IVsWindowFrame windowFrame = (IVsWindowFrame) window.Frame;
            Microsoft.VisualStudio.ErrorHandler.ThrowOnFailure(windowFrame.Show());
        }

        private void WindowEvents_WindowActivated(EnvDTE.Window gotFocus, EnvDTE.Window lostFocus)
        {
            if (gotFocus == null
                || gotFocus.Kind != "Document"
                || gotFocus.Document == null
                || gotFocus.Document.FullName == _jsParserToolWindowManager.ActiveDocFullName)
            {
                return;
            }

            _jsParserToolWindowManager.CallParserForDocument(new VS2010CodeProvider(gotFocus.Document));
        }

        private void DocumentEvents_DocumentSaved(Document doc)
        {
            if (doc.ActiveWindow != null
                && doc.ActiveWindow.Left == 0
                && doc.ActiveWindow.Top == 0)
            {
                //This is invisible doc
                return;
            }

            _jsParserToolWindowManager.CallParserForDocument(new VS2010CodeProvider(doc));
        }

        private void DocumentEvents_DocumentOpened(Document doc)
        {
            _jsParserToolWindowManager.CallParserForDocument(new VS2010CodeProvider(doc));
        }
    }
}
