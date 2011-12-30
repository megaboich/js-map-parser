using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JsParserCore.Parsers
{
    public static class TaskListAggregator
    {
        public static IEnumerable<TaskListItem> GetTaskList(IEnumerable<CustomComment> comments)
        {
            return comments.
                Select(c => {
                    var todoIndex = c.Text.IndexOf("TODO", StringComparison.InvariantCultureIgnoreCase);
                    if (todoIndex >= 0)
                    {
                        return new TaskListItem
                        {
                            Description = c.Text.Substring(todoIndex),
                            StartLine = c.Start,
                            StartColumn = 0
                        };
                    }
                    return null;
                })
                .Where(c => c != null);
        }
    }
}
