using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using NppPluginNET.Properties;

namespace NppPluginNET
{
    partial class PluginBase
    {
        internal const string PluginName = "Javascript Map Parser";
        internal static int idMenuItemParserUi = -1;
        internal static PluginSettings Settings;

        private static frmParserUiContainer _frmParserUiContainer;
        private static readonly JsParserIntegration jsParser = new JsParserIntegration();

        internal static void CommandMenuInit()
        {
            // get path of plugin configuration
            StringBuilder sbIniFilePath = new StringBuilder(Win32.MAX_PATH);
            Win32.SendMessage(nppData._nppHandle, NppMsg.NPPM_GETPLUGINSCONFIGDIR, Win32.MAX_PATH, sbIniFilePath);

            Settings = new PluginSettings(sbIniFilePath.ToString());

            
            SetCommand(0, "Show Javascript Map Parser UI Panel", ShowParserUiPanel);
            idMenuItemParserUi = 0;

            SetCommand(1, "---", null);

            SetCommand(2, "About", About);

            //SetCommand(3, "Attach Debugger", AttachDebugger);

            if (Settings.ShowToolWindow)
            {
                ShowParserUiPanel();
            }
        }

        internal static void SetToolBarIcon()
        {
            var tbIcons = new toolbarIcons();
            tbIcons.hToolbarBmp = Resources.star.GetHbitmap();
            var pTbIcons = Marshal.AllocHGlobal(Marshal.SizeOf(tbIcons));
            Marshal.StructureToPtr(tbIcons, pTbIcons, false);
            Win32.SendMessage(nppData._nppHandle, NppMsg.NPPM_ADDTOOLBARICON,
                _funcItems.Items[idMenuItemParserUi]._cmdID, pTbIcons);
            Marshal.FreeHGlobal(pTbIcons);
        }

        internal static void PluginCleanUp()
        {
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
                Icon tbIcon;
                using (var newBmp = new Bitmap(16, 16))
                {
                    var g = Graphics.FromImage(newBmp);
                    var colorMap = new ColorMap[1];
                    colorMap[0] = new ColorMap();
                    colorMap[0].OldColor = Color.Fuchsia;
                    colorMap[0].NewColor = Color.FromKnownColor(KnownColor.ButtonFace);
                    var attr = new ImageAttributes();
                    attr.SetRemapTable(colorMap);
                    g.DrawImage(Resources.star, new Rectangle(0, 0, 16, 16), 0, 0, 16, 16, GraphicsUnit.Pixel, attr);
                    tbIcon = Icon.FromHandle(newBmp.GetHicon());
                }

                var nppTbData = new NppTbData();
                nppTbData.hClient = _frmParserUiContainer.Handle;
                nppTbData.pszName = PluginName;
                // the dlgDlg should be the index of funcItem where the current function pointer is in
                // this case is 15.. so the initial value of funcItem[15]._cmdID - not the updated internal one !
                nppTbData.dlgID = idMenuItemParserUi;
                // define the default docking behaviour
                nppTbData.uMask = NppTbMsg.DWS_DF_CONT_RIGHT | NppTbMsg.DWS_ICONTAB | NppTbMsg.DWS_ICONBAR;
                nppTbData.hIconTab = (uint) tbIcon.Handle;
                nppTbData.pszModuleName = PluginName;
                var ptrNppTbData = Marshal.AllocHGlobal(Marshal.SizeOf(nppTbData));
                Marshal.StructureToPtr(nppTbData, ptrNppTbData, false);

                Win32.SendMessage(nppData._nppHandle, NppMsg.NPPM_DMMREGASDCKDLG, 0, ptrNppTbData);
                // Following message will toogle both menu item state and toolbar button
                Win32.SendMessage(nppData._nppHandle, NppMsg.NPPM_SETMENUITEMCHECK,
                    _funcItems.Items[idMenuItemParserUi]._cmdID, 1);
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