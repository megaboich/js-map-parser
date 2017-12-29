using fastJSON;
using System.IO;

namespace JsMapParser.NppPlugin
{
    public class JsMapParserPluginSettings
    {
        private string _configPath;

        public bool ShowToolWindow { get; set; }
        public bool ToolWindowVisible { get; set; }

        public JsMapParserPluginSettings()
        {
        }

        private static string GetConfigFilePath(string configPath)
        {
            // if path doesn't exist, we create it
            if (!Directory.Exists(configPath))
            {
                Directory.CreateDirectory(configPath);
            }
            // make your file full file path name
            return Path.Combine(configPath, "JsMapParser.json");
        }

        public static JsMapParserPluginSettings Load(string configPath)
        {
            var filePath = GetConfigFilePath(configPath);
            if (File.Exists(filePath))
            {
                try
                {
                    var settingsJson = File.ReadAllText(filePath);
                    var settings = JSON.ToObject<JsMapParserPluginSettings>(settingsJson);
                    settings._configPath = configPath;
                    return settings;
                }
                catch
                {
                }
            }

            return new JsMapParserPluginSettings() { _configPath = configPath };
        }

        public void Save()
        {
            try
            {
                var serialized = JSON.ToNiceJSON(this, new JSONParameters()
                {
                    UseExtensions = false
                });
                File.WriteAllText(GetConfigFilePath(_configPath), serialized);
            }
            catch
            {
            }
        }
    }
}