using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;

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
