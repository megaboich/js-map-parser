using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;

namespace JsParserCore.UI
{
	public class CustomTreeView : TreeView
	{
		private const int WM_HSCROL = 0x114;
		private const int WM_VSCROLL = 0x115;
		private const int WM_MOUSEWHEEL = 0x20A;

		[Description("UserControlOnLoadDescr")]
		[Category("CatBehavior")]
		public event EventHandler OnScroll;

		protected override void WndProc(ref Message m)
		{
			switch (m.Msg)
			{
				case WM_HSCROL:
				case WM_MOUSEWHEEL:
				case WM_VSCROLL:
					OnScroll.Invoke(null, null);
					break;
			}

			base.WndProc(ref m);
		}
	}
}
