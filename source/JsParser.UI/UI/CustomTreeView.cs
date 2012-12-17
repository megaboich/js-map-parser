using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;

namespace JsParser.UI.UI
{
    public class CustomTreeView : TreeView
    {
        private const int WM_HSCROL = 0x114;
        private const int WM_VSCROLL = 0x115;
        private const int WM_MOUSEWHEEL = 0x20A;

        public enum ScrollType
        {
            Vertical,
            Horizontal
        }

        public class ScrollEventArgs: EventArgs
        {
            public ScrollType ScrollType{get;set;}
        }

        public delegate void ScrollEventHandler(object sender, ScrollEventArgs e);

        [Description("UserControlOnLoadDescr")]
        [Category("CatBehavior")]
        public event ScrollEventHandler OnScroll;

        public CustomTreeView()
            : base()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }

        protected override void WndProc(ref Message m)
        {
            var scrollType = ScrollType.Vertical;
            switch (m.Msg)
            {
                case WM_HSCROL:
                    scrollType = ScrollType.Horizontal;
                    goto action;
                case WM_MOUSEWHEEL:
                case WM_VSCROLL:
                    action:;
                    OnScroll.Invoke(
                        this,
                        new ScrollEventArgs { ScrollType = scrollType });
                    break;
            }

            base.WndProc(ref m);
        }
    }
}
