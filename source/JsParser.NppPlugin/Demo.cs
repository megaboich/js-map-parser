using System;
using System.IO;
using System.Text;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace NppPluginNET
{
    partial class PluginBase
    {
        #region " Fields "
        internal const string PluginName = "Javascript Map Parser";
        static string iniFilePath = null;
        static string sectionName = "Insert Extension";
        static string keyName = "doCloseTag";
        static bool doCloseTag = false;
        static string sessionFilePath = @"C:\text.session";
        static frmParserUiContainer _frmParserUiContainer = null;
        static internal int idFrmGotToLine = -1;
        static Bitmap tbBmp = Properties.Resources.star;
        static Bitmap tbBmp_tbTab = Properties.Resources.star_bmp;
        static Icon tbIcon = null;
        #endregion

        #region " Startup/CleanUp "
        static internal void CommandMenuInit()
        {
            // Initialization of your plugin commands
            // You should fill your plugins commands here
 
        	//
	        // Firstly we get the parameters from your plugin config file (if any)
	        //

	        // get path of plugin configuration
            StringBuilder sbIniFilePath = new StringBuilder(Win32.MAX_PATH);
            Win32.SendMessage(nppData._nppHandle, NppMsg.NPPM_GETPLUGINSCONFIGDIR, Win32.MAX_PATH, sbIniFilePath);
            iniFilePath = sbIniFilePath.ToString();

	        // if config path doesn't exist, we create it
            if (!Directory.Exists(iniFilePath))
	        {
                Directory.CreateDirectory(iniFilePath);
	        }

	        // make your plugin config file full file path name
            iniFilePath = Path.Combine(iniFilePath, PluginName + ".ini");

	        // get the parameter value from plugin config
	        doCloseTag = (Win32.GetPrivateProfileInt(sectionName, keyName, 0, iniFilePath) != 0);

            // with function :
            // SetCommand(int index,                            // zero based number to indicate the order of command
            //            string commandName,                   // the command name that you want to see in plugin menu
            //            NppFuncItemDelegate functionPointer,  // the symbol of function (function pointer) associated with this command. The body should be defined below. See Step 4.
            //            ShortcutKey *shortcut,                // optional. Define a shortcut to trigger this command
            //            bool check0nInit                      // optional. Make this menu item be checked visually
            //            );
           
            SetCommand(0, "Show Javascript Map Parser UI Panel", ShowParserUiPanel);
            idFrmGotToLine = 0;

            SetCommand(1, "---", null);

            SetCommand(0, "About", About);
        }
        static internal void SetToolBarIcon()
        {
            toolbarIcons tbIcons = new toolbarIcons();
            tbIcons.hToolbarBmp = tbBmp.GetHbitmap();
            IntPtr pTbIcons = Marshal.AllocHGlobal(Marshal.SizeOf(tbIcons));
            Marshal.StructureToPtr(tbIcons, pTbIcons, false);
            Win32.SendMessage(nppData._nppHandle, NppMsg.NPPM_ADDTOOLBARICON, _funcItems.Items[idFrmGotToLine]._cmdID, pTbIcons);
            Marshal.FreeHGlobal(pTbIcons);
        }
        static internal void PluginCleanUp()
        {
	        Win32.WritePrivateProfileString(sectionName, keyName, doCloseTag ? "1" : "0", iniFilePath);
        }
        #endregion

        #region " Menu functions "

        static void About()
        {
            System.Diagnostics.Process.Start("https://github.com/megaboich/jsparser");
        }

        static void ShowParserUiPanel()
        {
            // Dockable Dialog Demo
            // 
            // This demonstration shows you how to do a dockable dialog.
            // You can create your own non dockable dialog - in this case you don't nedd this demonstration.
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
                _nppTbData.dlgID = idFrmGotToLine;
                // define the default docking behaviour
                _nppTbData.uMask = NppTbMsg.DWS_DF_CONT_RIGHT | NppTbMsg.DWS_ICONTAB | NppTbMsg.DWS_ICONBAR;
                _nppTbData.hIconTab = (uint)tbIcon.Handle;
                _nppTbData.pszModuleName = PluginName;
                IntPtr _ptrNppTbData = Marshal.AllocHGlobal(Marshal.SizeOf(_nppTbData));
                Marshal.StructureToPtr(_nppTbData, _ptrNppTbData, false);

                Win32.SendMessage(nppData._nppHandle, NppMsg.NPPM_DMMREGASDCKDLG, 0, _ptrNppTbData);
                // Following message will toogle both menu item state and toolbar button
                Win32.SendMessage(nppData._nppHandle, NppMsg.NPPM_SETMENUITEMCHECK, _funcItems.Items[idFrmGotToLine]._cmdID, 1);
            }
            else
            {
            	if (!_frmParserUiContainer.Visible)
            	{
	                Win32.SendMessage(nppData._nppHandle, NppMsg.NPPM_DMMSHOW, 0, _frmParserUiContainer.Handle);
	                Win32.SendMessage(nppData._nppHandle, NppMsg.NPPM_SETMENUITEMCHECK, _funcItems.Items[idFrmGotToLine]._cmdID, 1);
            	}
            	else
            	{
	                Win32.SendMessage(nppData._nppHandle, NppMsg.NPPM_DMMHIDE, 0, _frmParserUiContainer.Handle);
	                Win32.SendMessage(nppData._nppHandle, NppMsg.NPPM_SETMENUITEMCHECK, _funcItems.Items[idFrmGotToLine]._cmdID, 0);
            	}
            }
            //_frmParserUiContainer..Focus();
        }
        #endregion
    }
}   
