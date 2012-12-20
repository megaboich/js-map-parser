using System;
using System.Linq;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.ComponentModel.Design;
using Microsoft.Win32;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using EnvDTE;
using JsParser.UI.UI;
using System.Windows;
using System.IO;
using JsParser.Package.UI;
using JsParser.UI.Services;
using JsParser.Package.Infrastructure;
using EnvDTE80;
using Microsoft.VisualStudio.Platform.WindowManagement;

namespace JsParser.Package
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
    [ProvideToolWindow(typeof(JsParserToolWindow))]

    // This attribute triggers load of extension
    [ProvideAutoLoad(VSConstants.UICONTEXT.SolutionExists_string)]
    [ProvideAutoLoad(VSConstants.UICONTEXT.NoSolution_string)]
    

    [ProvideOptionPageAttribute(typeof(OptionsPageCustom), "Javascript Parser Extension", "General", 113, 114, true)]
    [Guid(GuidList.guidJsParserPackagePkgString)]
    public sealed class JsParserPackage : Microsoft.VisualStudio.Shell.Package
    {
        private static JsParserService _jsParserService;
        private DocumentEvents _documentEvents;
        private SolutionEvents _solutionEvents;
        private WindowEvents _windowEvents;
        private string _activeDocName;
        private IUIThemeProvider _uiVS2010UIThemeProvider;

        public static JsParserService JsParserService
        {
            get
            {
                return _jsParserService;
            }
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

        /////////////////////////////////////////////////////////////////////////////
        // Overriden Package Implementation
        #region Package Members

        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initilaization code that rely on services provided by VisualStudio.
        /// </summary>
        protected override void Initialize()
        {
            Trace.WriteLine(string.Format(CultureInfo.CurrentCulture, "Entering Initialize() of: {0}", this.ToString()));

            //System.Diagnostics.Debugger.Break();

            // Add our command handlers for menu (commands must exist in the .vsct file)
            var mcs = (IMenuCommandService)GetService(typeof(IMenuCommandService));
            if (mcs != null)
            {
                // Create the command for the tool window
                var toolwndCommandID = new CommandID(GuidList.guidJsParserPackageCmdSet, (int)PkgCmdIDList.cmdJsParser);
                var menuToolWin = new MenuCommand(JsParserMenuCmd, toolwndCommandID);
                mcs.AddCommand(menuToolWin);

                // Create the command for the tool window
                toolwndCommandID = new CommandID(GuidList.guidJsParserPackageCmdSet, (int)PkgCmdIDList.cmdJsParserFind);
                menuToolWin = new MenuCommand(JsParserMenuFindCmd, toolwndCommandID);
                mcs.AddCommand(menuToolWin);
            }

            _jsParserService = new JsParserService();

            var dte = (DTE)GetService(typeof(DTE));
            Events events = dte.Events;
            _documentEvents = events.get_DocumentEvents(null);
            _solutionEvents = events.SolutionEvents;
            _windowEvents = events.get_WindowEvents(null);
            _documentEvents.DocumentSaved += documentEvents_DocumentSaved;
            _documentEvents.DocumentOpened += documentEvents_DocumentOpened;
            _windowEvents.WindowActivated += windowEvents_WindowActivated;
            _uiVS2010UIThemeProvider = new VS2010UIThemeProvider(GetService);

            base.Initialize();
        }
        #endregion

        private T CreateToolWindow<T>()
            where T : ToolWindowPane
        {
            // Get the instance number 0 of this tool window. This window is single instance so this instance
            // is actually the only one.
            // The last flag is set to true so that if the tool window does not exists it will be created.
            var window = (T)this.FindToolWindow(typeof(T), 0, true);
            if ((null == window) || (null == window.Frame))
            {
                throw new NotSupportedException(Resources.Resources.CanNotCreateWindow);
            }
            return window;
        }

        private T FindToolWindow<T>()
             where T : ToolWindowPane
        {
            var window = (T)this.FindToolWindow(typeof(T), 0, false);
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

        #region DTE Events
        private void windowEvents_WindowActivated(EnvDTE.Window gotFocus, EnvDTE.Window lostFocus)
        {
            if (gotFocus == null
                || gotFocus.Kind != "Document"
                || gotFocus.Document == null
                || gotFocus.Document.FullName == _activeDocName)
            {
                return;
            }

            CallParserForDocument(gotFocus.Document);
        }

        private void documentEvents_DocumentSaved(Document doc)
        {
            CallParserForDocument(doc);
        }

        private void documentEvents_DocumentOpened(Document doc)
        {
            CallParserForDocument(doc);
        }
        #endregion

        private void CallParserForDocument(Document doc)
        {
            try
            {
                _activeDocName = doc.FullName;
                var codeProvider = new VS2010CodeProvider(doc);

                var result = _jsParserService.Process(codeProvider);

                var toolWindow = FindToolWindow<JsParserToolWindow>();

                if (result == null)
                {
                    // Not JS case - need to clean tree
                    if (toolWindow != null)
                    {
                        toolWindow.NavigationTreeView.Clear();
                    }

                    return;
                }

                if (result.FileName == string.Empty)
                {
                    // skip - cached result
                    return;
                }

                ErrorNotificationCommunicator.FireActionsForDoc(
                    _activeDocName, 
                    new ErrorsNotificationArgs {FullFileName = _activeDocName, Errors = result.Errors});

                if (toolWindow != null)
                {
                    NotifyColorChangeToToolWindow();
                    toolWindow.NavigationTreeView.UpdateTree(result, codeProvider);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + Environment.NewLine + ex.Source);
            }
        }

        private void NotifyColorChangeToToolWindow()
        {
            var toolWindow = FindToolWindow<JsParserToolWindow>();
            if (toolWindow != null)
            {
                toolWindow.NavigationTreeView.InitColors(_uiVS2010UIThemeProvider);
            }
        }
    }
}
