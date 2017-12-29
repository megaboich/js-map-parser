using System;
using System.Collections.Generic;
using System.Drawing;

namespace JsParser.UI.UI
{
    public class Palette: IDisposable
    {
        private Dictionary<int, Brush> _solidBrushes = new Dictionary<int, Brush>();

        public Brush GetSolidBrush(Color c)
        {
            Brush br;
            if (_solidBrushes.TryGetValue(c.ToArgb(), out br))
            {
                return br;
            }
            else
            {
                br = new SolidBrush(c);
                _solidBrushes.Add(c.ToArgb(), br);
                return br;
            }
        }

        public void CleanUp()
        {
            foreach (var kv in _solidBrushes)
            {
                kv.Value.Dispose();
            }

            _solidBrushes.Clear();
        }

        public void Dispose()
        {
            CleanUp();
        }
    }
}
