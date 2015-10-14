using System;
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

                //File.AppendAllLines(Path.Combine(Path.GetTempPath(), "npp_logevent.log"), new[]
                //{
                //    nc.nmhdr.code + ":" + nc.lParam + "," + nc.wParam
                //});

                StringBuilder resultFileName = new StringBuilder(Win32.MAX_PATH);
                Win32.SendMessage(PluginBase.nppData._nppHandle, NppMsg.NPPM_GETFILENAME, Win32.MAX_PATH, resultFileName);

                if (nc.nmhdr.code == (uint) NppMsg.NPPN_TBMODIFICATION)
                {
                    PluginBase._funcItems.RefreshItems();
                    PluginBase.SetToolBarIcon();
                }
                else if (nc.nmhdr.code == (uint) SciMsg.SCN_CHARADDED)
                {
                    //PluginBase.doInsertHtmlCloseTag((char)nc.ch);
                }
                else if (nc.nmhdr.code == (uint) NppMsg.NPPN_SHUTDOWN)
                {
                    PluginBase.PluginCleanUp();
                    Marshal.FreeHGlobal(_ptrPluginName);
                }
                else if (nc.nmhdr.code == (uint) NppMsg.NPPN_FILESAVED)
                {
                    PluginBase.OnFileSaved();
                }
                else if (nc.nmhdr.code == (uint) NppMsg.NPPN_BUFFERACTIVATED)
                {
                    PluginBase.OnFileChanged();
                }
            }
            catch (Exception ex)
            {
                File.AppendAllLines(Path.Combine(Path.GetTempPath(), "npp_logevent.log"), new[]
                {
                    "ERROR: " + ex.Message + " " + ex.StackTrace
                });
            }
        }
    }
}
