using System;
using System.Collections;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using NppPlugin.DllExport;

namespace JsMapParser.NppPlugin
{
    class UnmanagedExports
    {
        [DllExport(CallingConvention=CallingConvention.Cdecl)]
        static bool isUnicode()
        {
            return true;
        }

        [DllExport(CallingConvention = CallingConvention.Cdecl)]
        static void setInfo(NppData notepadPlusData)
        {
            PluginBase.nppData = notepadPlusData;
            PluginBase.CommandMenuInit();
        }

        [DllExport(CallingConvention = CallingConvention.Cdecl)]
        static IntPtr getFuncsArray(ref int nbF)
        {
            nbF = PluginBase._funcItems.Items.Count;
            return PluginBase._funcItems.NativePointer;
        }

        [DllExport(CallingConvention = CallingConvention.Cdecl)]
        static uint messageProc(uint Message, IntPtr wParam, IntPtr lParam)
        {
            return 1;
        }

        static IntPtr _ptrPluginName = IntPtr.Zero;
        [DllExport(CallingConvention = CallingConvention.Cdecl)]
        static IntPtr getName()
        {
            if (_ptrPluginName == IntPtr.Zero)
                _ptrPluginName = Marshal.StringToHGlobalUni(PluginBase.PluginName);
            return _ptrPluginName;
        }

        [DllExport(CallingConvention = CallingConvention.Cdecl)]
        static void beNotified(IntPtr notifyCode)
        {
            try
            {
                SCNotification nc = (SCNotification) Marshal.PtrToStructure(notifyCode, typeof (SCNotification));
                var commandCode = nc.nmhdr.code;
                switch (commandCode)
                {
                    case (uint)NppMsg.NPPN_READY:
                        PluginBase.OnReady();
                        break;
                    case (uint)NppMsg.NPPN_TBMODIFICATION:
                        PluginBase._funcItems.RefreshItems();
                        PluginBase.SetToolBarIcon();
                        break;
                    case (uint)SciMsg.SCN_CHARADDED:
                        break;
                    case (uint)NppMsg.NPPN_SHUTDOWN:
                        PluginBase.PluginCleanUp();
                        Marshal.FreeHGlobal(_ptrPluginName);
                        break;
                    case (uint)NppMsg.NPPN_FILESAVED:
                        PluginBase.OnFileSaved();
                        break;
                    case (uint)NppMsg.NPPN_BUFFERACTIVATED:
                        PluginBase.OnFileChanged();
                        break;
                }
            }
            catch (Exception ex)
            {
                File.AppendAllLines(Path.Combine(Path.GetTempPath(), "npp_js_map_parser_event.log"), new[]
                {
                    "ERROR: " + ex.Message + " " + ex.StackTrace
                });
            }
        }
    }
}
