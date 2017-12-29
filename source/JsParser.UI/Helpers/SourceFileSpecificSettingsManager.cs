using fastJSON;
using JsParser.UI.Properties;
using JsParser.UI.UI;
using System.Collections.Generic;
using System.Drawing;

namespace JsParser.UI.Helpers
{
    public class SourceFileSpecificSettings
    {
        public Dictionary<string, string> Marks = new Dictionary<string, string>();
    }

    public class SourceFileSpecificSettingsManager
    {
        private Dictionary<string, SourceFileSpecificSettings> _storage;

        private string _currentFile;
        private SourceFileSpecificSettings _currentFileSettings;

        public SourceFileSpecificSettingsManager()
        {
            if (!string.IsNullOrEmpty(Settings.Default.FileSpecificData))
            {
                try
                {
                    _storage = JSON.ToObject<Dictionary<string, SourceFileSpecificSettings>>(Settings.Default.FileSpecificData,
                        new JSONParameters() { UseExtensions = false });
                }
                catch
                {
                    _storage = new Dictionary<string, SourceFileSpecificSettings>();
                }
            }
            else
            {
                 _storage = new Dictionary<string, SourceFileSpecificSettings>();
            }
        }

        public void Save()
        {
            var d = JSON.ToNiceJSON(_storage, new JSONParameters() { UseExtensions = false });
            Settings.Default.FileSpecificData = d;
            Settings.Default.Save();
        }

        public void SetFile(string file)
        {
            _currentFile = file;
            if (_storage.ContainsKey(file))
            {
                _currentFileSettings = _storage[file];
            }
            else
            {
                _currentFileSettings = null;
            }
        }

        public SourceFileSpecificSettings CurrentFileSettings
        {
            get
            {
                if (_currentFileSettings == null)
                {
                    _currentFileSettings = new SourceFileSpecificSettings();
                    _storage[_currentFile] = _currentFileSettings;
                }
                return _currentFileSettings;
            }
        }

        private void SelectNode(string mark, CustomTreeNode treenode)
        {
            if (!string.IsNullOrEmpty(mark))
            {
                treenode.Tags = mark;

                switch (mark[0])
                {
                    case 'B':
                        treenode.ForeColor = Settings.Default.taggedFunction2Color;
                        break;
                    case 'G':
                        treenode.ForeColor = Settings.Default.taggedFunction3Color;
                        break;
                    case 'O':
                        treenode.ForeColor = Settings.Default.taggedFunction4Color;
                        break;
                    case 'R':
                        treenode.ForeColor = Settings.Default.taggedFunction5Color;
                        break;
                    case 'S':
                        treenode.ForeColor = Settings.Default.taggedFunction6Color;
                        break;
                }
            }
            else
            {
                treenode.ForeColor = SystemColors.WindowText;
                treenode.NodeFont = null;
                treenode.Tags = null;
            }
        }

        public void SetMark(string mark, CustomTreeNode treenode)
        {
            SelectNode(mark, treenode);

            if (!string.IsNullOrEmpty(mark))
            {
                CurrentFileSettings.Marks[treenode.Text] = mark;
            }
            else
            {
                CurrentFileSettings.Marks.Remove(treenode.Text);
            }

            Save();
        }

        public void ResetMarks()
        {
            _currentFileSettings = null;
            _storage[_currentFile] = null;
        }

        public void RestoreMark(CustomTreeNode treeNode)
        {
            if (_currentFileSettings == null)
            {
                return;
            }

            if (CurrentFileSettings.Marks.ContainsKey(treeNode.Text))
            {
                SelectNode(CurrentFileSettings.Marks[treeNode.Text], treeNode);
            }
        }
    }
}
