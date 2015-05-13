using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JsParser.Core.Parsers
{
    public static class TaskListAggregator
    {
        public static IEnumerable<TaskListItem> GetTaskList(IEnumerable<CommentWrapper> comments, string[] todokeywords)
        {
            return comments.
                Select(c => {
                    foreach (var kw in todokeywords)
                    {
                        var todoIndex = c.Spelling.IndexOf(kw, StringComparison.InvariantCultureIgnoreCase);
                        if (todoIndex >= 0)
                        {
                            return new TaskListItem
                            {
                                Description = c.Spelling.Substring(todoIndex),
                                StartLine = c.StartLine,
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
