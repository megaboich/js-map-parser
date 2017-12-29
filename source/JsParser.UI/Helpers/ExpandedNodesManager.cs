using JsParser.UI.UI;
using System.Collections.Generic;

namespace JsParser.UI.Helpers
{
    class ExpandedNodesManager
	{
		private Dictionary<string, Dictionary<string, bool>> _storage = new Dictionary<string, Dictionary<string, bool>>();
		private string _activeDocName;
		private Dictionary<string, bool> _activeStorage;

		public void SetFile(string activeDocumentName )
		{
			_activeDocName = activeDocumentName;
			if (!HasDocumentInStorage(_activeDocName))
			{
				_storage.Add(_activeDocName, new Dictionary<string, bool>());
			}

			_activeStorage = _storage[_activeDocName];
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

		public bool? IsNoteExpanded(CustomTreeNode node)
		{
			var storage = ActiveStorage;
			var nodeKey = GetNodeKey(node);
			if (storage.ContainsKey(nodeKey))
			{
				return storage[nodeKey];
			}

			return null;
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
