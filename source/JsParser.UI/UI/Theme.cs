using System.Drawing;
using System.Xml.Linq;

namespace JsParser.UI.UI
{
    public class Theme
    {
        public bool IsPredefined { get; set; }
        public string Name { get; set; }
        public ColorTable Colors { get; set; }

        public XElement SaveToXml()
        {
            var themeXml = new XElement("theme");
            themeXml.SetAttributeValue("Name", this.Name);
            themeXml.SetAttributeValue("Predefined", this.IsPredefined);
            themeXml.SetAttributeValue("ControlBackground", ColorTranslator.ToHtml(this.Colors.ControlBackground));
            themeXml.SetAttributeValue("ControlText", ColorTranslator.ToHtml(this.Colors.ControlText));
            themeXml.SetAttributeValue("GridLines", ColorTranslator.ToHtml(this.Colors.GridLines));
            themeXml.SetAttributeValue("LineNumbersText", ColorTranslator.ToHtml(this.Colors.LineNumbersText));
            themeXml.SetAttributeValue("HighlightBackground", ColorTranslator.ToHtml(this.Colors.HighlightBackground));
            themeXml.SetAttributeValue("HighlightInactiveBackground",
                ColorTranslator.ToHtml(this.Colors.HighlightInactiveBackground));
            themeXml.SetAttributeValue("HighlightInactiveText",
                ColorTranslator.ToHtml(this.Colors.HighlightInactiveText));
            themeXml.SetAttributeValue("HighlightText", ColorTranslator.ToHtml(this.Colors.HighlightText));
            themeXml.SetAttributeValue("MenuBackground", ColorTranslator.ToHtml(this.Colors.MenuBackground));
            themeXml.SetAttributeValue("TabText", ColorTranslator.ToHtml(this.Colors.TabText));
            themeXml.SetAttributeValue("WindowBackground", ColorTranslator.ToHtml(this.Colors.WindowBackground));
            themeXml.SetAttributeValue("WindowText", ColorTranslator.ToHtml(this.Colors.WindowText));
            return themeXml;
        }

        public static Theme LoadFromXml(XElement tx)
        {
            var t = new Theme();
            t.Name = tx.Attribute("Name").Value;
            t.IsPredefined = bool.Parse(tx.Attribute("Predefined").Value);
            t.Colors = new ColorTable();
            t.Colors.ControlBackground = ColorTranslator.FromHtml(tx.Attribute("ControlBackground").Value);
            t.Colors.ControlText = ColorTranslator.FromHtml(tx.Attribute("ControlText").Value);
            t.Colors.GridLines = ColorTranslator.FromHtml(tx.Attribute("GridLines").Value);
            t.Colors.LineNumbersText = ColorTranslator.FromHtml(tx.Attribute("LineNumbersText").Value);
            t.Colors.HighlightBackground = ColorTranslator.FromHtml(tx.Attribute("HighlightBackground").Value);
            t.Colors.HighlightInactiveBackground =
                ColorTranslator.FromHtml(tx.Attribute("HighlightInactiveBackground").Value);
            t.Colors.HighlightInactiveText = ColorTranslator.FromHtml(tx.Attribute("HighlightInactiveText").Value);
            t.Colors.HighlightText = ColorTranslator.FromHtml(tx.Attribute("HighlightText").Value);
            t.Colors.MenuBackground = ColorTranslator.FromHtml(tx.Attribute("MenuBackground").Value);
            t.Colors.TabText = ColorTranslator.FromHtml(tx.Attribute("TabText").Value);
            t.Colors.WindowBackground = ColorTranslator.FromHtml(tx.Attribute("WindowBackground").Value);
            t.Colors.WindowText = ColorTranslator.FromHtml(tx.Attribute("WindowText").Value);
            return t;
        }

        public static Theme GetDefaultBlueTheme()
        {
            return new Theme()
            {
                Name = "Blue",
                IsPredefined = true,
                Colors = new ColorTable()
                {
                    ControlBackground = ColorTranslator.FromHtml("#293955"),
                    ControlText = ColorTranslator.FromHtml("#000000"),
                    WindowBackground = ColorTranslator.FromHtml("#ffffff"),
                    WindowText = ColorTranslator.FromHtml("#000000"),
                    HighlightBackground = ColorTranslator.FromHtml("#3399ff"),
                    HighlightInactiveBackground = ColorTranslator.FromHtml("#b4b4b4"),
                    HighlightText = ColorTranslator.FromHtml("#ffffff"),
                    HighlightInactiveText = ColorTranslator.FromHtml("#000000"),
                    GridLines = ColorTranslator.FromHtml("#f0f0f0"),
                    LineNumbersText = Color.Gray,
                    TabText = ColorTranslator.FromHtml("#ffffff"),
                    MenuBackground = ColorTranslator.FromHtml("#e9ecee"),
                }
            };
        }

        public static Theme GetDefaultDarkTheme()
        {
            return Theme.LoadFromXml(XElement.Parse(@"
<theme Name='Dark'
 Predefined='true' 
 ControlBackground='#2d2d30'
 ControlText='#f1f1f1' 
 GridLines='#000000'
 LineNumbersText='#777777'
 HighlightBackground='#3399ff' 
 HighlightInactiveBackground='#3f3f46' 
 HighlightInactiveText='#f1f1f1' 
 HighlightText='#FFFFFF' 
 MenuBackground='#1b1b1c' 
 TabText='#d0d0d0' 
 WindowBackground='#252526' 
 WindowText='#f1f1f1' />"));
        }

        public static Theme GetDefaultLightTheme()
        {
            return Theme.LoadFromXml(XElement.Parse(@"
<theme Name='Light'
 Predefined='true' 
 ControlBackground='#efeff2'
 ControlText='#1e1e1e' 
 GridLines='#f0f0f0' 
 LineNumbersText='#777777'
 HighlightBackground='#3399ff' 
 HighlightInactiveBackground='#cccedb' 
 HighlightInactiveText='#1e1e1e' 
 HighlightText='#ffffff' 
 MenuBackground='#e7e8ec' 
 TabText='#444444' 
 WindowBackground='#f6f6f6' 
 WindowText='#1e1e1e' />"));
        }
    }
}

