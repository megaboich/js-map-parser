using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JsParser.Package.UI
{
    public static class ErrorNotificationCommunicator
    {
        static object _lockObj = new object();
        static Dictionary<string, List<Action<JsParser.UI.UI.NavigationTreeView.ErrorsNotificationArgs>>> _subscribers
            = new Dictionary<string, List<Action<JsParser.UI.UI.NavigationTreeView.ErrorsNotificationArgs>>>();

        public static void SubscribeForErrors(Action<JsParser.UI.UI.NavigationTreeView.ErrorsNotificationArgs> action, string docIdentifier)
        {
            lock (_lockObj)
            {
                if (!_subscribers.ContainsKey(docIdentifier))
                {
                    _subscribers[docIdentifier] = new List<Action<JsParser.UI.UI.NavigationTreeView.ErrorsNotificationArgs>>();
                }

                if (!_subscribers[docIdentifier].Contains(action))
                {
                    _subscribers[docIdentifier].Add(action);
                }
            }
        }

        public static void UnsubscribeFromErrors(Action<JsParser.UI.UI.NavigationTreeView.ErrorsNotificationArgs> action, string docIdentifier)
        {
            lock (_lockObj)
            {
                if (_subscribers.ContainsKey(docIdentifier))
                {
                    var list = _subscribers[docIdentifier];
                    if (list.Contains(action))
                    {
                        list.Remove(action);
                    }

                    if (list.Count == 0)
                    {
                        _subscribers.Remove(docIdentifier);
                    }
                }
            }
        }

        public static void FireActionsForDoc(string docIdentifier, JsParser.UI.UI.NavigationTreeView.ErrorsNotificationArgs args)
        {
            lock(_lockObj)
            {
                if (_subscribers.ContainsKey(docIdentifier))
                {
                    var list = _subscribers[docIdentifier];
                    foreach(var action in list)
                    {
                        try
                        {
                            action(args);
                        }
                        catch {}
                    }
                }
            }
        }
    }
}
