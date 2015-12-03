using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace JsParser.UI.UI
{
    public interface IThemeProvider
    {
        Theme CurrentTheme { get; }

        string CurrentThemeName { get; }

        IEnumerable<Theme> GetThemes();

        void SetCurrent(string name);

        void AddTheme(string name);
        void RemoveTheme(string name);
        void UpdateTheme(string name, string newName, ColorTable newColorTable);
    }

    public class ThemeProvider : IThemeProvider
    {
        private List<Theme> _themes;
        public string CurrentThemeName { get; set; }

        public Theme CurrentTheme
        {
            get { return _themes.FirstOrDefault(t => t.Name == CurrentThemeName); }
        }

        public ThemeProvider()
        {
            _themes = new List<Theme>();
        }

        public static ThemeProvider Deserialize(string serialized)
        {
            try
            {
                var tp = new ThemeProvider();
                var rootElt = XElement.Parse(serialized);
                tp.CurrentThemeName = rootElt.Attribute("Current").Value;
                var themesXml = rootElt.Elements("theme");
                foreach (var tx in themesXml)
                {
                    var t = Theme.LoadFromXml(tx);
                    tp._themes.Add(t);
                }

                return tp;
            }
            catch (Exception)
            {
                var tp = new ThemeProvider();
                tp._themes = new List<Theme>();
                tp._themes.Add(Theme.GetDefaultBlueTheme());
                tp.CurrentThemeName = tp._themes[0].Name;
                return tp;
            }
        }

        public string Serialize()
        {
            var rootElt = new XElement("root");
            foreach (var theme in _themes)
            {
                var themeXml = theme.SaveToXml();
                rootElt.Add(themeXml);
            }
            rootElt.SetAttributeValue("Current", this.CurrentThemeName);
            return rootElt.ToString();
        }

        public IEnumerable<Theme> GetThemes()
        {
            return _themes;
        }

        public void SetCurrent(string name)
        {
            CurrentThemeName = name;
        }

        public void AddTheme(string name)
        {
            var theme = new Theme()
            {
                Name = name,
                Colors = CurrentTheme.Colors.Clone(),
                IsPredefined = false
            };

            _themes.Add(theme);
        }

        public void RemoveTheme(string name)
        {
            _themes.Remove(_themes.FirstOrDefault(t => t.Name == name));
        }

        public void UpdateTheme(string name, string newName, ColorTable newColorTable)
        {
            throw new NotImplementedException();
        }
    }
}
