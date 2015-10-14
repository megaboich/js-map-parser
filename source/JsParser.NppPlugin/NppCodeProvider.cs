using System;
using System.IO;
using System.Reflection;
using JsParser.Core.Code;

namespace JsMapParser.NppPlugin
{
    internal class NppCodeProvider : ICodeProvider
    {
        public NppCodeProvider(string fileName)
        {
            FullName = fileName;
            Path = System.IO.Path.GetDirectoryName(fileName);
            Name = System.IO.Path.GetFileName(fileName);

            ContainerName = Assembly.GetExecutingAssembly().FullName;
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

        public string Path { get; private set; }
        public string Name { get; private set; }
        public string FullName { get; private set; }
        public string ContainerName { get; set; }

        public void SelectionMoveToLineAndOffset(int startLine, int startColumn)
        {
            PluginBase.GoToPosition(startLine, startColumn);
        }

        public void SetFocus()
        {
            PluginBase.SetFocus();
        }

        public void GetCursorPos(out int line, out int column)
        {
            PluginBase.GetCursorPos(out line, out column);
        }
    }
}
