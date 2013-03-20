using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JsParser.Core.Parsers
{
    public static class TaskListAggregator
    {
        public static IEnumerable<TaskListItem> GetTaskList(IEnumerable<CustomComment> comments, string[] todokeywords)
        {
            return comments.
                Select(c => {
                    foreach (var kw in todokeywords)
                    {
                        var todoIndex = c.Text.IndexOf(kw, StringComparison.InvariantCultureIgnoreCase);
                        if (todoIndex >= 0)
                        {
                            return new TaskListItem
                            {
                                Description = c.Text.Substring(todoIndex),
                                StartLine = c.Start,
                                StartColumn = 0
                            };
                        }
                    }

                    return null;
                })
                .Where(c => c != null);
        }
    }
}
