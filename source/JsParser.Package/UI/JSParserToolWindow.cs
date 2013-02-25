using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Shell;
using JsParser.UI.UI;
using System.Windows.Controls;
using System.Windows.Forms.Integration;
using System.Reflection;
using EnvDTE;
using System.Diagnostics;
using JsParser.Core.Helpers;
using Microsoft.VisualStudio.TextManager.Interop;
using JsParser.UI;
using JsParser.Core.Code;
using JsParser.Package.Infrastructure;
using JsParser.UI.Infrastructure;

namespace JsParser.Package.UI
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
    [Guid(GuidList.guidToolWindowPersistanceString)]
    public class JsParserToolWindow : ToolWindowPane, IJsParserToolWindow
    {
        private NavigationTreeView _navigationTreeView;

        public NavigationTreeView NavigationTreeView
        {
            get
            {
                return _navigationTreeView;
            }
        }

        /// <summary>
        /// Standard constructor for the tool window.
        /// </summary>
        public JsParserToolWindow() :
            base(null)
        {
            // Set the window title reading it from the resources.
            this.Caption = Resources.Resources.ToolWindowTitle;
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

            base.Content = new JsParserNavTreeHolder();

            _navigationTreeView = (NavigationTreeView)((WindowsFormsHost)(((Panel)(((JsParserNavTreeHolder)Content).Content)).Children[0])).Child;

        }

        public override void OnToolWindowCreated()
        {
            if (JsParserPackage.JsParserToolWindowManager != null)
            {
                JsParserPackage.JsParserToolWindowManager.PerformInitialParsing(_navigationTreeView);
            }
            
            base.OnToolWindowCreated();
        }

        public void FindCommand()
        {
            _navigationTreeView.Find();
        }
    }
}
