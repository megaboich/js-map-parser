using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading;

namespace JsMapParser.NppPlugin
{
    public static class ToolsHelper
    {
        public static Icon GetTransparentIcon(Bitmap bitmapFromResource, int width, int height)
        {
            Icon tbIcon;
            using (var newBmp = new Bitmap(width, height))
            {
                var g = Graphics.FromImage(newBmp);
                var colorMap = new ColorMap[1];
                colorMap[0] = new ColorMap();
                colorMap[0].OldColor = Color.Fuchsia;
                colorMap[0].NewColor = Color.FromKnownColor(KnownColor.ButtonFace);
                var attr = new ImageAttributes();
                attr.SetRemapTable(colorMap);
                g.DrawImage(bitmapFromResource, new Rectangle(0, 0, width, height), 0, 0, width, height, GraphicsUnit.Pixel, attr);
                tbIcon = Icon.FromHandle(newBmp.GetHicon());
            }
            return tbIcon;
        }

        public static Timer SetTimeOut(Action action, TimeSpan delay)
        {
            var timer = new Timer(_ => action(), null, delay, TimeSpan.FromMilliseconds(-1));
            return timer;
        }
    }
}
