using JsParser.Core.Code;
using JsParser.Core.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JsParser.Core.Infrastructure
{
    public class JsParserEvent: EventArgs
    {
        public ICodeProvider Code { get; set; }
    }

    /// <summary>
    /// Event args for subscribing for Error events 
    /// </summary>
    public class JsParserErrorsNotificationArgs : JsParserEvent
    {
        public string FullFileName { get; set; }
        public List<ErrorMessage> Errors { get; set; }
    }

    public static class JsParserEventsBroadcaster
    {
        static object _lockObj = new object();
        static Dictionary<string, List<Action<JsParserEvent>>> _subscribers
            = new Dictionary<string, List<Action<JsParserEvent>>>();

        public static void Subscribe(Action<JsParserEvent> action, string docIdentifier)
        {
            lock (_lockObj)
            {
                if (!_subscribers.ContainsKey(docIdentifier))
                {
                    _subscribers[docIdentifier] = new List<Action<JsParserEvent>>();
                }

                if (!_subscribers[docIdentifier].Contains(action))
                {
                    _subscribers[docIdentifier].Add(action);
                }
            }
        }

        public static void Unsubscribe(Action<JsParserEvent> action, string docIdentifier)
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

        public static void UpdateSubscription(string oldDocId, string newDocId)
        {
            lock (_lockObj)
            {
                if (_subscribers.ContainsKey(oldDocId))
                {
                    var list = _subscribers[oldDocId];
                    _subscribers.Remove(oldDocId);
                    _subscribers[newDocId] = list;
                }
            }
        }

        public static void FireActionsForDoc(string docIdentifier, JsParserEvent args)
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
