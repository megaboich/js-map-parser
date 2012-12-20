using JsParser.Core.Parsers;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JsParser.Package.Infrastructure
{
    /// <summary>
    /// Event args for subscribing for Error events 
    /// </summary>
    public class ErrorsNotificationArgs : EventArgs
    {
        public string FullFileName { get; set; }
        public List<ErrorMessage> Errors { get; set; }
    }
        

    public static class ErrorNotificationCommunicator
    {
        static object _lockObj = new object();
        static Dictionary<string, List<Action<ErrorsNotificationArgs>>> _subscribers
            = new Dictionary<string, List<Action<ErrorsNotificationArgs>>>();

        public static void SubscribeForErrors(Action<ErrorsNotificationArgs> action, string docIdentifier)
        {
            lock (_lockObj)
            {
                if (!_subscribers.ContainsKey(docIdentifier))
                {
                    _subscribers[docIdentifier] = new List<Action<ErrorsNotificationArgs>>();
                }

                if (!_subscribers[docIdentifier].Contains(action))
                {
                    _subscribers[docIdentifier].Add(action);
                }
            }
        }

        public static void UnsubscribeFromErrors(Action<ErrorsNotificationArgs> action, string docIdentifier)
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

        public static void FireActionsForDoc(string docIdentifier, ErrorsNotificationArgs args)
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
