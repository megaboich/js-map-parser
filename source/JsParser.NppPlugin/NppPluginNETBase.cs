using System;
using System.Text;

namespace NppPluginNET
{
    partial class PluginBase
    {
        internal static NppData nppData;
        internal static FuncItems _funcItems = new FuncItems();

        internal static IntPtr NppHandle
        {
            get { return nppData._nppHandle; }
        }

        internal static void SetCommand(int index, string commandName, NppFuncItemDelegate functionPointer)
        {
            SetCommand(index, commandName, functionPointer, new ShortcutKey(), false);
        }

        internal static void SetCommand(int index, string commandName, NppFuncItemDelegate functionPointer,
            ShortcutKey shortcut)
        {
            SetCommand(index, commandName, functionPointer, shortcut, false);
        }

        internal static void SetCommand(int index, string commandName, NppFuncItemDelegate functionPointer,
            bool checkOnInit)
        {
            SetCommand(index, commandName, functionPointer, new ShortcutKey(), checkOnInit);
        }

        internal static void SetCommand(int index, string commandName, NppFuncItemDelegate functionPointer,
            ShortcutKey shortcut, bool checkOnInit)
        {
            FuncItem funcItem = new FuncItem();
            funcItem._cmdID = index;
            funcItem._itemName = commandName;
            if (functionPointer != null)
                funcItem._pFunc = new NppFuncItemDelegate(functionPointer);
            if (shortcut._key != 0)
                funcItem._pShKey = shortcut;
            funcItem._init2Check = checkOnInit;
            _funcItems.Add(funcItem);
        }

        internal static IntPtr GetCurrentScintilla()
        {
            int curScintilla;
            Win32.SendMessage(NppHandle, NppMsg.NPPM_GETCURRENTSCINTILLA, 0, out curScintilla);
            return (curScintilla == 0) ? nppData._scintillaMainHandle : nppData._scintillaSecondHandle;
        }

        public static string GetCurrentFile()
        {
            var path = new StringBuilder(Win32.MAX_PATH);
            Win32.SendMessage(NppHandle, NppMsg.NPPM_GETFULLCURRENTPATH, 0, path);
            return path.ToString();
        }

        public static void UncheckMenuItem(int menuId)
        {
            Win32.SendMessage(NppHandle, NppMsg.NPPM_SETMENUITEMCHECK, _funcItems.Items[menuId]._cmdID, 0);
        }

        public static void GetCursorPos(out int line, out int col)
        {
            IntPtr sci = GetCurrentScintilla();

            int currentPos = (int) Win32.SendMessage(sci, SciMsg.SCI_GETCURRENTPOS, 0, 0);

            line = (int) Win32.SendMessage(sci, SciMsg.SCI_LINEFROMPOSITION, currentPos, 0) + 1;
            col = (int) Win32.SendMessage(sci, SciMsg.SCI_GETCOLUMN, currentPos, 0);
        }

        internal static void SetFocus()
        {
            IntPtr sci = GetCurrentScintilla();
            Win32.SendMessage(sci, SciMsg.SCI_GRABFOCUS, 0, 0);
        }

        internal static void GoToPosition(int startLine, int startColumn)
        {
            IntPtr sci = GetCurrentScintilla();
            var pos = (int) Win32.SendMessage(sci, SciMsg.SCI_POSITIONFROMLINE, startLine - 1, 0);

            Win32.SendMessage(sci, SciMsg.SCI_GOTOPOS, pos + startColumn - 1, 0);
        }
    }
}
