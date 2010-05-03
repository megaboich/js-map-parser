using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JsParserCore.Properties;
using System.Web.Script.Serialization;

namespace JsParserCore.UI
{
	public class MarksManager
	{
		private IDictionary<string, IDictionary<string, string>> _bookmarkedItems;

		private string _currentFile;
		private IDictionary<string, string> _currentDictionary;

		public MarksManager()
		{
			if (!string.IsNullOrEmpty(Settings.Default.Data))
			{
				try
				{
					_bookmarkedItems = (new JavaScriptSerializer()).Deserialize<Dictionary<string, IDictionary<string, string>>>(Settings.Default.Data);
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
			Settings.Default.Data = d;
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
				treenode.Tags += mark;
			}
			else
			{
				treenode.Tags = null;
			}
		}

		public void SetMark(string mark, CustomTreeNode treenode)
		{
			SelectNode(mark, treenode);

			if (!string.IsNullOrEmpty(mark))
			{
				if (CurrentDictionary.ContainsKey(treenode.Text))
				{
					CurrentDictionary[treenode.Text] = CurrentDictionary[treenode.Text] + mark;
				}
				else
				{
					CurrentDictionary.Add(treenode.Text, mark);
				}
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
