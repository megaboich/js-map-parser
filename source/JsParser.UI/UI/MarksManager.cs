using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JsParser.Core.Properties;
using System.Web.Script.Serialization;
using System.Drawing;

namespace JsParser.Core.UI
{
	public class MarksManager
	{
		private IDictionary<string, IDictionary<string, string>> _bookmarkedItems;

		private string _currentFile;
		private IDictionary<string, string> _currentDictionary;

		public MarksManager()
		{
			if (!string.IsNullOrEmpty(Settings.Default.BookmarksData))
			{
				try
				{
					_bookmarkedItems = (new JavaScriptSerializer()).Deserialize<Dictionary<string, IDictionary<string, string>>>(Settings.Default.BookmarksData);
				}
				catch
				{
					_bookmarkedItems = new Dictionary<string, IDictionary<string, string>>();
				}
			}
			else
			{
				 _bookmarkedItems = new Dictionary<string, IDictionary<string, string>>();
			}
		}

		public void Save()
		{
			var d = (new JavaScriptSerializer()).Serialize(_bookmarkedItems);
			Settings.Default.BookmarksData = d;
			Settings.Default.Save();
		}

		public void SetFile(string file)
		{
			_currentFile = file;
			if (_bookmarkedItems.ContainsKey(file))
			{
				_currentDictionary = _bookmarkedItems[file];
			}
			else
			{
				_currentDictionary = null;
			}
		}

		public IDictionary<string, string> CurrentDictionary
		{
			get
			{
				if (_currentDictionary == null)
				{
					_currentDictionary = new Dictionary<string, string>();
					_bookmarkedItems[_currentFile] = _currentDictionary;
				}
				return _currentDictionary;
			}
		}

		private void SelectNode(string mark, CustomTreeNode treenode)
		{
			if (!string.IsNullOrEmpty(mark))
			{
				treenode.Tags = mark;

				switch (mark[0])
				{
					case 'W':
						treenode.ForeColor = Settings.Default.taggedFunction1Color;
						treenode.NodeFont = Settings.Default.taggedFunction1Font;
						break;
					case 'B':
						treenode.ForeColor = Settings.Default.taggedFunction2Color;
						treenode.NodeFont = Settings.Default.taggedFunction2Font;
						break;
					case 'G':
						treenode.ForeColor = Settings.Default.taggedFunction3Color;
						treenode.NodeFont = Settings.Default.taggedFunction3Font;
						break;
					case 'O':
						treenode.ForeColor = Settings.Default.taggedFunction4Color;
						treenode.NodeFont = Settings.Default.taggedFunction4Font;
						break;
					case 'R':
						treenode.ForeColor = Settings.Default.taggedFunction5Color;
						treenode.NodeFont = Settings.Default.taggedFunction5Font;
						break;
					case 'S':
						treenode.ForeColor = Settings.Default.taggedFunction6Color;
						treenode.NodeFont = Settings.Default.taggedFunction6Font;
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
				CurrentDictionary[treenode.Text] = mark;
			}
			else
			{
				CurrentDictionary.Remove(treenode.Text);
			}

			Save();
		}

		public void ResetMarks()
		{
			_currentDictionary = null;
			_bookmarkedItems[_currentFile] = null;
		}

		public void RestoreMark(CustomTreeNode treeNode)
		{
			if (_currentDictionary == null)
			{
				return;
			}

			if (CurrentDictionary.ContainsKey(treeNode.Text))
			{
				SelectNode(CurrentDictionary[treeNode.Text], treeNode);
			}
		}
	}
}
