using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace JsParser.Core.UI
{
    public interface IUIThemeProvider
    {
        ColorTable GetColors();
    }

    public class DefaultUIThemeProvider : IUIThemeProvider
    {
        ColorTable _colorTable;

        public DefaultUIThemeProvider()
        {
            _colorTable = ColorTable.Default;
        }

        public ColorTable GetColors()
        {
            return _colorTable;
        }
    }
}
