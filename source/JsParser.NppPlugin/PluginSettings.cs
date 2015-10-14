using System.IO;

namespace JsMapParser.NppPlugin
{
    internal class PluginSettings
    {
        public bool ShowToolWindow { get; set; }

        private string _iniFilePath;

        public PluginSettings(string configPath)
        {
            // if config path doesn't exist, we create it
            if (!Directory.Exists(configPath))
            {
                Directory.CreateDirectory(configPath);
            }

            // make your plugin config file full file path name
            _iniFilePath = Path.Combine(configPath, "JsMapParser.ini");

            // get the parameter value from plugin config
            ShowToolWindow = (Win32.GetPrivateProfileInt("JsMapParserExtension", "ShowToolWindow", 0, _iniFilePath) != 0);
        }

        public void Save()
        {
            Win32.WritePrivateProfileString("JsMapParserExtension", "ShowToolWindow", ShowToolWindow ? "1" : "0", _iniFilePath);
        }
    }
}
