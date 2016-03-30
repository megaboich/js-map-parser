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
    }

    public class ThemeProvider : IThemeProvider
    {
        private List<Theme> _themes;
        public string CurrentThemeName { get; set; }

        public Theme CurrentTheme
        {
            get
            {
                var tm = _themes.FirstOrDefault(t => t.Name == CurrentThemeName);
                if (tm == null && _themes.Count > 0)
                {
                    tm = _themes.First();
                    CurrentThemeName = tm.Name;
                }

                return tm;
            }
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
                var themesXml = rootElt.Elements("theme");
                foreach (var tx in themesXml)
                {
                    var t = Theme.LoadFromXml(tx);
                    tp._themes.Add(t);
                }

                tp.CurrentThemeName = rootElt.Attribute("Current").Value;
                
                if (tp._themes.Count == 0)
                {
                    throw new Exception("load defaults then");
                }

                return tp;
            }
            catch (Exception)
            {
                var tp = new ThemeProvider();
                tp._themes = new List<Theme>();
                tp._themes.Add(Theme.GetDefaultLightTheme());
                tp._themes.Add(Theme.GetDefaultDarkTheme());
                tp._themes.Add(Theme.GetDefaultBlueTheme());
                tp.CurrentThemeName = tp._themes.First().Name;
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
            rootElt.SetAttributeValue("Current", this.CurrentTheme.Name);
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

            SetCurrent(name);
        }

        public void RemoveTheme(string name)
        {
            if (_themes.Count > 1)
            {
                var themeToRemove = _themes.FirstOrDefault(t => t.Name == name);
                if (!themeToRemove.IsPredefined)
                {
                    _themes.Remove(themeToRemove);
                    SetCurrent(_themes.First().Name);
                }
            }
        }
    }
}
