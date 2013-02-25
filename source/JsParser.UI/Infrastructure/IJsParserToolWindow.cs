using JsParser.UI.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JsParser.UI.Infrastructure
{
    public interface IJsParserToolWindow
    {
        NavigationTreeView NavigationTreeView { get; }
    }
}
