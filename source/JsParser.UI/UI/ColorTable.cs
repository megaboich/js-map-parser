using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace JsParser.UI.UI
{
    public class ColorTable
    {
        public Color ControlBackground { get; set; }

        public Color ControlText { get; set; }

        public Color WindowBackground { get; set; }

        public Color WindowText { get; set; }

        public Color HighlightBackground { get; set; }

        public Color HighlightInactiveBackground { get; set; }

        public Color HighlightText { get; set; }

        public Color HighlightInactiveText { get; set; }

        public Color GridLines { get; set; }

        public Color TabText { get; set; }

        public static ColorTable Default
        {
            get
            {
                return new ColorTable
                {
                    ControlBackground = SystemColors.Control,
                    ControlText = SystemColors.ControlText,
                    WindowBackground = SystemColors.Window,
                    WindowText = SystemColors.WindowText,
                    HighlightBackground = SystemColors.Highlight,
                    HighlightText = SystemColors.HighlightText,
                    HighlightInactiveBackground = SystemColors.InactiveCaption,
                    HighlightInactiveText = SystemColors.InactiveCaptionText,
                    GridLines = SystemColors.ActiveBorder,
                    TabText = SystemColors.WindowText
                };
            }
        }
    }
}
