using System;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace JsParser.UI.UI
{
    [Serializable]
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
        public Color MenuBackground { get; set; }
        public Color LineNumbersText { get; set; }

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
                    LineNumbersText = Color.Gray,
                    TabText = SystemColors.WindowText,
                    MenuBackground = SystemColors.Control,
                };
            }
        }

        public ColorTable Clone()
        {
            return new ColorTable()
            {
                ControlBackground = ControlBackground,
                ControlText = ControlText,
                WindowBackground = WindowBackground,
                WindowText = WindowText,
                HighlightBackground = HighlightBackground,
                HighlightText = HighlightText,
                HighlightInactiveBackground = HighlightInactiveBackground,
                HighlightInactiveText = HighlightInactiveText,
                GridLines = GridLines,
                LineNumbersText = LineNumbersText,
                TabText = TabText,
                MenuBackground = MenuBackground,
            };
        }

        public string GetHash()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                new BinaryFormatter().Serialize(stream, this);
                return Convert.ToBase64String(stream.ToArray());
            }
        }
    }
}
