using System.Drawing;
using System.Drawing.Imaging;

namespace JsMapParser.NppPlugin.Helpers
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
    }
}
