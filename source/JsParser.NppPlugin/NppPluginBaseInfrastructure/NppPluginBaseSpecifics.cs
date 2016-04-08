using System;
using System.IO;
using System.Runtime.InteropServices;
using NppPlugin.DllExport;

namespace JsMapParser.NppPlugin.NppPluginBaseInfrastructure
{
    internal abstract partial class NppPluginBase
    {
        private static NppPluginBase GetPluginInstance()
        {
            return new JsMapParserPlugin();
        }
    }
}