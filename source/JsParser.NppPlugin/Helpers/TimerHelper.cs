using System;
using System.Collections.Generic;
using System.Threading;

namespace JsMapParser.NppPlugin.Helpers
{
    public static class TimerHelper
    {
        /// <summary>
        /// Storage to keep references to timer objects. We need them to call dispose later and Garbage Collector will not dispose any timers in meantime
        /// </summary>
        private static readonly Dictionary<Guid, Timer> timerReferences = new Dictionary<Guid, Timer>();

        /// <summary>
        /// Creates timer to execute action after delay only once
        /// </summary>
        /// <param name="action"></param>
        /// <param name="delay"></param>
        public static Guid SetTimeOut(Action action, TimeSpan delay)
        {
            lock (timerReferences)
            {
                var timerId = Guid.NewGuid();
                var timer = new Timer(_ =>
                {
                    action();
                    Timer timerInstance;
                    if (timerReferences.TryGetValue(timerId, out timerInstance))
                    {
                        timerInstance.Dispose();
                        timerReferences.Remove(timerId);
                    }
                }, null, delay, TimeSpan.FromMilliseconds(-1));
                timerReferences[timerId] = timer;
                return timerId;
            }
        }
    }
}
