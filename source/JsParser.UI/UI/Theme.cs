using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
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
            themeXml.SetAttributeValue("HighlightBackground", ColorTranslator.ToHtml(this.Colors.HighlightBackground));
            themeXml.SetAttributeValue("HighlightInactiveBackground", ColorTranslator.ToHtml(this.Colors.HighlightInactiveBackground));
            themeXml.SetAttributeValue("HighlightInactiveText", ColorTranslator.ToHtml(this.Colors.HighlightInactiveText));
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
            t.Colors.HighlightBackground = ColorTranslator.FromHtml(tx.Attribute("HighlightBackground").Value);
            t.Colors.HighlightInactiveBackground = ColorTranslator.FromHtml(tx.Attribute("HighlightInactiveBackground").Value);
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
                    TabText = ColorTranslator.FromHtml("#ffffff"),
                    MenuBackground = ColorTranslator.FromHtml("#e9ecee"),
                }
            };
        }
    }
}
