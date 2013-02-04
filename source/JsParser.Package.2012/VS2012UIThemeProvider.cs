using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.Shell.Interop;
using System.Drawing;
using System.Diagnostics;
using System.IO;
using JsParser.UI.UI;
using Microsoft.VisualStudio.PlatformUI;
using System.Reflection;
using Microsoft.VisualStudio.Shell;

namespace JsParser.Package._2012
{
    public class VS2012UIThemeProvider : IUIThemeProvider
    {
        private Action _themeChangedAction;
        private ColorTable _colorTable;

        public VS2012UIThemeProvider(Func<Type, object> serviceResolver)
        {
            ReloadColors();
        }

        private void ReloadColors()
        {
            _colorTable = new ColorTable
            {
                ControlBackground = VSColorTheme.GetThemedColor(EnvironmentColors.EnvironmentBackgroundColorKey),
                ControlText = VSColorTheme.GetThemedColor(EnvironmentColors.ToolWindowTextColorKey),

                MenuBackground = VSColorTheme.GetThemedColor(EnvironmentColors.CommandBarMenuBackgroundGradientBeginColorKey),

                WindowBackground = VSColorTheme.GetThemedColor(EnvironmentColors.ToolWindowBackgroundColorKey),
                WindowText = VSColorTheme.GetThemedColor(EnvironmentColors.ToolWindowTextColorKey),

                HighlightBackground = VSColorTheme.GetThemedColor(EnvironmentColors.SystemHighlightColorKey),
                //HighlightInactiveBackground = VSColorTheme.GetThemedColor(EnvironmentColors.DarkColorKey),
                HighlightInactiveBackground = VSColorTheme.GetThemedColor(EnvironmentColors.SystemActiveBorderColorKey),

                HighlightText = VSColorTheme.GetThemedColor(EnvironmentColors.SystemHighlightTextColorKey),
                HighlightInactiveText = VSColorTheme.GetThemedColor(EnvironmentColors.ToolWindowTextColorKey),

                GridLines = VSColorTheme.GetThemedColor(EnvironmentColors.GridLineColorKey),

                TabText = VSColorTheme.GetThemedColor(EnvironmentColors.ToolWindowTabTextColorKey),
            };


            //    //collect all colors
            //    var props = typeof(EnvironmentColors).GetProperties(BindingFlags.Static | BindingFlags.Public | BindingFlags.GetProperty);

            //    var dic = new Dictionary<string, object>();
            //    int i=0;
            //    foreach (var p in props.Where(p => p.PropertyType == typeof(ThemeResourceKey)))
            //    {
            //        try
            //        {
            //            var resKey = (ThemeResourceKey)p.GetValue(null);
            //            var color = VSColorTheme.GetThemedColor(resKey);
            //            dic.Add(p.Name, new
            //            {
            //                index = i,
            //                color = System.Drawing.ColorTranslator.ToHtml(color),
            //            });
            //        }
            //        catch { }
            //        ++i;
            //    }

            //    File.WriteAllText("c:\\vscolors_new.txt", new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(dic));
            //
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
            _themeChangedAction = action;
            VSColorTheme.ThemeChanged += VSColorTheme_ThemeChanged;
        }

        private void VSColorTheme_ThemeChanged(ThemeChangedEventArgs e)
        {
            ReloadColors();
            if (_themeChangedAction != null)
            {
                _themeChangedAction();
            }
        }
    }
}
