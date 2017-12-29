using NppPlugin.DllExport;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace JsMapParser.NppPlugin.NppPluginBaseInfrastructure
{
    internal abstract partial class NppPluginBase
    {
        private static NppPluginBase _plugin;

        [DllExport(CallingConvention = CallingConvention.Cdecl)]
        private static bool isUnicode()
        {
            return true;
        }

        [DllExport(CallingConvention = CallingConvention.Cdecl)]
        private static void setInfo(NppData notepadPlusData)
        {
            if (_plugin == null)
            {
                _plugin = GetPluginInstance();
            }
            _plugin._nppData = notepadPlusData;
            _plugin.OnInit?.Invoke();
            _plugin.CreateMenuItems();
        }

        [DllExport(CallingConvention = CallingConvention.Cdecl)]
        private static IntPtr getFuncsArray(ref int nbF)
        {
            nbF = _plugin._funcItems.Items.Count;
            return _plugin._funcItems.NativePointer;
        }

        [DllExport(CallingConvention = CallingConvention.Cdecl)]
        private static uint messageProc(uint Message, IntPtr wParam, IntPtr lParam)
        {
            return 1;
        }

        private static IntPtr _ptrPluginName = IntPtr.Zero;

        [DllExport(CallingConvention = CallingConvention.Cdecl)]
        private static IntPtr getName()
        {
            if (_ptrPluginName == IntPtr.Zero)
            {
                if (_plugin == null)
                {
                    _plugin = GetPluginInstance();
                }
                _ptrPluginName = Marshal.StringToHGlobalUni(_plugin.PluginName);
            }
            return _ptrPluginName;
        }

        [DllExport(CallingConvention = CallingConvention.Cdecl)]
        private static void beNotified(IntPtr notifyCode)
        {
            try
            {
                SCNotification nc = (SCNotification) Marshal.PtrToStructure(notifyCode, typeof (SCNotification));
                var commandCode = nc.nmhdr.code;
                _plugin.ProcessCommand(commandCode);
                switch (commandCode)
                {
                    case (uint) NppMsg.NPPN_SHUTDOWN:
                        Marshal.FreeHGlobal(_ptrPluginName);
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