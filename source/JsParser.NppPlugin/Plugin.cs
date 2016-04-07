using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using JsMapParser.NppPlugin.Forms;
using JsMapParser.NppPlugin.Helpers;

namespace JsMapParser.NppPlugin
{
    partial class PluginBase
    {
        internal const string PluginName = "JavaScript Map Parser";
        internal static int idMenuItemParserUi = -1;
        internal static PluginSettings Settings;
        internal static bool IsDebugMode = false;

        private static frmParserUiContainer _frmParserUiContainer;
        private static readonly JsParserIntegration jsParser = new JsParserIntegration();

        internal static void CommandMenuInit()
        {
            // get path of plug-in configuration
            StringBuilder sbIniFilePath = new StringBuilder(Win32.MAX_PATH);
            Win32.SendMessage(nppData._nppHandle, NppMsg.NPPM_GETPLUGINSCONFIGDIR, Win32.MAX_PATH, sbIniFilePath);

            Settings = new PluginSettings(sbIniFilePath.ToString());

            SetCommand(0, "Show Panel", ShowParserUiPanel, Settings.ShowToolWindow);
            idMenuItemParserUi = 0;

            SetCommand(1, "---", null);

            SetCommand(2, "About", About);

            IsDebugMode = Environment.GetEnvironmentVariable("npp_js_map_parser_addin_debug_mode") == "true";
            if (IsDebugMode)
            {
                SetCommand(3, "Attach Debugger", AttachDebugger);
            }
        }

        public static void OnReady()
        {
            if (Settings.ShowToolWindow)
            {
                ShowParserUiPanel();

                if (Settings.ToolWindowVisible)
                {
                    //Also register timer to show panel once more with delay - so our panel will be likely shown the last
                    TimerHelper.SetTimeOut(ShowThePanel, TimeSpan.FromMilliseconds(100));
                    TimerHelper.SetTimeOut(ShowThePanel, TimeSpan.FromMilliseconds(200));
                    TimerHelper.SetTimeOut(ShowThePanel, TimeSpan.FromMilliseconds(500));
                    TimerHelper.SetTimeOut(ShowThePanel, TimeSpan.FromMilliseconds(1000));
                    TimerHelper.SetTimeOut(ShowThePanel, TimeSpan.FromMilliseconds(1500));
                }
            }
        }

        private static void ShowThePanel()
        {
            Win32.SendMessage(nppData._nppHandle, NppMsg.NPPM_DMMSHOW, 0, _frmParserUiContainer.Handle);
        }

        internal static void SetToolBarIcon()
        {
            var tbIcons = new toolbarIcons();
            tbIcons.hToolbarBmp = Resources.Resources.jsparsericon.GetHbitmap();
            var pTbIcons = Marshal.AllocHGlobal(Marshal.SizeOf(tbIcons));
            Marshal.StructureToPtr(tbIcons, pTbIcons, false);
            Win32.SendMessage(nppData._nppHandle, NppMsg.NPPM_ADDTOOLBARICON, _funcItems.Items[idMenuItemParserUi]._cmdID, pTbIcons);
            Marshal.FreeHGlobal(pTbIcons);
        }

        internal static void PluginCleanUp()
        {
            Settings.ToolWindowVisible = _frmParserUiContainer != null && _frmParserUiContainer.Visible;
            Settings.Save();
        }

        private static void About()
        {
            Process.Start("https://github.com/megaboich/js-map-parser");
        }

        private static void AttachDebugger()
        {
            Debugger.Launch();
            Debugger.Break();
        }

        private static void ShowParserUiPanel()
        {
            if (_frmParserUiContainer == null)
            {
                _frmParserUiContainer = new frmParserUiContainer();
                var tbIcon = ToolsHelper.GetTransparentIcon(Resources.Resources.jsparsericon, 16, 16);
                var nppTbData = new NppTbData();
                nppTbData.hClient = _frmParserUiContainer.Handle;
                nppTbData.pszName = PluginName;
                nppTbData.dlgID = idMenuItemParserUi;
                // define the default docking behavior
                nppTbData.uMask = NppTbMsg.DWS_DF_CONT_RIGHT | NppTbMsg.DWS_ICONTAB | NppTbMsg.DWS_ICONBAR;
                nppTbData.hIconTab = (uint) tbIcon.Handle;
                nppTbData.pszModuleName = PluginName;
                var ptrNppTbData = Marshal.AllocHGlobal(Marshal.SizeOf(nppTbData));
                Marshal.StructureToPtr(nppTbData, ptrNppTbData, false);

                Win32.SendMessage(nppData._nppHandle, NppMsg.NPPM_DMMREGASDCKDLG, 0, ptrNppTbData);

                // Following message will toggle both menu item state and toolbar button
                Win32.SendMessage(
                    nppData._nppHandle, 
                    NppMsg.NPPM_SETMENUITEMCHECK,
                    _funcItems.Items[idMenuItemParserUi]._cmdID, 
                    1);

                Settings.ShowToolWindow = true;
            }
            else
            {
                if (!_frmParserUiContainer.Visible)
                {
                    Win32.SendMessage(nppData._nppHandle, NppMsg.NPPM_DMMSHOW, 0, _frmParserUiContainer.Handle);
                    Win32.SendMessage(nppData._nppHandle, NppMsg.NPPM_SETMENUITEMCHECK,
                        _funcItems.Items[idMenuItemParserUi]._cmdID, 1);
                    Settings.ShowToolWindow = true;
                }
                else
                {
                    Win32.SendMessage(nppData._nppHandle, NppMsg.NPPM_DMMHIDE, 0, _frmParserUiContainer.Handle);
                    Win32.SendMessage(nppData._nppHandle, NppMsg.NPPM_SETMENUITEMCHECK,
                        _funcItems.Items[idMenuItemParserUi]._cmdID, 0);
                    Settings.ShowToolWindow = false;
                }
            }

            jsParser.InitUi(_frmParserUiContainer);
            var fileName = GetCurrentFile();
            jsParser.UpdateTree(fileName, true);
        }

        public static void OnFileSaved()
        {
            var fileName = GetCurrentFile();
            jsParser.UpdateTree(fileName);
        }

        public static void OnFileChanged()
        {
            var fileName = GetCurrentFile();
            jsParser.UpdateTree(fileName);
        }
    }
}