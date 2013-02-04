using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace JsParser.UI.UI
{
    public interface IUIThemeProvider
    {
        ColorTable Colors { get; }

        void SubscribeToThemeChanged(Action action);
    }

    public class DefaultUIThemeProvider : IUIThemeProvider
    {
        ColorTable _colorTable;

        public DefaultUIThemeProvider()
        {
            _colorTable = ColorTable.Default;
        }

        public ColorTable Colors
        {
            get
            {
                return _colorTable;
            }
        }

        public void SubscribeToThemeChanged(Action action)
        {
            //nothing here
        }
    }
}
