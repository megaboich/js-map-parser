using JsParser.Core.Code;
using System;
using System.IO;

namespace JsMapParser.NppPlugin
{
    internal class NppCodeProvider : ICodeProvider
    {
        private JsMapParserPlugin _plugin;

        public string Path { get; private set; }
        public string Name { get; private set; }
        public string FullName { get; private set; }
        public string ContainerName { get; set; }

        public NppCodeProvider(JsMapParserPlugin plugin, string fileName)
        {
            _plugin = plugin;

            FullName = fileName;
            Path = System.IO.Path.GetDirectoryName(fileName);
            Name = System.IO.Path.GetFileName(fileName);

            ContainerName = "Notepad++ " + _plugin.GetNppVersion();
        }

        public string LoadCode()
        {
            try
            {
                return File.ReadAllText(FullName);
            }
            catch (Exception)
            {
                return "function Failed_To_Load_File(){};";
            }
        }

        public void SelectionMoveToLineAndOffset(int startLine, int startColumn)
        {
            _plugin.GoToPosition(startLine, startColumn);
        }

        public void SetFocus()
        {
            _plugin.SetFocus();
        }

        public void GetCursorPos(out int line, out int column)
        {
            _plugin.GetCursorPos(out line, out column);
        }
    }
}
