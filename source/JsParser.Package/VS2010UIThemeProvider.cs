using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JsParser.Core.UI;
using Microsoft.VisualStudio.Shell.Interop;
using System.Drawing;
using System.Diagnostics;
using System.Web.Script.Serialization;
using System.IO;

namespace JsParser_package
{
    public class VS2010UIThemeProvider : IUIThemeProvider
    {
        private Func<Type, object> _serviceResolver;

        public VS2010UIThemeProvider(Func<Type, object> serviceResolver)
        {
            _serviceResolver = serviceResolver;
        }

        private Color getVSColor(IVsUIShell2 uiShell2, __VSSYSCOLOREX color)
        {
            //get the COLORREF structure
            uint win32Color;
            uiShell2.GetVSSysColorEx((int)color, out win32Color);
            //translate it to a managed Color structure
            return ColorTranslator.FromWin32((int)win32Color);
        }

        public ColorTable GetColors()
        {
            //getIVSUIShell2
            var uiShell2 = _serviceResolver(typeof(SVsUIShell)) as IVsUIShell2;
            Debug.Assert(uiShell2 != null, "failed to get IVsUIShell2");

            var colorTable = new ColorTable
            {
                ControlBackground = getVSColor(uiShell2, __VSSYSCOLOREX.VSCOLOR_ENVIRONMENT_BACKGROUND),
                WindowText = getVSColor(uiShell2, __VSSYSCOLOREX.VSCOLOR_TOOLWINDOW_TEXT),
                WindowBackground = getVSColor(uiShell2, __VSSYSCOLOREX.VSCOLOR_TOOLWINDOW_BACKGROUND),
                ControlText = getVSColor(uiShell2, __VSSYSCOLOREX.VSCOLOR_TOOLWINDOW_TEXT),
            };

            //collect all colors
            var colorIndexes = Enum.GetValues(typeof(__VSSYSCOLOREX));
            var colorNames = Enum.GetNames(typeof(__VSSYSCOLOREX));
            var dic = new Dictionary<string, object>();
            for (var index = 0; index < colorIndexes.Length; ++index)
            {
                dic.Add(colorNames[index], new
                {
                    index = (int)colorIndexes.GetValue(index),
                    color = System.Drawing.ColorTranslator.ToHtml(getVSColor(uiShell2, (__VSSYSCOLOREX)colorIndexes.GetValue(index))),
                });
            }

            File.WriteAllText("c:\\vscolors.txt", new JavaScriptSerializer().Serialize(dic));

            return colorTable;
        }
    }
}
