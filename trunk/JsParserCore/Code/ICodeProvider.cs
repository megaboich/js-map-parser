using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JsParcerCore.Code
{
	public interface ICodeProvider
	{
		string LoadCode();

		string Path { get; }

		string Name { get; }

		void SelectionMoveToLineAndOffset(int startLine, int startColumn);

		void SetFocus();
	}
}
