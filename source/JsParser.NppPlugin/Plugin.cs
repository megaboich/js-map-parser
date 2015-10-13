using System;
using System.IO;
using System.Text;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace NppPluginNET
{
    partial class PluginBase
    {
        internal const string PluginName = "Javascript Map Parser";   
        
        static internal int idMenuItemParserUi = -1;
        static Bitmap tbBmp = Properties.Resources.star;
        static Bitmap tbBmp_tbTab = Properties.Resources.star_bmp;
        static Icon tbIcon = null;

        private static frmParserUiContainer _frmParserUiContainer = null;
        private static JsParserIntegration jsParser = new JsParserIntegration();
        
        static internal void CommandMenuInit()
        {
            SetCommand(0, "Show Javascript Map Parser UI Panel", ShowParserUiPanel);
            idMenuItemParserUi = 0;

            SetCommand(1, "---", null);

            SetCommand(2, "About", About);

            SetCommand(3, "Attach Debugger", AttachDebugger);
        }
        static internal void SetToolBarIcon()
        {
            toolbarIcons tbIcons = new toolbarIcons();
            tbIcons.hToolbarBmp = tbBmp.GetHbitmap();
            IntPtr pTbIcons = Marshal.AllocHGlobal(Marshal.SizeOf(tbIcons));
            Marshal.StructureToPtr(tbIcons, pTbIcons, false);
            Win32.SendMessage(nppData._nppHandle, NppMsg.NPPM_ADDTOOLBARICON, _funcItems.Items[idMenuItemParserUi]._cmdID, pTbIcons);
            Marshal.FreeHGlobal(pTbIcons);
        }
        static internal void PluginCleanUp()
        {
        }
        
        static void About()
        {
            System.Diagnostics.Process.Start("https://github.com/megaboich/jsparser");
        }

        static void AttachDebugger()
        {
            Debugger.Launch();
            Debugger.Break();
        }

        static void ShowParserUiPanel()
        {
            if (_frmParserUiContainer == null)
            {
                _frmParserUiContainer = new frmParserUiContainer();

                using (Bitmap newBmp = new Bitmap(16, 16))
                {
                    Graphics g = Graphics.FromImage(newBmp);
                    ColorMap[] colorMap = new ColorMap[1];
                    colorMap[0] = new ColorMap();
                    colorMap[0].OldColor = Color.Fuchsia;
                    colorMap[0].NewColor = Color.FromKnownColor(KnownColor.ButtonFace);
                    ImageAttributes attr = new ImageAttributes();
                    attr.SetRemapTable(colorMap);
                    g.DrawImage(tbBmp_tbTab, new Rectangle(0, 0, 16, 16), 0, 0, 16, 16, GraphicsUnit.Pixel, attr);
                    tbIcon = Icon.FromHandle(newBmp.GetHicon());
                }
                
                NppTbData _nppTbData = new NppTbData();
                _nppTbData.hClient = _frmParserUiContainer.Handle;
                _nppTbData.pszName = "Js Map Parser";
                // the dlgDlg should be the index of funcItem where the current function pointer is in
                // this case is 15.. so the initial value of funcItem[15]._cmdID - not the updated internal one !
                _nppTbData.dlgID = idMenuItemParserUi;
                // define the default docking behaviour
                _nppTbData.uMask = NppTbMsg.DWS_DF_CONT_RIGHT | NppTbMsg.DWS_ICONTAB | NppTbMsg.DWS_ICONBAR;
                _nppTbData.hIconTab = (uint)tbIcon.Handle;
                _nppTbData.pszModuleName = PluginName;
                IntPtr _ptrNppTbData = Marshal.AllocHGlobal(Marshal.SizeOf(_nppTbData));
                Marshal.StructureToPtr(_nppTbData, _ptrNppTbData, false);

                Win32.SendMessage(nppData._nppHandle, NppMsg.NPPM_DMMREGASDCKDLG, 0, _ptrNppTbData);
                // Following message will toogle both menu item state and toolbar button
                Win32.SendMessage(nppData._nppHandle, NppMsg.NPPM_SETMENUITEMCHECK, _funcItems.Items[idMenuItemParserUi]._cmdID, 1);
            }
            else
            {
                if (!_frmParserUiContainer.Visible)
                {
                    Win32.SendMessage(nppData._nppHandle, NppMsg.NPPM_DMMSHOW, 0, _frmParserUiContainer.Handle);
                    Win32.SendMessage(nppData._nppHandle, NppMsg.NPPM_SETMENUITEMCHECK, _funcItems.Items[idMenuItemParserUi]._cmdID, 1);
                }
                else
                {
                    Win32.SendMessage(nppData._nppHandle, NppMsg.NPPM_DMMHIDE, 0, _frmParserUiContainer.Handle);
                    Win32.SendMessage(nppData._nppHandle, NppMsg.NPPM_SETMENUITEMCHECK, _funcItems.Items[idMenuItemParserUi]._cmdID, 0);
                }
            }

            jsParser.InitUi(_frmParserUiContainer);
            string fileName = PluginBase.GetCurrentFile();
            jsParser.UpdateTree(fileName, ignoreCache: true);
        }
        
        public static void OnFileSaved()
        {
            string fileName = PluginBase.GetCurrentFile();
            jsParser.UpdateTree(fileName);
        }

        public static void OnFileChanged()
        {
            string fileName = PluginBase.GetCurrentFile();
            jsParser.UpdateTree(fileName);
        }
    }
}   
