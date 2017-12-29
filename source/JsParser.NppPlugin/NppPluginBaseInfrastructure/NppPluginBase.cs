using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace JsMapParser.NppPlugin.NppPluginBaseInfrastructure
{
    public delegate void NppEventHandler();

    internal class ToolbarButtonDefinition
    {
        public int Id { get; set; }
        public Bitmap Image { get; set; }
    }

    internal class MenuItemDefinition
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public NppFuncItemDelegate Handler { get; set; }
    }

    internal enum PanelPosition : uint
    {
        Left = NppTbMsg.DWS_DF_CONT_LEFT,
        Right = NppTbMsg.DWS_DF_CONT_RIGHT,
        Bottom = NppTbMsg.DWS_DF_CONT_BOTTOM,
        Top = NppTbMsg.DWS_DF_CONT_TOP,
        Floating = NppTbMsg.DWS_DF_FLOATING
    }

    internal abstract partial class NppPluginBase
    {
        private NppData _nppData;
        private FuncItems _funcItems = new FuncItems();

        internal abstract string PluginName { get; }
        internal abstract ICollection<ToolbarButtonDefinition> GetToolbarButtons();
        internal abstract ICollection<MenuItemDefinition> GetMenuItems();

        internal event NppEventHandler OnInit;
        internal event NppEventHandler OnReady;
        internal event NppEventHandler OnShutDown;
        internal event NppEventHandler OnFileSaved;
        internal event NppEventHandler OnFileChanged;
        
        private void SetCommand(int index, string commandName, NppFuncItemDelegate functionPointer,
            ShortcutKey shortcut = new ShortcutKey(), bool checkOnInit = false)
        {
            FuncItem funcItem = new FuncItem();
            funcItem._cmdID = index;
            funcItem._itemName = commandName;
            if (functionPointer != null)
            {
                funcItem._pFunc = functionPointer;
            }
            if (shortcut._key != 0)
            {
                funcItem._pShKey = shortcut;
            }
            funcItem._init2Check = checkOnInit;
            _funcItems.Add(funcItem);
        }

        protected void RegisterPanel(int uiId, Bitmap icon, Form panel, PanelPosition position = PanelPosition.Left)
        {
            var tbIcon = GetTransparentIcon(icon, 16, 16);
            var nppTbData = new NppTbData();
            nppTbData.hClient = panel.Handle;
            nppTbData.pszName = PluginName;
            nppTbData.dlgID = uiId;
            // define the default docking behavior
            nppTbData.uMask = (NppTbMsg)position | NppTbMsg.DWS_ICONTAB | NppTbMsg.DWS_ICONBAR;
            nppTbData.hIconTab = (uint)tbIcon.Handle;
            nppTbData.pszModuleName = PluginName;
            var ptrNppTbData = Marshal.AllocHGlobal(Marshal.SizeOf(nppTbData));
            Marshal.StructureToPtr(nppTbData, ptrNppTbData, false);

            Win32.SendMessage(_nppData._nppHandle, NppMsg.NPPM_DMMREGASDCKDLG, 0, ptrNppTbData);
        }

        protected void ShowPanel(Form panel, bool isShow)
        {
            Win32.SendMessage(_nppData._nppHandle, isShow ? NppMsg.NPPM_DMMSHOW : NppMsg.NPPM_DMMHIDE, 0, panel.Handle);
        }

        private static Icon GetTransparentIcon(Bitmap bitmapFromResource, int width, int height)
        {
            Icon tbIcon;
            using (var newBmp = new Bitmap(width, height))
            {
                var g = Graphics.FromImage(newBmp);
                var colorMap = new ColorMap[1];
                colorMap[0] = new ColorMap();
                colorMap[0].OldColor = Color.Fuchsia;
                colorMap[0].NewColor = Color.FromKnownColor(KnownColor.ButtonFace);
                var attr = new ImageAttributes();
                attr.SetRemapTable(colorMap);
                g.DrawImage(bitmapFromResource, new Rectangle(0, 0, width, height), 0, 0, width, height, GraphicsUnit.Pixel, attr);
                tbIcon = Icon.FromHandle(newBmp.GetHicon());
            }
            return tbIcon;
        }

        private IntPtr GetCurrentScintilla()
        {
            int curScintilla;
            Win32.SendMessage(_nppData._nppHandle, NppMsg.NPPM_GETCURRENTSCINTILLA, 0, out curScintilla);
            return (curScintilla == 0) ? _nppData._scintillaMainHandle : _nppData._scintillaSecondHandle;
        }

        protected string GetCurrentFile()
        {
            var path = new StringBuilder(Win32.MAX_PATH);
            Win32.SendMessage(_nppData._nppHandle, NppMsg.NPPM_GETFULLCURRENTPATH, 0, path);
            return path.ToString();
        }

        protected void CheckMenuItem(int menuId, bool isCheck)
        {
            Win32.SendMessage(_nppData._nppHandle, NppMsg.NPPM_SETMENUITEMCHECK, _funcItems.Items[menuId]._cmdID, isCheck ? 1 : 0);
        }

        internal void GetCursorPos(out int line, out int col)
        {
            IntPtr sci = GetCurrentScintilla();

            int currentPos = (int) Win32.SendMessage(sci, SciMsg.SCI_GETCURRENTPOS, 0, 0);

            line = (int) Win32.SendMessage(sci, SciMsg.SCI_LINEFROMPOSITION, currentPos, 0) + 1;
            col = (int) Win32.SendMessage(sci, SciMsg.SCI_GETCOLUMN, currentPos, 0);
        }

        internal void SetFocus()
        {
            IntPtr sci = GetCurrentScintilla();
            Win32.SendMessage(sci, SciMsg.SCI_GRABFOCUS, 0, 0);
        }

        internal void GoToPosition(int startLine, int startColumn)
        {
            IntPtr sci = GetCurrentScintilla();
            var pos = (int) Win32.SendMessage(sci, SciMsg.SCI_POSITIONFROMLINE, startLine - 1, 0);

            Win32.SendMessage(sci, SciMsg.SCI_GOTOPOS, pos + startColumn - 1, 0);
        }

        private void CreateMenuItems()
        {
            foreach (var menuItem in GetMenuItems())
            {
                SetCommand(menuItem.Id, menuItem.Text, menuItem.Handler);
            }
        }

        private void SetToolBarIcons()
        {
            var toolBarItems = GetToolbarButtons();
            foreach (var button in toolBarItems)
            {
                var tbIcons = new toolbarIcons();
                tbIcons.hToolbarBmp = button.Image.GetHbitmap();
                var pTbIcons = Marshal.AllocHGlobal(Marshal.SizeOf(tbIcons));
                Marshal.StructureToPtr(tbIcons, pTbIcons, false);
                Win32.SendMessage(_nppData._nppHandle, NppMsg.NPPM_ADDTOOLBARICON, _funcItems.Items[button.Id]._cmdID, pTbIcons);
                Marshal.FreeHGlobal(pTbIcons);
            }
        }

        /// <summary>
        /// Get path of plug-in configuration
        /// </summary>
        /// <returns></returns>
        protected string GetConfigFolder()
        {
            StringBuilder sbIniFilePath = new StringBuilder(Win32.MAX_PATH);
            Win32.SendMessage(_nppData._nppHandle, NppMsg.NPPM_GETPLUGINSCONFIGDIR, Win32.MAX_PATH, sbIniFilePath);
            return sbIniFilePath.ToString();
        }

        public Version GetNppVersion()
        {
            var version = Win32.SendMessage(_nppData._nppHandle, NppMsg.NPPM_GETNPPVERSION, 0, 0);

            int lo = unchecked((short) (long) version);
            int hi = unchecked((short) ((long) version >> 16));
            return new Version(hi, lo);
        }

        private void ProcessCommand(uint commandCode)
        {
            switch (commandCode)
            {
                case (uint)NppMsg.NPPN_READY:
                    OnReady?.Invoke();
                    break;
                case (uint)NppMsg.NPPN_TBMODIFICATION:
                    _funcItems.RefreshItems();
                    SetToolBarIcons();
                    break;
                case (uint)SciMsg.SCN_CHARADDED:
                    break;
                case (uint)NppMsg.NPPN_SHUTDOWN:
                    OnShutDown?.Invoke();
                    _funcItems.Dispose();
                    break;
                case (uint)NppMsg.NPPN_FILESAVED:
                    OnFileSaved?.Invoke();
                    break;
                case (uint)NppMsg.NPPN_BUFFERACTIVATED:
                    OnFileChanged?.Invoke();
                    break;
            }
        }
    }
}
