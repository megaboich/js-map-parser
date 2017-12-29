namespace JsParser.Core.Code
{
    public interface ICodeProvider
	{
		string LoadCode();

		string Path { get; }

		string Name { get; }

		string FullName { get; }

		string ContainerName { get; set; }

		void SelectionMoveToLineAndOffset(int startLine, int startColumn);

		void SetFocus();

		void GetCursorPos(out int line, out int column);
	}
}
