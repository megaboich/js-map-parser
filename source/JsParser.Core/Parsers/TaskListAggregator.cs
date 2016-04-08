using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JsParser.Core.Parsers
{
    public static class TaskListAggregator
    {
        public static IEnumerable<TaskListItem> GetTaskList(IEnumerable<CommentWrapper> comments, IEnumerable<string> todokeywords)
        {
            return comments.
                Select(c => {
                    foreach (var kw in todokeywords)
                    {
                        var todoIndex = c.Spelling.IndexOf(kw, StringComparison.InvariantCultureIgnoreCase);
                        if (todoIndex >= 0)
                        {
                            var todoClause = c.Spelling.Substring(todoIndex);
                            var endIndex = todoClause.IndexOfAny(new[] { '\r', '\n' });
                            if (endIndex >= 0)
                            {
                                todoClause = todoClause.Substring(0, endIndex);
                            }

                            return new TaskListItem
                            {
                                Description = todoClause,
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
