using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JsParserCore.UI
{
	class ExpandedNodesManager
	{
		private Dictionary<string, Dictionary<string, bool>> _storage = new Dictionary<string, Dictionary<string, bool>>();
		private string _activeDocName;
		private Dictionary<string, bool> _activeStorage;

		public string ActiveDocumentName 
		{
			get { return _activeDocName; }
			set
			{
				_activeDocName = value;
				if (!HasDocumentInStorage(_activeDocName))
				{
					_storage.Add(_activeDocName, new Dictionary<string, bool>());
				}

				_activeStorage = _storage[ActiveDocumentName];
			}
		}

		public bool HasDocumentInStorage(string docName)
		{
			return _storage.ContainsKey(docName);
		}

		public Dictionary<string, bool> ActiveStorage
		{
			get
			{
				return _activeStorage;
			}
		}

		public bool IsNoteExpanded(CustomTreeNode node)
		{
			var storage = ActiveStorage;
			var nodeKey = GetNodeKey(node);
			if (storage.ContainsKey(nodeKey))
			{
				return storage[nodeKey];
			}

			return false;
		}

		public void SetExpandedState(CustomTreeNode node)
		{
			ActiveStorage[GetNodeKey(node)] = node.IsExpanded;
		}

		private string GetNodeKey(CustomTreeNode node)
		{
			return node.CodeNode.StartLine + "_(" + node.Text + ")";
		}
	}
}
