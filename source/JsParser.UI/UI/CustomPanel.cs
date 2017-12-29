using System.Windows.Forms;

namespace JsParser.UI.UI
{
	public class CustomPanel : Panel
	{
        public CustomPanel()
            : base()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }
	}
}
